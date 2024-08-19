using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Windows.Interop;

using LoLRatings.Data;
using LoLRatings.Data.Prediction;
using System.Diagnostics;


namespace LoLRatings.Ui
{
    public partial class MainWindow : Window
    {
        private MainViewModel _mainViewModel;

        private readonly DispatcherTimer _autoUpdateTimer;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            // Make window transparent/clickthrough
            var hwnd = new WindowInteropHelper(this).Handle;
            WindowsServices.SetWindowExTransparent(hwnd);
        }

        public MainWindow()
        {
            InitializeComponent();

            // Move window to top left
            Left = 0;
            Top = 0;

            // Init view model
            _mainViewModel = new MainViewModel();
            DataContext = _mainViewModel;

            // Init/start auto update
            _autoUpdateTimer = new DispatcherTimer();
            _autoUpdateTimer.Interval = TimeSpan.FromMilliseconds(Constants.UPDATE_INTERVAL);
            _autoUpdateTimer.Tick += AutoUpdateTimer_Tick;
            _autoUpdateTimer.Start();
        }

        private async void AutoUpdateTimer_Tick(object sender, EventArgs e)
        {
            // Get live data
            JObject allLiveJsonData = await LiveDataFetcher.GetLiveData();

            if (allLiveJsonData == null)
            {
                return;
            }

            // Get game data
            GameData gameData = new GameData(allLiveJsonData);

            if (!gameData.IsAvailable() || !Game.AVAILABLE_GAME_MODES.Contains(gameData.GameMode))
            {
                return;
            }

            // Get player repository
            PlayerRepository playerRepository = new PlayerRepository(gameData.PlayerJsonDataList);

            // Update player ratings with passive stats
            if (!PlayerEvaluator.UpdatePlayerListPassive(playerRepository, gameData.PlayerJsonDataList))
            {
                return;
            }

            // Update player ratings with events
            if (!PlayerEvaluator.UpdatePlayerListEvents(playerRepository, gameData.EventJsonDataList))
            {
                return; 
            }

            // Update player statistics with new updated player ratings
            playerRepository.UpdatePlayerStatistics();

            // Update display
            UpdatePlayerRatingsDisplay(playerRepository, gameData);
        }

        private void UpdatePlayerRatingsDisplay(PlayerRepository playerRepository, GameData gameData)
        {
            // Get game statistics
            GameStatistics gameStatistics = new GameStatistics(playerRepository);

            // Update win percent
            var winPercentages = gameStatistics.GetWinPercentages();
            _mainViewModel.WinPercentBlue = $"{Math.Round(winPercentages[Game.ORDER], 1)} %";
            _mainViewModel.WinPercentRed = $"{Math.Round(winPercentages[Game.CHAOS], 1)} %";

            // Update trainings data and prediction engine
            PredictionManager.UpdateTrainingsDataList(gameData, gameStatistics.GetRatingPercentDiff());
            PredictionManager.UpdatePredectionEngine();

            // Update end time
            float endTime = gameStatistics.GetEndTime();
            int minutes = (int)(endTime / 60);
            int seconds = (int)(endTime % 60);
            _mainViewModel.EndTime = $"{minutes:00}:{seconds:00}";

            // Update rating percent diff
            float ratingPercentDiff = gameStatistics.GetRatingPercentDiff();
            _mainViewModel.RatingPercentDiff = Math.Round(ratingPercentDiff, 1).ToString();

            // Update player list
            _mainViewModel.UpdatePlayerList(playerRepository.PlayerList);
        }
    }

    public static class WindowsServices
    {
        private const int WS_EX_TRANSPARENT = 0x00000020;
        private const int GWL_EXSTYLE = (-20);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        public static void SetWindowExTransparent(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }
    }
}
