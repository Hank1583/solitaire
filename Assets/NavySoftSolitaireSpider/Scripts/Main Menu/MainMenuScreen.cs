using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UserWindow;
using Calendar;
public class MainMenuScreen : MonoBehaviour
{
	[SerializeField]
	GameObject calebdarDoublerObj;

	private void Start ()
	{
		PlayerPrefAPI.Set ();
 


        GameSettings.Instance.isCalendarGame = false;
        GameSettings.Instance.randomDeal = false;
        PlayGame();


    }
	#region Public
	public void PlayGame ()
	{
        
		GameFlowDispatcher.Instance.FromMenuToGame ();
       
       
	}
	public void ShowSettings ()
	{
		GameFlowDispatcher.Instance.FromMenuToSettings ();
	}
	public void ShowCallendar ()
	{
       


        if (IsCalendarApproveGame ())
			GameFlowDispatcher.Instance.FromMenuToCalendar ();
		else
			PopUpChangeScoring ();
	}
	public void ShowStats ()
	{
		PopUpManager.Instance.ShowStats ();
	}

	public void ShowCenter ()
	{
		//if (GameSettings.Instance.isSocial)
			PopUpManager.Instance.ShowCenter ();
	}
	#endregion

	private bool IsCalendarApproveGame()
	{
		return (GameSettings.Instance.isStandardSet);
	}
	#region PopUpWindow
	private void PopUpChangeScoring()
	{
		string titleLineData = "CHANGE SCORING";
		string listLinesData = "The score setting will be changed\nto standard type for Daily Challenge.\nWould you like to continue?";

		List<ResultButtonData> buttonData = new List<ResultButtonData> ();
		buttonData.Add (new ResultButtonData ("Ok", 1, OnOk));
		buttonData.Add (new ResultButtonData ("Cancel", 2, OnCancel));

		PopUpManager.Instance.ShowDialog (titleLineData, listLinesData, buttonData);
	}
	private void OnOk()
	{
		GameSettings.Instance.isStandardSet = true;
		GameSettings.Instance.isComulativeVegasSet = false;
		GameSettings.Instance.isOneCardSet = true;
		PopUpManager.Instance.Close ();
		GameFlowDispatcher.Instance.FromMenuToCalendar ();
	}
	private void OnCancel()
	{
		PopUpManager.Instance.Close ();
	}
	#endregion   
	#region Debug // Production Delete it
	public void OnDebug()
	{
		GameSettings.Instance.startDay = 1;
		GameSettings.Instance.startMonth = 11;
		GameSettings.Instance.startYear = 2016;
	}
	public void OnDebugSettingsClear()
	{
		GameProgress gp = new GameProgress ();
		gp.Clear ();
	}
	#endregion
	private bool IsShow2x()
	{

        return false;
	}
	private void Show2x(bool isShow)
	{
		calebdarDoublerObj.SetActive (isShow);
	}
}