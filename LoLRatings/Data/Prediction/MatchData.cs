using Microsoft.ML.Data;


namespace LoLRatings.Data.Prediction
{
    public class MatchData
    {
        public float ratingPercentDiff { get; set; }
        public float endTime { get; set; }
    }

    public class MatchEndTimePrediction
    {
        [ColumnName("Score")]
        public float PredictedEndTime { get; set; }
    }
}