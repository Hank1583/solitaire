using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UserWindow;

public class RateUsController : MonoBehaviour {


	private string appleId = "123456789";
	private string androidAppId = "com.navysoft.spider";

	private static RateUsController _instance = null;
	public static RateUsController instance{
		get{ 
			if (_instance == null) {				
				_instance = FindObjectOfType<RateUsController>();
				if (_instance == null) {
					// create instance
					GameObject go = new GameObject("RateUsController");
					_instance = go.AddComponent<RateUsController>();
				}
			}
			return _instance;
		}
	}
	[System.Serializable]
	private class StoredData{
		string launchKey = "launchCount";
		string lastShowedKey = "lastShowedDay";
		string userAccessKey = "userAccess";

		public int gameLaunchTimes = 0;
		public int lastShowedDay = 0;
		public bool userAccessNextShow = true;

		public void Load(){
			if (PlayerPrefs.HasKey (launchKey)) {
				gameLaunchTimes = PlayerPrefs.GetInt (launchKey);
			}

			if (PlayerPrefs.HasKey (lastShowedKey)) {
				lastShowedDay = PlayerPrefs.GetInt (lastShowedKey);
			}		

			if (PlayerPrefs.HasKey (userAccessKey)) {
				userAccessNextShow = PlayerPrefs.GetInt (userAccessKey) == 1;
			}		
		
		}
		public void Save(){
			PlayerPrefs.SetInt (launchKey, gameLaunchTimes);
			PlayerPrefs.SetInt (lastShowedKey, lastShowedDay);
			PlayerPrefs.SetInt (userAccessKey, userAccessNextShow?1:0);
			
		}
	}
	[SerializeField]
	StoredData data;
	// Use this for initialization
	IEnumerator Start () {
		data = new StoredData();
		data.Load ();
		yield return 0;

		// increase game launched times count
		data.gameLaunchTimes++;
		data.Save();
		yield return 0;

		CheckRatePopUp();
	}

	void CheckRatePopUp(){

		// guard clause if "No, Thanks" was pressed
		if (!data.userAccessNextShow)
			return;

		// guard clause for appering after few times launched.
		if (data.gameLaunchTimes < 3) {
			return;
		}

		// guard clause to show only in each 2-nd day
		if (Mathf.Abs(getDayToday () - data.lastShowedDay) < 2)
			return;

//		print ("show review");
		// show pop up
		PopUpReviewApp (true);

		// save last appeating date
		data.lastShowedDay = getDayToday ();
		data.Save ();

	}

	int getDayToday(){
		System.DateTime today = System.DateTime.Today;
		return today.Day;
	}




	#region PopUpDialogWindow
	public void PopUpReviewApp(bool enableLaterButton = false)
	{
		string titleLineData = "REVIEW THIS GAME";
		string listLinesData = "Thanks for playing\n'Spider Solitaire'.\nIf you like this game, please write\nsome rewiev for us!";

		List<ResultButtonData> buttonData = new List<ResultButtonData> ();
		if (enableLaterButton) {
			buttonData.Add (new ResultButtonData ("Rate Us", 1, OnRate));
			buttonData.Add (new ResultButtonData ("Later", 0, OnLater));
			buttonData.Add (new ResultButtonData ("No, thanks", 2, OnNoThanks));
		} else {
			buttonData.Add (new ResultButtonData ("OK", 1, OnRate));
			buttonData.Add (new ResultButtonData ("Close", 2, OnLater));		
		}

		PopUpManager.Instance.ShowDialog (titleLineData, listLinesData, buttonData);
	}
	private void OnRate()
	{
		PopUpManager.Instance.Close ();
		#if UNITY_ANDROID
		Application.OpenURL ("market://details?id=" + androidAppId);
		#elif UNITY_IOS
		// NOTE: not tested
		Application.OpenURL("itms-apps://itunes.apple.com/app/id" + appleId);
		#endif
	}
	private void OnLater()
	{
		PopUpManager.Instance.Close ();
	}
	private void OnNoThanks()
	{
		// prevent appearing in the future
		data.userAccessNextShow = false;
		data.Save ();

		PopUpManager.Instance.Close ();
	}
	#endregion
}
