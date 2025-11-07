 
using UnityEngine;

public enum StatsType
{
    gamesPlayed,

    gamesWon,

    winRate,

    highScore,

    moves,

    shortestTime,

    avgTimePerGame,

    totalTimePlayedGame,
}
public class StatsSettings : MonoBehaviour
{
    private static StatsSettings _instance = null;
    public static StatsSettings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<StatsSettings>();
                if (_instance == null)
                {
                    throw new UnityException("StatsSetting Not In Hierarchy ");
                }
            }
            return _instance;
        }
    }


    public int[] gamesPlayed;

    public int[] gamesWon;

    public int[] winRate;

    public int[] highScore;

    public int[] moves;

    public int[] shortestTime;

    public int[] avgTimePerGame;

    public int[] totalTimePlayedGame;

    private StatsData statsData = new StatsData();
    public class StatsData
    {
        public string gamesPlayedData;
        public string gamesWonData;
        public string winRateData;
        public string highScoreData;
        public string movesData;
        public string shortestTimeData;
        public string avgTimePerGameData;
        public string totalTimePlayedGameData;
    }


    private void Start()
    {
        Load();
    }

    public void ResetAllStats()
    {
        const int COUNT_GAME_TYPE = 3;
       highScore = new int[COUNT_GAME_TYPE];
       gamesPlayed = new int[COUNT_GAME_TYPE];
       gamesWon = new int[COUNT_GAME_TYPE];
       winRate = new int[COUNT_GAME_TYPE];
       moves = new int[COUNT_GAME_TYPE];
       shortestTime = new int[COUNT_GAME_TYPE];
       avgTimePerGame = new int[COUNT_GAME_TYPE];
        totalTimePlayedGame = new int[COUNT_GAME_TYPE];


        moves = new int[COUNT_GAME_TYPE];
        for (int i = 0; i < moves.Length; i++)
        {
            moves[i] = 9999;
        }
        Save();
    }

    public void Save()
    {
        statsData.gamesPlayedData = JsonHelper.ToJson<int>(gamesPlayed);
        statsData.gamesWonData = JsonHelper.ToJson<int>(gamesWon);
        statsData.winRateData = JsonHelper.ToJson<int>(winRate);
        statsData.highScoreData = JsonHelper.ToJson<int>(highScore);
        statsData.movesData = JsonHelper.ToJson<int>(moves);
        statsData.shortestTimeData = JsonHelper.ToJson<int>(shortestTime);
        statsData.avgTimePerGameData = JsonHelper.ToJson<int>(avgTimePerGame) ;
        statsData.totalTimePlayedGameData = JsonHelper.ToJson<int>(totalTimePlayedGame);

        string data = JsonUtility.ToJson(statsData);

        PlayerPrefs.SetString("StatsGame", data);

    }

    public void Load()
    {
        string data = PlayerPrefs.GetString("StatsGame", string.Empty);
        if (string.IsNullOrEmpty(data))
        {
            ResetAllStats();
        }
        else
        {
            statsData = JsonUtility.FromJson<StatsData>(data);
            gamesPlayed = JsonHelper.FromJson<int>(statsData.gamesPlayedData);
            gamesWon = JsonHelper.FromJson<int>(statsData.gamesWonData);
            winRate = JsonHelper.FromJson<int>(statsData.winRateData);
            highScore = JsonHelper.FromJson<int>(statsData.highScoreData);
            moves = JsonHelper.FromJson<int>(statsData.movesData);
            shortestTime = JsonHelper.FromJson<int>(statsData.shortestTimeData);
            avgTimePerGame = JsonHelper.FromJson<int>(statsData.avgTimePerGameData);
            totalTimePlayedGame = JsonHelper.FromJson<int>(statsData.totalTimePlayedGameData);
        }
    }


    public void UpdateStats(int position, StatsType statsType, int value)
    {
        switch (statsType)
        {
            case StatsType.gamesPlayed:
                gamesPlayed[position] += value;
                break;
            case StatsType.gamesWon:
                gamesWon[position] += value;
                break;
            case StatsType.winRate:
                winRate[position] = value;
                break;
            case StatsType.highScore:
                highScore[position] = value;
                break;
            case StatsType.moves:
                moves[position] = value;
                break;
       
            case StatsType.shortestTime:
                shortestTime[position] = value;
                break;
            case StatsType.avgTimePerGame:
                avgTimePerGame[position] = value;
                break;
            case StatsType.totalTimePlayedGame:
                totalTimePlayedGame[position] += value;
                break;

        }

        Save();
    }


    

	

	
}