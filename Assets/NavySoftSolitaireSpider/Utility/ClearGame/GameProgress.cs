using UnityEngine;
using System;
using System.IO;
using Calendar;

public class GameProgress
{
	public void Clear()
	{
		ClearPlayerPrefs ();
		InitNamePref ();
		ClearPersistanceData ();
		InitPlayerPref ();
		ClearPlayerPref ();
	}
	private void ClearPlayerPrefs ()
	{
		PlayerPrefs.DeleteAll ();
	}
	private void ClearPersistanceData()
	{
		string path = Application.persistentDataPath;

		string[] fileList = Directory.GetFiles(path, "*.*");

		foreach (string name in fileList)
			File.Delete (name);
	}
	private void ClearPlayerPref() // reset game to default
	{
		GameSettings.Instance.isSoundSet = true;
		GameSettings.Instance.isStandardSet = true;
		GameSettings.Instance.isComulativeVegasSet = false;
		GameSettings.Instance.isOneCardSet = true;
		GameSettings.Instance.isAutoTapMoveSet = true;
		GameSettings.Instance.isAutoHintSet = true;
		GameSettings.Instance.isHandSet = true;
		GameSettings.Instance.isScoreSet = true;
		GameSettings.Instance.isMoveTimeSet = true;
		GameSettings.Instance.isEffectSet = true;
		GameSettings.Instance.isCongratsScreenSet = true;
		GameSettings.Instance.visualPlayBackgroundSet = 0;
		GameSettings.Instance.visualCardBacksSet = 0;
		GameSettings.Instance.scoreComulativeVegasOne = 0;
		GameSettings.Instance.scoreComulativeVegasThree = 0;
		GameSettings.Instance.danceCurrentCounter = 0;
//		GameSettings.Instance.playerName
		GameSettings.Instance.startDay = 1;
		GameSettings.Instance.startMonth = 1;
		GameSettings.Instance.startYear = 2017;
	}

	public void InitPlayerPref() // clear old seance data
	{
		PlayerPrefAPI.Get ();
		GameSettings.Instance.deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;

		GameSettings.Instance.isCalendarGame = false;
		GameSettings.Instance.isGameWon = false;
		//GameSettings.Instance.calendarData = new string[2];
		GameSettings.Instance.calendarGameDay = 0;
		GameSettings.Instance.calendarGameMonth = 0;
		GameSettings.Instance.calendarGameYear = 0;
		GameSettings.Instance.calendarTryEarnPoints = 0;
		GameSettings.Instance.isGameStarted = false;
		GameSettings.Instance.isMenu = false;
		GameSettings.Instance.isCriticalChanges = false;
	}
	#region Player Name
	public void InitNamePref ()
	{
		if (!PlayerPrefs.HasKey ("Name"))
			CreateName ();
		InitName ();
	}
	private void CreateName ()
	{
		string name = GameSettings.Instance.deviceUniqueIdentifier;
		if (name.Length > 10)
			name = name.Substring (0, 10);
		name = "ID" + name;
		PlayerPrefs.SetString ("Name", name);
		PlayerPrefs.Save ();
	}
	private void InitName ()
	{
		string name = PlayerPrefs.GetString ("Name");
		GameSettings.Instance.playerName = name;
	}
	#endregion
	#region Calendar
	public void InitCalendarPref()
	{
		if (!PlayerPrefs.HasKey ("StartCallendarDate"))
			CreateStartCallendarDate ();
		InitStartCallendarDate ();
	}
	private void CreateStartCallendarDate()
	{
		DateTime today = DateTime.Today;

		int day = today.Day;
		int month = today.Month;
		int year = today.Year;

		month--;
		if (month.Equals (0))
		{
			month = 12;
			year--;
		}

		string startingDate = year.ToString () + ":" + month.ToString () + ":" + day.ToString ();
		PlayerPrefs.SetString ("StartCallendarDate", startingDate);
		PlayerPrefs.Save ();
	}
	private void InitStartCallendarDate()
	{
		string startingDate = PlayerPrefs.GetString ("StartCallendarDate");
		string[] date = startingDate.Split (':');
		GameSettings.Instance.startDay = int.Parse (date [2]);
		GameSettings.Instance.startMonth = int.Parse (date [1]);
		GameSettings.Instance.startYear = int.Parse (date [0]);
	}
	#endregion
}