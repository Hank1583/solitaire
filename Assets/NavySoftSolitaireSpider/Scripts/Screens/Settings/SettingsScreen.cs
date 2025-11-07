using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UserWindow;
public class SettingsScreen : MonoBehaviour
{

    [SerializeField]
    private SwitchScript viewScoreTimeSwitch;
    [SerializeField]
    private SwitchScript highLightFreeCardsSwitch;
    [SerializeField]
    private SwitchScript soundSwitch;
    [SerializeField]
    private SwitchScript handSwitch;
    [SerializeField]
    private SwitchScript effectsSwitch;
 
    [SerializeField]
    private SwitchScript playAutoHintSwitch;
    [SerializeField]
    private SwitchScript playAutoTapMoveSwitch;



    [SerializeField]
    private GameObject[] orientationObj;

    [SerializeField]
    private GameObject[] suitCardObjs;


    [SerializeField]
    private SwitchScript CongratsScreenSwitch;
    [SerializeField]
    private Image visualPlayBackgroundImage;
    [SerializeField]
    private Image visualCardBackImage;
    [SerializeField]
    private Image visualCardFaceImage;


    private int prevSuitCard = 0;

    private void Start()
    {
        if (GameSettings.Instance.isSoundSet) Sound.Instance.Shift();

        Init();
        InitVisualStyle();

    }

    private void Init()
    {
        soundSwitch.Init(GameSettings.Instance.isSoundSet);

        playAutoTapMoveSwitch.Init(GameSettings.Instance.isAutoTapMoveSet);
        playAutoHintSwitch.Init(GameSettings.Instance.isAutoHintSet);
        highLightFreeCardsSwitch.Init(GameSettings.Instance.isHighLightMode);
        ShowOrientationSet();
        handSwitch.Init(GameSettings.Instance.isHandSet);
        effectsSwitch.Init(GameSettings.Instance.isEffectSet);
        viewScoreTimeSwitch.Init(GameSettings.Instance.isMoveTimeSet);

        ShowDrawCardSet(GameSettings.Instance.numberOfSuit);
        prevSuitCard = GameSettings.Instance.numberOfSuit;
        
    }
    private void InitVisualStyle()
    {
        visualPlayBackgroundImage.sprite = ImageSettings.Instance.background[GameSettings.Instance.visualPlayBackgroundSet];
        visualCardBackImage.sprite = ImageSettings.Instance.cardbackHiResolurion[GameSettings.Instance.visualCardBacksSet];
        visualCardFaceImage.sprite = ImageSettings.Instance.cardFacesIcon[GameSettings.Instance.visualCardFacesSet];
    }

    #region Utilits

    private void SetScoring(bool isStandard, bool isComulative)
    {

        GameSettings.Instance.isComulativeVegasSet = isComulative;
        // Save Prefs
    }


    private void UpdateView()
    {
        SolitaireStageViewHelperClass view = FindObjectOfType<SolitaireStageViewHelperClass>();
        if (view != null)
        {
            SolitaireStageViewHelperClass.instance.UpdateViewSettings();
        }

        PlayerPrefAPI.Set();
    }

    private void ShowOrientationSet()
    {
        bool auto = false;
        bool portrait = false;
        bool landSpace = false;



        switch (GameSettings.Instance.orientationType)
        {
            case GameSettings.OrientationType.Auto:
                auto = true;
                break;
            case GameSettings.OrientationType.LandSpace:
                landSpace = true;
                break;
            case GameSettings.OrientationType.Portrait:
                portrait = true;
                break;
        }



        orientationObj[0].SetActive(portrait);
        orientationObj[1].SetActive(landSpace);
        orientationObj[2].SetActive(auto);
    }



    #endregion;
    #region Public
    public void OnSound()
    {
        GameSettings.Instance.isSoundSet = !GameSettings.Instance.isSoundSet;
        soundSwitch.SwitchTo(GameSettings.Instance.isSoundSet);
        if (GameSettings.Instance.isSoundSet)
            Sound.Instance.Up();
        //		else 
        //			Sound.Instance.Down ();
    }
    public void OnHand()
    {
        GameSettings.Instance.isHandSet = !GameSettings.Instance.isHandSet;
        handSwitch.SwitchTo(GameSettings.Instance.isHandSet);
        TryToPlaySound(GameSettings.Instance.isHandSet);
      StartCoroutine(  SolitaireStageViewHelperClass.instance.SetLayerTableu(GameSettings.Instance.isHandSet));
        UpdateView();
    }

