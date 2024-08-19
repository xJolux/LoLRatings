using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.ML;
using Newtonsoft.Json.Linq;
using System.Linq;


namespace LoLRatings.Data.Prediction
{
    public static class PredictionManager
    {
        public static PredictionEngine<MatchData, MatchEndTimePrediction> _predictionEngine;
        private static int _lastDataCount = 0;

        public static List<MatchData> GetTrainingsDataList()
        {
            string jsonPath = Constants.TRANINGS_DATA_JSON_PATH;
            string json = File.ReadAllText(jsonPath);
            var data = JsonConvert.DeserializeObject<List<MatchData>>(json);

            return data;
        }

        public static void SaveTrainingsDataList(List<MatchData> data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(Constants.TRANINGS_DATA_JSON_PATH, json);
        }

        public static float PredictEndTime(float ratingPercentDiff)
        {
            if (_predictionEngine != null)
            {
                return _predictionEngine.Predict(new MatchData { ratingPercentDiff = ratingPercentDiff }).PredictedEndTime;
            }

            return 0;
        }

        public static void UpdateTrainingsDataList(GameData gameData, float ratingPercentDiff)
        {
            // check if data is 
            if (gameData.EventJsonDataList.Count() < 60 || gameData.GameMode != "CLASSIC" || !gameData.IsLive)
            {
                return;
            }

            // Get last event data
            JToken lastEvent = gameData.EventJsonDataList.Last();
            int lastEventTime = lastEvent["EventTime"]?.ToObject<int>() ?? 0;

            if (lastEvent == null || lastEvent["EventName"]?.ToString() != "GameEnd" || lastEventTime == 0)
            {
                return;
            }

            // create new match training data
            var trainingDataList = GetTrainingsDataList();
            MatchData newMatchData = new MatchData { ratingPercentDiff = ratingPercentDiff, endTime = lastEventTime };

            if (trainingDataList.Any(rd => rd.ratingPercentDiff == newMatchData.ratingPercentDiff && rd.endTime == newMatchData.endTime))
            {
                return;
            }

            // add new match training data
            trainingDataList.Add(newMatchData);
            SaveTrainingsDataList(trainingDataList);
        }

        public static void UpdatePredectionEngine()
        {
            // get data
            var trainingDataList = GetTrainingsDataList();

            if (_lastDataCount != trainingDataList.Count && trainingDataList.Count > 0)
            {
                // init MLContext
                var mlContext = new MLContext();

                // load training data
                var trainingData = mlContext.Data.LoadFromEnumerable(trainingDataList);

                // create the learning pipeline
                var pipeline = mlContext.Transforms.Concatenate("Features", "ratingPercentDiff")
                    .Append(mlContext.Transforms.NormalizeMeanVariance("Features"))
                    .Append(mlContext.Regression.Trainers.LbfgsPoissonRegression(labelColumnName: "endTime"));

                // train the model
                var model = pipeline.Fit(trainingData);

                // create prediction engine
                _predictionEngine = mlContext.Model.CreatePredictionEngine<MatchData, MatchEndTimePrediction>(model);

                _lastDataCount = trainingDataList.Count;
            }
            else if (trainingDataList.Count == 0)
            {
                // clear prediction engine
                _predictionEngine = null;
                _lastDataCount = 0;
            }
        }
    }
}
