using UnityEngine;

[System.Serializable]
public class PrefMainData
{
	[System.Serializable]
	public class RecordPref
	{
		// restore this from PlayerPref
		public bool isSoundSet;
		public bool isStandardSet;
		public bool isComulativeVegasSet;
		public bool isOneCardSet;
		public bool isAutoTapMoveSet;
		public bool isAutoHintSet;
		public bool isHandSet;
		public bool isScoreSet;
		public bool isMoveTimeSet;
		public bool isEffectSet;
		public bool isCongratsScreenSet;
		public int visualPlayBackgroundSet;
		public int visualCardBacksSet;
        public int visualCardFaceSet;
        public int scoreComulativeVegasOne;
		public int scoreComulativeVegasThree;
        public int orientationType;
        public string cardData;

      


        public int danceCurrentCounter;
        public bool isHighLightMode;
        public int numberOfSuit;
    }
	public RecordPref settings;
}

public static class PlayerPrefAPI 
{
	public static void Set()
	{
		PrefMainData parsedData = new PrefMainData();
		parsedData.settings = new PrefMainData.RecordPref ();
		parsedData.settings.isSoundSet = GameSettings.Instance.isSoundSet;
		parsedData.settings.isStandardSet = GameSettings.Instance.isStandardSet;
		parsedData.settings.isComulativeVegasSet = GameSettings.Instance.isComulativeVegasSet;
		parsedData.settings.isOneCardSet = GameSettings.Instance.isOneCardSet;
		parsedData.settings.isAutoTapMoveSet = GameSettings.Instance.isAutoTapMoveSet;
		parsedData.settings.isAutoHintSet = GameSettings.Instance.isAutoHintSet;
		parsedData.settings.isHandSet = GameSettings.Instance.isHandSet;
		parsedData.settings.isScoreSet = GameSettings.Instance.isScoreSet;
		parsedData.settings.isMoveTimeSet = GameSettings.Instance.isMoveTimeSet;
		parsedData.settings.isEffectSet = GameSettings.Instance.isEffectSet;
		parsedData.settings.isCongratsScreenSet = GameSettings.Instance.isCongratsScreenSet;
		parsedData.settings.visualPlayBackgroundSet = GameSettings.Instance.visualPlayBackgroundSet;
		parsedData.settings.visualCardBacksSet = GameSettings.Instance.visualCardBacksSet;
        parsedData.settings.visualCardFaceSet = GameSettings.Instance.visualCardFacesSet;
        parsedData.settings.scoreComulativeVegasOne = GameSettings.Instance.scoreComulativeVegasOne;
		parsedData.settings.scoreComulativeVegasThree = GameSettings.Instance.scoreComulativeVegasThree;
        parsedData.settings.numberOfSuit = GameSettings.Instance.numberOfSuit;
        parsedData.settings.danceCurrentCounter = GameSettings.Instance.danceCurrentCounter;
        parsedData.settings.orientationType = (int)GameSettings.Instance.orientationType;
        parsedData.settings.cardData =  GameSettings.Instance.calendarData;
        parsedData.settings.isHighLightMode = GameSettings.Instance.isHighLightMode;


    


        string rawSetData = JsonUtility.ToJson (parsedData);
 
        PlayerPrefs.SetString("Setting",rawSetData);
		PlayerPrefs.Save ();
	}

	public static void Get()
	{
		if (PlayerPrefs.HasKey ("Setting"))
		{
			string rawGetData = PlayerPrefs.GetString ("Setting");
			PrefMainData parsedData = ParseSetting(rawGetData);

			GameSettings.Instance.isSoundSet = parsedData.settings.isSoundSet;
			GameSettings.Instance.isStandardSet = parsedData.settings.isStandardSet;
			GameSettings.Instance.isComulativeVegasSet = parsedData.settings.isComulativeVegasSet;
			GameSettings.Instance.isOneCardSet = parsedData.settings.isOneCardSet;
			GameSettings.Instance.isAutoTapMoveSet = parsedData.settings.isAutoTapMoveSet;
			GameSettings.Instance.isAutoHintSet = parsedData.settings.isAutoHintSet;
			GameSettings.Instance.isHandSet = parsedData.settings.isHandSet;
			GameSettings.Instance.isScoreSet = parsedData.settings.isScoreSet;
			GameSettings.Instance.isMoveTimeSet = parsedData.settings.isMoveTimeSet;
			GameSettings.Instance.isEffectSet = parsedData.settings.isEffectSet;
			GameSettings.Instance.isCongratsScreenSet = parsedData.settings.isCongratsScreenSet;
			GameSettings.Instance.visualPlayBackgroundSet = parsedData.settings.visualPlayBackgroundSet;
			GameSettings.Instance.visualCardBacksSet = parsedData.settings.visualCardBacksSet;
            GameSettings.Instance.visualCardFacesSet = parsedData.settings.visualCardFaceSet;
            GameSettings.Instance.numberOfSuit = parsedData.settings.numberOfSuit;
            GameSettings.Instance.scoreComulativeVegasOne = parsedData.settings.scoreComulativeVegasOne;
			GameSettings.Instance.scoreComulativeVegasThree = parsedData.settings.scoreComulativeVegasThree;
            GameSettings.Instance.calendarData = parsedData.settings.cardData;
            GameSettings.Instance.isHighLightMode = parsedData.settings.isHighLightMode;



            switch (parsedData.settings.orientationType)
            {
                case 0: GameSettings.Instance.orientationType = GameSettings.OrientationType.Auto; break;
                case 1: GameSettings.Instance.orientationType = GameSettings.OrientationType.Portrait; break;
                case 2: GameSettings.Instance.orientationType = GameSettings.OrientationType.LandSpace; break;
            }
            GameSettings.Instance.danceCurrentCounter = parsedData.settings.danceCurrentCounter;

        


        }
		else {
			ApplyDefaultGameSettings ();
//			throw new UnityException ("There are no Setting in PlayerPref");
		}

	}

