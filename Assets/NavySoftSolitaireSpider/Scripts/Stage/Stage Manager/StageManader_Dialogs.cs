using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UserWindow;
 
using Convert;
public partial class StageManager
{
    #region PopUpWindow
    private void PopUpBackToCalendar()
    {
        OnBreakGameReturnToCalendarClose();


    }

    private void PopUpReturnToCalendar()
    {
 
        if (managerLogic.isGameWin)
        {
            SpawnRecordTableCallback();
        }

    }
    private void SaveResultCallback(bool isSaved)
    {
        //ServerGameAPI.Instance.Get();
    }
    private void SpawnRecordTableCallback()
    {
        ResultTextLineData titleLineData = null;
        if (managerLogic.isGameWin)
        {
            titleLineData = new ResultTextLineData("Total Score", Color.white, true, StringsConvert.ConvertToComaFormat(managerLogic.score), Color.green);
        }
        List<ResultTextLineData> listLinesData = new List<ResultTextLineData>();

        ResultTextLineData line;
        line = new ResultTextLineData("", Color.white);
        listLinesData.Add(line);
        line = new ResultTextLineData("Pleace check your network connection", Color.white);
        listLinesData.Add(line);
        List<ResultButtonData> buttonData = new List<ResultButtonData>();

        buttonData.Add(new ResultButtonData("Continue", 1, OnReturnCalendarClose));

        PopUpManager.Instance.ShowScroll(titleLineData, listLinesData, managerLogic.isGameWin, buttonData);
    }



    private void PopUpReturnToMenu()
    {
        ResultTextLineData titleLineData = null;

        int gameTypeIndex = managerLogic.GetGameTypeIndex();
        int bestScoreInt = StatsSettings.Instance.highScore[gameTypeIndex];
        int bestTimeInt = StatsSettings.Instance.shortestTime[gameTypeIndex];
        int bestMove = StatsSettings.Instance.moves[gameTypeIndex];
   
        if(managerLogic.score > bestScoreInt)
        {
            bestScoreInt = managerLogic.score;
            StatsSettings.Instance.UpdateStats(gameTypeIndex, StatsType.highScore, managerLogic.score);
        }
        string totalScore = managerLogic.score.ToString();
        string bestScore = bestScoreInt.ToString();

        string move = managerLogic.moves.ToString();





        string playTime = StringsConvert.ConvertToMinutesSeconds(((int)managerLogic.timer));
        string bestTime = StringsConvert.ConvertToMinutesSeconds(bestTimeInt);

        string winRate = StringsConvert.ConcatPersent(StatsSettings.Instance.winRate[gameTypeIndex]);

        bool isHighScore = (managerLogic.isGameWin && (managerLogic.score > bestScoreInt));
        bool isHighTime = (managerLogic.isGameWin && ((int)managerLogic.timer < bestTimeInt));


        Color color = (managerLogic.isGameWin) ? Color.green : Color.white;

        List<ResultTextLineData> listLinesData = new List<ResultTextLineData>();

        listLinesData.Add(new ResultTextLineData(totalScore, Color.white, false, bestScore, Color.white));
        listLinesData.Add(new ResultTextLineData(playTime, Color.white, false, bestTime, Color.white));
        listLinesData.Add(new ResultTextLineData(move, Color.white, false, bestMove.ToString(), Color.white));

      


        List<ResultButtonData> buttonData = new List<ResultButtonData>();


   
 
        buttonData.Add(new ResultButtonData("New Game", 0, OnWinDealGame));
   



        PopUpManager.Instance.ShowResult(titleLineData, listLinesData, managerLogic.isGameWin, buttonData);
    }

    private void OnReturnContinue()
    {
        PopUpManager.Instance.Close();
    }
    private void OnReturnSolution()
    {
        PopUpManager.Instance.Close();
        StartCoroutine(ShowSolution());
    }
    private void OnReturnRetry()
    {
        PopUpManager.Instance.Close();
        GeneralRestartGame();
    }
    private void OnReturnReplay()
    {
        PopUpManager.Instance.Close();
        //AdvertisementsManager.Instance.ShowInterstitial();
        GeneralRestartGame();
    }
    private void OnReturnShare()
    {
        string textToShare = CreatePostText();

    }
    private string CreatePostText()
    {
        string prefix = "Try to beat my score points!";
        string url = GameSettings.Instance.url;
        string textToShare = prefix + "\n" + url;
        return textToShare;
    }

    private void OnReturnCalendarClose()
    {
        PopUpManager.Instance.Close();
        managerLogic.SetStopStatsAndSetting();
       
        GeneralBackToCalendar();
    }
    private void OnReturnMenuClose()
    {
        PopUpManager.Instance.Close();
        managerLogic.SetStopStatsAndSetting();
        GeneralNewGame();
    }
    private void OnWinDealGame()
    {
        GameSettings.Instance.randomDeal = false;
        PopUpManager.Instance.Close();
        //AdvertisementsManager.Instance.ShowInterstitial();
        managerLogic.SetStopStatsAndSetting();
        GeneralNewGame();
        HUDController.instance.VisibleLayout(true);
    }

    private void OnRandomDealGame()
    {
        GameSettings.Instance.randomDeal = true;
        //AdvertisementsManager.Instance.ShowInterstitial();
        PopUpManager.Instance.Close();
        managerLogic.SetStopStatsAndSetting();
        GeneralNewGame();
    }
    private void OnStageCancel()
    {
        PopUpManager.Instance.Close();
    }
    private void OnBreakGameReturnToCalendarClose()
    {
        GameSettings.Instance.isCalendarGame = false;
        PopUpManager.Instance.Close();

        managerLogic.SetStopStatsAndSetting();
        GeneralBackToCalendar();
    }
    // ---
    private void PopUpExitSolution()
    {
        string titleLineData = "EXIT FROM SOLUTION";
        string listLinesData = "Would you like to retry this game?";

        List<ResultButtonData> buttonData = new List<ResultButtonData>();
        buttonData.Add(new ResultButtonData("New Game", 2, OnSolutionNewGame));
        buttonData.Add(new ResultButtonData("Restart", 0, OnSolutionRestart));

        PopUpManager.Instance.ShowDialog(titleLineData, listLinesData, buttonData);
    }
    private void OnSolutionNewGame()
    {
        PopUpManager.Instance.Close();
        managerLogic.SetStopStatsAndSetting();
        GeneralNewGame();
    }
    private void OnSolutionRestart()
    {
        PopUpManager.Instance.Close();
        GeneralRestartGame();
    }
    // ---
    private void PopUpAutoComplete()
    {

       
    }
    public void OnAuto()
    {

        AutoCompleteGame();

    }
    private void OnManual()
    {
        PopUpManager.Instance.Close();
    }
    #endregion

    private void LoadCalendarScene()
    {
        PopUpBackToCalendar();
    }
}