    public void OnScoringReset()
    {
        PopUpScoringReset();
    }

    public void OnPlayAutoTapMove()
    {
        GameSettings.Instance.isAutoTapMoveSet = !GameSettings.Instance.isAutoTapMoveSet;
        playAutoTapMoveSwitch.SwitchTo(GameSettings.Instance.isAutoTapMoveSet);
        TryToPlaySound(GameSettings.Instance.isAutoTapMoveSet);
    }
    public void OnPlayAutoHint()
    {
        GameSettings.Instance.isAutoHintSet = !GameSettings.Instance.isAutoHintSet;
        playAutoHintSwitch.SwitchTo(GameSettings.Instance.isAutoHintSet);
        TryToPlaySound(GameSettings.Instance.isAutoHintSet);
    }
    public void OnHighLightFreeCard()
    {
        GameSettings.Instance.isHighLightMode = !GameSettings.Instance.isHighLightMode;
        highLightFreeCardsSwitch.SwitchTo(GameSettings.Instance.isHighLightMode);
        TryToPlaySound(GameSettings.Instance.isHighLightMode);
        SolitaireStageViewHelperClass.instance.HighLightFreeCards();
    }
    public void ShowStats()
    {
        PopUpManager.Instance.ShowStats();
    }




    public void OnOrientation(int value)
    {


        if (value == 0)
        {
            GameSettings.Instance.orientationType = GameSettings.OrientationType.Auto;
        }
        else if (value == 1)
        {
            GameSettings.Instance.orientationType = GameSettings.OrientationType.Portrait;
        }
        else
        {
            GameSettings.Instance.orientationType = GameSettings.OrientationType.LandSpace;
        }

        TryToPlaySound(true);
        ShowOrientationSet();

    }




    public void OnViewScore()
    {

        GameSettings.Instance.isMoveTimeSet = !GameSettings.Instance.isMoveTimeSet;
        viewScoreTimeSwitch.SwitchTo(GameSettings.Instance.isMoveTimeSet);
        TryToPlaySound(GameSettings.Instance.isMoveTimeSet);
        UpdateView();
    }

    public void OnEffectSet()
    {

        GameSettings.Instance.isEffectSet = !GameSettings.Instance.isEffectSet;
        effectsSwitch.SwitchTo(GameSettings.Instance.isEffectSet);
        TryToPlaySound(GameSettings.Instance.isEffectSet);

    }
    public void OnAnimationCongratsScreen()
    {
        GameSettings.Instance.isCongratsScreenSet = !GameSettings.Instance.isCongratsScreenSet;
        CongratsScreenSwitch.SwitchTo(GameSettings.Instance.isCongratsScreenSet);
        TryToPlaySound(GameSettings.Instance.isCongratsScreenSet);
    }





   


  

    public void OnNumberOfSuits(int cardPlayInGame)
    {
        if (!GameSettings.Instance.isCalendarGame)
        {
            GameSettings.Instance.numberOfSuit = cardPlayInGame;
          
         
                ShowDrawCardSet(GameSettings.Instance.numberOfSuit);
                TryToPlayUp();
            PopUpStageChangeSettings();


        }
        else
        {
            PopUpCalendarWarning();
        }
    }


    private void ShowDrawCardSet(int cardPlayInGame)
    {
        int suitCard = 0;
        switch (cardPlayInGame)
        {

            case 2:
                suitCard = 1;
                break;
            case 4:
                suitCard = 2;
                break;
        }


        for (int i = 0; i < suitCardObjs.Length; i++)
        {
            suitCardObjs[i].SetActive(true);
        }
        suitCardObjs[suitCard].SetActive(false);
    }



    public void OnHowToPlay()
    {
        TryToPlayUp();

        PopUpManager.Instance.ShowRule();
    }
    public void OnVisualPlayBackground()
    {
        TryToPlayUp();
        PopUpManager.Instance.ShowPlayBackground(() => InitVisualStyle());
    }
    public void OnVisualCardBacksSet()
    {
        TryToPlayUp();
        PopUpManager.Instance.ShowCardBack(() => InitVisualStyle());
    }
    public void OnVisualCardFacesSet()
    {
        TryToPlayUp();
        PopUpManager.Instance.ShowCardFace(() => InitVisualStyle());
    }
    public void OnBack()
    {

        if (GameSettings.Instance.isCalendarGame && !IsCalendarApproveGame())
            PopUpCalendarBlockChange();

        // Refresh view
        SolitaireStageViewHelperClass view = FindObjectOfType<SolitaireStageViewHelperClass>();
        if (view != null)
        {
            SolitaireStageViewHelperClass.instance.UpdateViewSettings();
        }

        PlayerPrefAPI.Set();
        PopUpManager.Instance.Close();

    }

