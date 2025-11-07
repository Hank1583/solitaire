using HUD;
using SmartOrientation;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class HUDController : MonoHandler
{


    IHUDActions managerHUD;

    ICardActions managerCardActions;


    [SerializeField]
    private GameObject topPanelPortrait;

    [SerializeField]
    private GameObject[] hintsShow;
    [SerializeField]
    private GameObject[] noHintsDetected;
    [SerializeField]
    private Text[] textsHint;
    [SerializeField]
    private List<Button> buttonsAction = new List<Button>();

    [SerializeField]
    private List<StatusBar> statusBars;
    [SerializeField]
    private PopUpWindow popUpWindow;
    [SerializeField]
    private HideBottomPanelAnim bottomPanel;
    [SerializeField]
    private HideBottomPanelAnim bottomPanelLandscape;

    public SmartOrientationHUD smartOrientationHUD;
    [SerializeField]
    private GameObject portraitLayout;
    [SerializeField]
    private GameObject landSpaceLayout;

    private bool playClicked = false;


    [SerializeField]
    private TextMeshProUGUI[] playModes;

    private int countAdsNewGame;

    public GameObject triggerLess;
    public GameObject triggerFull;

    private int countAdsHitHint = 0;
    private int countAdsHitUndo = 0;
    private int countAdsSolution = 0;
    public bool WinGame { get; private set; }
    private static HUDController _instance = null;
    public static HUDController instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = (GameObject)Resources.Load("HUD");
                GameObject go2 = Instantiate(go);
                _instance = go2.GetComponentInChildren<HUDController>();
            }
            return _instance;
        }
    }
    //	private void Awake() {instance = this;}

    // Use this for initialization
    void Start()
    {
        managerHUD = (IHUDActions)StageManager.instance;
        managerCardActions = (ICardActions)StageManager.instance;
        transform.SetAsFirstSibling();


    }


    public void UpdateGameMode()
    {
        if (GameSettings.Instance.isCalendarGame)
        {
            for (int i = 0; i < playModes.Length; i++)
            {

                playModes[i].text = "DAILY";
            }

        }
        else
        {

            for (int i = 0; i < playModes.Length; i++)
            {

                playModes[i].text = (GameSettings.Instance.randomDeal) ? "RANDOM" : "WINNING";


            }

        }
    }

    public void SetBarsVisibility(bool isMoveTime, bool isMoveTime2, bool isScore)
    {
        foreach (var b in statusBars)
        {
            b.SetVisibility(isMoveTime, isMoveTime2, isScore);
        }
    }


    public void VisibleButtonsAction(bool visible)
    {
        for (int i = 0; i < buttonsAction.Count; i++)
        {
            buttonsAction[i].interactable = visible;
        }
    }







    public void ResetStatusBars()
    {
        for (int i = 0; i < statusBars.Count; i++)
        {
            statusBars[i].reset();
        }
    }

    public void SetMove(int move)
    {
        // syncronize landscape status bar
        foreach (var b in statusBars)
        {
            b.move = move;
        }
    }

    public void SetTime(int time)
    {
        // syncronize landscape status bar
        foreach (var b in statusBars)
        {
            b.time = time;
        }
    }

    public void SetScore(int score)
    {

        foreach (var b in statusBars)
        {
            b.score = score;
        }
    }

    private UnityAction actionForTrigger;
    public void triggerClick()
    {
        if (actionForTrigger != null)
        {
            actionForTrigger();
        }
        //		triggerFull.SetActive (false);
        //		triggerLess.SetActive (false);
    }
    public void setTrigger(bool isFull, UnityAction action)
    {
        actionForTrigger = action;
        if (isFull)
        {
            triggerFull.SetActive(true);
        }
        else
        {
            triggerLess.SetActive(true);
        }
    }


    public bool isBottomPanelShown = true;
    public void ToggleBottomPanel()
    {

        popUpWindow.hide();
        playClicked = false;
        isBottomPanelShown = !isBottomPanelShown;
        bottomPanel.ShowPanel(isBottomPanelShown);
        bottomPanelLandscape.ShowPanel(isBottomPanelShown);
    }



    public void VisibleLayout(bool visible)
    {
        WinGame = !visible;
        if (DeviceOrientationHandler.instance.isVertical)
            portraitLayout.SetActive(visible);
        else
            landSpaceLayout.SetActive(visible);
    }


    #region HUD buttons pressed 

    /// <summary>
    /// Raises the back to menu button pressed event.
    /// </summary>
    public void OnBackToMenuPressed()
    {
        if (managerCardActions != null)
            managerCardActions.OnClickAnywhere();

        managerHUD.OnBackToMenuPressed();
    }

    /// <summary>
    /// Raises the shop click event.
    /// </summary>
    public void OnStorePressed()
    {
        if (managerCardActions != null)
            managerCardActions.OnClickAnywhere();

        managerHUD.OnStorePressed();
    }

    /// <summary>
    /// Show the manual(tutorial) screen.
    /// </summary>
    public void OnManualPressed()
    {
        if (managerCardActions != null)
            managerCardActions.OnClickAnywhere();



        managerHUD.OnShowManualPressed();
    }




    /// <summary>
    /// Shows the option menu.
    /// </summary>
    public void OnOptionsPressed()
    {
        popUpWindow.hide();
        playClicked = false;


        if (managerCardActions != null)
            managerCardActions.OnClickAnywhere();

        managerHUD.OnOptionsPressed();
    }

    public void OnCalendarPressed()
    {
        ShowAds();
        playClicked = false;
        managerHUD.OnDailyChallengePressed();
        popUpWindow.hide();

    }



    /// <summary>
    /// Shows the new menu.
    /// </summary>
    public void OnNewPressed()
    {

        if (playClicked)
        {
            popUpWindow.hide();
        }
        else
        {

            if (managerCardActions != null)
                managerCardActions.OnClickAnywhere();

            List<ButtonModel> buts = new List<ButtonModel>();



            buts.Add(button
                .setTitle("Stats")
                .setAction(() =>
                {

                    playClicked = false;
                    PopUpManager.Instance.ShowStats();
                    popUpWindow.hide();

                })
            );
            buts.Add(button
            .setTitle("Random Shuffle")
            .setAction(() =>
            {
                ShowAds();
                ContinueModeGame.instance.ClearAllDataCard();
                playClicked = false;
                managerHUD.OnNewGamePressed();
                GameSettings.Instance.randomDeal = true;
                GameSettings.Instance.isCalendarGame = false;
                popUpWindow.hide();

            })
        );
            buts.Add(button
            .setTitle("Winning Deal")
            .setAction(() =>
            {
                ShowAds();
                ContinueModeGame.instance.ClearAllDataCard();
                playClicked = false;
                GameSettings.Instance.randomDeal = false;
                GameSettings.Instance.isCalendarGame = false;
                managerHUD.OnNewGamePressed();
                popUpWindow.hide();

            })
        );
            buts.Add(button
                .setTitle("Restart This Game")
                .setAction(() =>
                {
                    ShowAds();
                    ContinueModeGame.instance.ClearAllDataCard(false);
                    playClicked = false;
                    managerHUD.OnRestartPressed();
                    popUpWindow.hide();

                })
            );




            popUpWindow.show(buts);
        }
        playClicked = !playClicked;
    }

    /// <summary>
    /// Shows the hint menu.
    /// </summary>
    public void OnHintsPressed()
    {
        popUpWindow.hide();
        playClicked = false;
        countAdsHitHint++;

        if (countAdsHitHint >= 6)
        {
            countAdsHitHint = 0;


            //AdvertisementsManager.Instance.ShowRewardVideo(null, null);


        }



        if (managerCardActions != null)
            managerCardActions.OnClickAnywhere();

        if (StageManager.instance.HasHint)
        {
            managerHUD.OnHintsPressed();
            return;
        }



        for (int i = 0; i < noHintsDetected.Length; i++)
        {
            noHintsDetected[i].SetActive(true);
        }
        StartCoroutine(HideNotification(noHintsDetected));
    }


    private IEnumerator HideNotification(GameObject[] notificationObjects)
    {
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < notificationObjects.Length; i++)
        {
            notificationObjects[i].SetActive(false);
        }
    }


    /// <summary>
    /// Shows the undo menu.
    /// </summary>
    public void OnUndoPressed()
    {
        popUpWindow.hide();
        playClicked = false;
        countAdsHitUndo++;
        if (countAdsHitUndo >= 6)
        {
            countAdsHitUndo = 0;


            //AdvertisementsManager.Instance.ShowRewardVideo(null, null);


        }

        if (managerCardActions != null)
            managerCardActions.OnClickAnywhere();
        managerHUD.OnUndoPressed();
    }
    #endregion




    // it makes life a bit easier :)
    ButtonModel button
    {
        get
        {
            return new ButtonModel();
        }
    }


    private void ShowAds()
    {
        countAdsNewGame++;
        if (countAdsNewGame > 3)
        {
            //AdvertisementsManager.Instance.ShowRewardVideo(null, null);
            countAdsNewGame = 0;
        }
        else
        {
            //AdvertisementsManager.Instance.ShowInterstitial();
        }
    }

}