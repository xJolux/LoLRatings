using LoLRatings.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;


public class MainViewModel : INotifyPropertyChanged
{
    private ObservableCollection<Player> _playerList;
    private string _winPercentBlue;
    private string _winPercentRed;
    private string _endTime;
    private string _ratingPercentDiff;

    public MainViewModel()
    {
        _playerList = new ObservableCollection<Player>();
    }

    public ObservableCollection<Player> PlayerList
    {
        get { return _playerList; }
        set { _playerList = value; OnPropertyChanged(nameof(PlayerList)); }
    }

    public string WinPercentBlue
    {
        get { return _winPercentBlue; }
        set { _winPercentBlue = value; OnPropertyChanged(nameof(WinPercentBlue)); }
    }

    public string WinPercentRed
    {
        get { return _winPercentRed; }
        set { _winPercentRed = value; OnPropertyChanged(nameof(WinPercentRed)); }
    }

    public string EndTime
    {
        get { return _endTime; }
        set { _endTime = value; OnPropertyChanged(nameof(EndTime)); }
    }

    public string RatingPercentDiff
    {
        get { return _ratingPercentDiff; }
        set { _ratingPercentDiff = value; OnPropertyChanged(nameof(RatingPercentDiff)); }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void UpdatePlayerList(List<Player> players)
    {
        PlayerList.Clear();
        foreach (Player player in players)
        {
            PlayerList.Add(player);
        }
    }
}
