using UnityEngine;

public class ManagerLogic 
{
	public const int SCORE_TIME_DECREASE = -20;
	public const int SCORE_VEGAS_DECREASE = -52;
	public const float SCORE_TIME = 10;
	public const float HINT_TIME = 10;
	public const int DECK_TURN_LIMIT = 2;

	public int DecreaseTimeScore {get{ return SCORE_TIME_DECREASE;}}


	public int moves;
	public float timer;
	public int score;

	public solitaireTimer scoreIncreaseTimer = new solitaireTimer(SCORE_TIME);
	public solitaireTimer hintTimer = new solitaireTimer(HINT_TIME);

	public int countDeckTurn;

	public bool isGameWin;
	public bool isAllowBlinkHint;
	public bool isAllowAutoComplete;
	public bool isAllowFinish;

	public bool HasDeckTurn
	{
		get { return countDeckTurn < DECK_TURN_LIMIT;}
	}

	// ref it
	public void InitScoring()
	{
		moves = 0;
		timer = 0;
		score = 0;
		if (GameSettings.Instance.isComulativeVegasSet)
			score = (GameSettings.Instance.isOneCardSet) ? GameSettings.Instance.scoreComulativeVegasOne : GameSettings.Instance.scoreComulativeVegasThree;
	 

		scoreIncreaseTimer.Clear ();
		hintTimer.Clear ();
		isAllowBlinkHint = true;
		GameSettings.Instance.isGameStarted = false;
		isGameWin = false;
		countDeckTurn = 0;
		isAllowAutoComplete = true;
		isAllowFinish = true;
	}

    public void ResetBlinkCard()
    {
        isAllowBlinkHint = true;
    }

    public void AddScore(int value)
    {
        score += value;

        if (score < 0) score = 0;
 
        HUDController.instance.SetScore(score); // dependency
    }

    public void SetScore(int value)
    {
        Debug.Log("value " + value);
        score  = value;

       

        HUDController.instance.SetScore(score); // dependency
    }
    public void SetBeginTimer(int timer)
    {
        this.timer = timer;
        HUDController.instance.SetTime(timer); // dependency
    }
    public void SetBeginMove(int moves)
    {
        this.moves = moves;
        HUDController.instance.SetMove(moves); // dependency
    }

    public void AddMoves()
	{
		HUDController.instance.SetMove(++moves); // dependency
	}

	#region Stats
	public void SetStopStatsAndSetting()
	{
		GameSettings.Instance.isGameWon = isGameWin;
	 
		if (GameSettings.Instance.isComulativeVegasSet) SetVegas (score);
	}
 
	private void SetVegas (int score)
	{	
		if (GameSettings.Instance.isOneCardSet) {
			GameSettings.Instance.scoreComulativeVegasOne = score;
		} else {
			GameSettings.Instance.scoreComulativeVegasThree = score;
		}
	}
	public void SetStatsScoreAndTime(int score, int time,int moves)
	{
       
		int statScore = score;
		int statTime = (int) time;
        int statMoves = moves ;
        int solitaireType = GetSolitaireType ();
        StatsSettings.Instance.totalTimePlayedGame[solitaireType] += time;
        StatsSettings.Instance.UpdateStats(solitaireType, StatsType.gamesWon, 1);
		if (statScore > StatsSettings.Instance.highScore [solitaireType]) {
            StatsSettings.Instance.UpdateStats(solitaireType, StatsType.highScore, statScore);
           
		}
		if (StatsSettings.Instance.shortestTime [solitaireType].Equals (0)) {
            StatsSettings.Instance.UpdateStats(solitaireType, StatsType.shortestTime, statTime);
             
		}
		if (statTime < StatsSettings.Instance.shortestTime [solitaireType]) {
            StatsSettings.Instance.UpdateStats(solitaireType, StatsType.shortestTime, statTime);
        }


        if (StatsSettings.Instance.moves[solitaireType].Equals(0))
        {
            StatsSettings.Instance.UpdateStats(solitaireType, StatsType.moves, statMoves);
           
        }
        if (statMoves < StatsSettings.Instance.moves[solitaireType])
        {
            StatsSettings.Instance.UpdateStats(solitaireType, StatsType.moves, statMoves);
            
        }
    }

	public int GetGameTypeIndex()
	{
	 
		return GetSolitaireType();
	}

	#endregion
	public void StartCountGame()
	{
		hintTimer.Clear ();
		isAllowBlinkHint = true; // ? Try del it and check
		if (GameSettings.Instance.isGameStarted) return;
		GameSettings.Instance.isGameStarted = true;
		int solitaireType = GetSolitaireType ();
        StatsSettings.Instance.UpdateStats(solitaireType, StatsType.gamesPlayed, 1);
        
	}
	public int GetSolitaireType()
	{
        if (GameSettings.Instance.numberOfSuit == 2) return 1;
        if (GameSettings.Instance.numberOfSuit == 4) return 2;
        return 0;
    }
}