    static void ApplyDefaultGameSettings()
    {
        GameSettings.Instance.isSoundSet = true;
        GameSettings.Instance.isStandardSet = true;
        GameSettings.Instance.isComulativeVegasSet = false;
        GameSettings.Instance.isOneCardSet = false; // NOTE: default Draw is 3 Cards for Setting
        GameSettings.Instance.isAutoTapMoveSet = true;
        GameSettings.Instance.isAutoHintSet = true;
        GameSettings.Instance.isHandSet = false;
        GameSettings.Instance.isScoreSet = true;
        GameSettings.Instance.isMoveTimeSet = true;
        GameSettings.Instance.isHighLightMode = true;
         GameSettings.Instance.isEffectSet = true;
        GameSettings.Instance.isCongratsScreenSet = true;
        GameSettings.Instance.visualPlayBackgroundSet = 0;
        GameSettings.Instance.visualCardBacksSet = 0;
        GameSettings.Instance.visualCardFacesSet = 0;
        GameSettings.Instance.numberOfSuit = 1;
        GameSettings.Instance.scoreComulativeVegasOne = 0;
        GameSettings.Instance.scoreComulativeVegasThree = 0;
        GameSettings.Instance.orientationType = GameSettings.OrientationType.Auto;
        GameSettings.Instance.danceCurrentCounter = 0;
        GameSettings.Instance.calendarData = string.Empty;
     
    }


	private static PrefMainData ParseSetting (string rawGetData)
	{
		PrefMainData parsedData;
		try
		{
			parsedData = JsonUtility.FromJson<PrefMainData>(rawGetData);
		}
		catch
		{
			parsedData = null;
		}
		return parsedData;
	}


    public static void SaveDataStep(string rawData)
    {
      //  Debug.Log("ContinueGame " + rawData);
        PlayerPrefs.SetString("ContinueGame", rawData);
    }
    public static void SaveDataRemainCardInSlot(string rawData)
    {
     
        PlayerPrefs.SetString("RemainCardInSlot", rawData);
    }

  

    public static string LoadDataStep()
    {
        return PlayerPrefs.GetString("ContinueGame");
    }


    public static void SaveStockCard(string rawData)
    {
        

        PlayerPrefs.SetString("StockCard", rawData);
    }
    public static void SaveFoundCard(string rawData)
    {

 
        PlayerPrefs.SetString("FoundCard", rawData);
    }


    public static string LoadStockCard()
    {
     
        return PlayerPrefs.GetString("StockCard");
    }

 
    public static string LoadDataRemainCardInSlot( )
    {
       return  PlayerPrefs.GetString("RemainCardInSlot");
    }



    public static string LoadFoundCard()
    {
//        Debug.Log("Data Found " + PlayerPrefs.GetString("FoundCard"));
        return PlayerPrefs.GetString("FoundCard");
    }


  
    public static void SaveMove(int move)
    {

        PlayerPrefs.SetInt("Move", move);
    }

    public static int LoadMove()
    {
        return PlayerPrefs.GetInt("Move");
    }

    public static void SaveTimer(int timer)
    {

        PlayerPrefs.SetInt("Time", timer);
    }

    public static int LoadTime()
    {
        return PlayerPrefs.GetInt("Time");
    }
    public static void SaveScore(int score)
    {

        PlayerPrefs.SetInt("Score", score);
    }

    public static int LoadScore()
    {

     return   PlayerPrefs.GetInt("Score", 0);
    }
   


}