    #endregion
    private void TryToPlaySound(bool value)
    {
        if (GameSettings.Instance.isSoundSet)
            if (value) Sound.Instance.Up();
            else Sound.Instance.Down();
    }
    private void TryToPlayUp()
    {
        if (GameSettings.Instance.isSoundSet) Sound.Instance.Up();
    }
    private bool IsCalendarApproveGame()
    {
        return (GameSettings.Instance.isOneCardSet.Equals(GameSettings.Instance.calendarIsOneCardSet));
    }

    #region PopUpWindow
    private void PopUpScoringReset()
    {
        string titleLineData = "RESET CUMULATIVE SCORE";
        string listLinesData = "Delete current cumulative scores." + "\n" + "Are you sure?";

        List<ResultButtonData> buttonData = new List<ResultButtonData>();
        buttonData.Add(new ResultButtonData("Ok", 1, OnOk));
        buttonData.Add(new ResultButtonData("Cancel", 2, OnCancel));

        PopUpManager.Instance.ShowDialog(titleLineData, listLinesData, buttonData);
    }
    private void OnOk()
    {
        GameSettings.Instance.scoreComulativeVegasOne = 0;
        GameSettings.Instance.scoreComulativeVegasThree = 0;
        PopUpManager.Instance.Close();
    }
    private void OnCancel()
    {
        PopUpManager.Instance.Close();
    }
    // ---
    private void PopUpCalendarBlockChange()
    {
        string titleLineData = "CAN NOT BE CHANGED!";
        string listLinesData = "You cannot change game mode of Daily Challenge.";

        List<ResultButtonData> buttonData = new List<ResultButtonData>();
        buttonData.Add(new ResultButtonData("Close", 2, OnClose));

        PopUpManager.Instance.ShowDialog(titleLineData, listLinesData, buttonData);
    }
    private void OnClose()
    {

        GameSettings.Instance.isOneCardSet = GameSettings.Instance.calendarIsOneCardSet;

        PopUpManager.Instance.Close();
    }
    // ---
    private void PopUpStageChangeSettings()
    {
        string titleLineData = "CHANGE SETTING";
        string listLinesData = "It will end your current game.\nAre you sure?";

        List<ResultButtonData> buttonData = new List<ResultButtonData>();
        buttonData.Add(new ResultButtonData("Ok", 1, OnOkChange));
        buttonData.Add(new ResultButtonData("Cancel", 2, OnCancelChange));

        PopUpManager.Instance.ShowDialog(titleLineData, listLinesData, buttonData);
    }

    private void OnOkChange()
    {
        PopUpManager.Instance.Close(); // close win
        PopUpManager.Instance.Close(); // close settings panel
        prevSuitCard =  GameSettings.Instance.numberOfSuit;
        
        GameSettings.Instance.isCriticalChanges = true; // newGame

        ContinueModeGame.instance.ClearAllDataCard();
        if (GameSettings.Instance.isCalendarGame)
        {
           StageManager.instance.GeneralRestartGame();
        }
        else
        {
         
            GameFlowDispatcher.Instance.FromSettingsToGame();
        }

        PlayerPrefAPI.Set();
         


    }
    private void OnCancelChange()
    {
        GameSettings.Instance.numberOfSuit = prevSuitCard;
        ShowDrawCardSet(prevSuitCard);
        PopUpManager.Instance.Close();
    }
    // ---
    private void PopUpCalendarWarning()
    {
        string titleLineData = "Can not be changed!";
        string listLinesData = "You cannot change the score\nor draw method\nin the meddle of Daily Challenge.";

        List<ResultButtonData> buttonData = new List<ResultButtonData>();
        buttonData.Add(new ResultButtonData("Close", 2, OnCloseCalendarWarning));

        PopUpManager.Instance.ShowDialog(titleLineData, listLinesData, buttonData);
    }
    private void OnCloseCalendarWarning()
    {
        PopUpManager.Instance.Close(); // close win
    }
    #endregion
}















