using UnityEngine;
using System;
using System.Collections.Generic;
using UserWindow;
using System.Collections;
public class PopUpManager : MonoBehaviour
{
    private static PopUpManager _instance = null;
    public static PopUpManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject PopUpPref = (GameObject)Resources.Load("PopUpWindow");
                GameObject obj = Instantiate(PopUpPref);
                obj.name = "PopUpWindow";
                _instance = obj.GetComponentInChildren<PopUpManager>();
            }
            return _instance;
        }
    }

    [SerializeField]
    private RectTransform holder;
    [SerializeField]
    private PopUpAnimator animator;
    #region Main
    //	public RectTransform Holder {get{ return holder;}}

    public void Show(GameObject origin, PopUpAnimator.Direction direction, Action callback)
    {
        animator.Show(origin, direction, callback);
    }
    public void Close()
    {
        animator.CloseLastWindow();
  
        BackScreen.instance.RemoveWindow();
    }
    private GameObject SpawnPref(GameObject pref, RectTransform holderRT)
    {
        GameObject obj = (GameObject)Instantiate(pref);
        obj.name = pref.name;
        obj.transform.SetParent(holderRT);
        return obj;
    }
    #endregion
    #region Panel
    public void ShowCenter()
    {
        BackScreen.instance.AddWindow();
        GameObject origin = WindowSettings.Instance.CenterPref;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.FromDown, null);
    }
    public void ShowStats()
    {
        BackScreen.instance.AddWindow();
        GameObject origin = WindowSettings.Instance.StatsPref;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.FromDown, null);
    }
    public void ShowSettings()
    {
        BackScreen.instance.AddWindow();
        GameObject origin = WindowSettings.Instance.SettingsPref;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.FromLeft, null);
    }
    public void ShowCardBack(Action func)
    {
        BackScreen.instance.AddWindow();
        GameObject origin = WindowSettings.Instance.CardBackPref;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.FromLeft, func);
    }

    public void ShowCardFace(Action func)
    {
        BackScreen.instance.AddWindow();
        GameObject origin = WindowSettings.Instance.CardFacePref;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.FromLeft, func);
    }
    public void ShowPlayBackground(Action func)
    {
        BackScreen.instance.AddWindow();
        Debug.Log("Show BackGround");
        GameObject origin = WindowSettings.Instance.PlayBackgroundPref;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.FromLeft, func);
    }
    public void ShowManual()
    {

        GameObject origin = WindowSettings.Instance.ManualPref;

        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.FromDown, null);
        origin.transform.Find("MenuScreen").gameObject.SetActive(false);
    }
    public void ShowRule()
    {
        BackScreen.instance.AddWindow();
        GameObject origin = WindowSettings.Instance.RulePref;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.FromRight, null);
    }
    public void ShowControl()
    {

        GameObject origin = WindowSettings.Instance.ControlPref;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.FromRight, null);
    }
    public void ShowScoring()
    {

        GameObject origin = WindowSettings.Instance.ScoringPref;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.FromRight, null);
    }
    public void ShowDaily()
    {

        GameObject origin = WindowSettings.Instance.DailyPref;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.FromRight, null);
    }
    public void ShowTips()
    {

        GameObject origin = WindowSettings.Instance.TipsPref;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.FromRight, null);
    }
    public void ShowMenu()
    {

        GameObject origin = WindowSettings.Instance.MenuPref;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.Central, null);

        //GameObject.FindObjectOfType<ServerAds>().ShowIcon();
    }
    public void ShowCalendar()
    {
        BackScreen.instance.AddWindow();
        GameObject origin = WindowSettings.Instance.CalendarPref;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.Central, null);
    }
    public void ShowWin(Action func)
    {
        GameObject origin = WindowSettings.Instance.WinPref;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.Central, func);
    }
    #endregion
    #region Dialog
    public void ShowResult(ResultTextLineData titleData, List<ResultTextLineData> listData, bool isAwarded, List<ResultButtonData> buttonData)
    {
        GameObject origin = WindowSettings.Instance.ResultPref;
        if (!animator.IsPresentWindowInPopUp(origin))
        {
            GameObject obj = SpawnPref(origin, holder);
            ResultWindowScreen manager = obj.GetComponentInChildren<ResultWindowScreen>();
            manager.Init(titleData, listData, isAwarded, buttonData);
            StartCoroutine(manager.Build());
            Show(obj, PopUpAnimator.Direction.FromLeft, null);
        }
    }
    public void ShowScroll(ResultTextLineData titleData, List<ResultTextLineData> listData, bool isAwarded, List<ResultButtonData> buttonData)
    {
        GameObject origin = WindowSettings.Instance.ScrollPref;
        if (!animator.IsPresentWindowInPopUp(origin))
        {
            GameObject obj = SpawnPref(origin, holder);
            ScrollWindowScreen manager = obj.GetComponentInChildren<ScrollWindowScreen>();
            manager.Init(titleData, listData, isAwarded, buttonData);
            Show(obj, PopUpAnimator.Direction.FromLeft, null);
            manager.Build();

        }
    }
    public void ShowDialog(string headData, string infoData, List<ResultButtonData> buttonData)
    {
        GameObject origin = WindowSettings.Instance.DialogPref;
        if (!animator.IsPresentWindowInPopUp(origin))
        {
            GameObject obj = SpawnPref(origin, holder);
            DialogWindowScreen manager = obj.GetComponentInChildren<DialogWindowScreen>();
            manager.Init(headData, infoData, buttonData);
            manager.Build();
            Show(obj, PopUpAnimator.Direction.FromRight, null);
        }
    }
    public void ShowInput(string head, string input, string hint, List<ResultButtonData> buttons, Action<string> inputCallback)
    {
        GameObject origin = WindowSettings.Instance.InputPref;
        if (!animator.IsPresentWindowInPopUp(origin))
        {
            GameObject obj = SpawnPref(origin, holder);
            InputWindowScreen manager = obj.GetComponentInChildren<InputWindowScreen>();
            manager.Init(head, input, hint, buttons, inputCallback);
            manager.Build();
            Show(obj, PopUpAnimator.Direction.FromRight, null);
        }
    }
    public void ShowCollection(string head, List<CollectionData> collectionData, List<ResultButtonData> buttons)
    {
        GameObject origin = WindowSettings.Instance.CollectionPref;
        if (!animator.IsPresentWindowInPopUp(origin))
        {
            GameObject obj = SpawnPref(origin, holder);
            CollectionWindowScreen manager = obj.GetComponentInChildren<CollectionWindowScreen>();
            manager.Init(head, collectionData, buttons);
            manager.Build();
            Show(obj, PopUpAnimator.Direction.FromUp, null);
        }
    }
    public void ShowEarnMedal(Sprite medalSprite, string infoText, List<ResultButtonData> buttons)
    {
        GameObject origin = WindowSettings.Instance.EarnMedalPref;
        if (!animator.IsPresentWindowInPopUp(origin))
        {
            GameObject obj = SpawnPref(origin, holder);
            EarnMedalWindowsScreen manager = obj.GetComponentInChildren<EarnMedalWindowsScreen>();
            manager.Init(medalSprite, infoText, buttons);
            manager.Build();
            Show(obj, PopUpAnimator.Direction.FromUp, null);
        }
    }
    #endregion
}