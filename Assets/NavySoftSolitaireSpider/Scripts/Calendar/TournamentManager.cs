namespace Calendar
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UserWindow;
 
	using Convert;

	public class TournamentManager : MonoBehaviour
	{

		[SerializeField]
		private Text data;
		[SerializeField]
		private Text draw;
		[SerializeField]
		private GameObject pointsInfoSingleObj;
		[SerializeField]
		private GameObject pointsInfoDoubleObj;


		[SerializeField]
		private GameObject linePref;
		[SerializeField]
		private RectTransform parentListLineRT;

		private Action OnBackCallback;
		private Action OnPlayCallback;

		[SerializeField]
		private ResultButtonIniter BackRBI;
		[SerializeField]
		private ResultButtonIniter PlayRBI;


//		private void OnEnable()
//		{
//			InitButtons ();
//		}
		private void InitButtons (bool hasMedal)
		{
			string playButtonText = hasMedal? "Play" : "Replay";

			ResultButtonData backData = new ResultButtonData ("Back", 2, OnBack);
			ResultButtonData playData = new ResultButtonData (playButtonText, 1, OnPlay);

			BackRBI.Init (backData);
			PlayRBI.Init (playData);
			BackRBI.Show ();
			PlayRBI.Show ();
		}
			
		private void OnClearText()
		{
			DestroyChildInObject (parentListLineRT.gameObject);
		}
		private void DestroyChildInObject(GameObject parent)
		{
			int totalChildren = parent.transform.childCount;
			GameObject[] delChildren = new GameObject[totalChildren];
			for (int i = 0; i < totalChildren; i++)
				delChildren[i] = parent.transform.GetChild (i).gameObject;
			for (int i = 0; i < totalChildren; i++)
				Destroy (delChildren[i]);
		}
	
		private void InitHeadLine()
		{
			CalendarIOManager ioManager = new CalendarIOManager ();
			int year = GameSettings.Instance.calendarGameYear;
			int month = GameSettings.Instance.calendarGameMonth;
			int day = GameSettings.Instance.calendarGameDay;

			string titleLineData = ioManager.monthName [month - 1].ToUpper () + ", " + day.ToString ();
			data.text = titleLineData;
			draw.text = (GameSettings.Instance.calendarIsOneCardSet) ? "Draw 1" : "Draw 3";
			bool isDouble = (ioManager.IsToday(day,month,year)) ? true : false;
			pointsInfoSingleObj.SetActive (!isDouble);
			pointsInfoDoubleObj.SetActive (isDouble);
		}



 
		private void PrintMessage (string text, Color color)
		{
			List<ResultTextLineData> listData = new List<ResultTextLineData> ();
			ResultTextLineData line;
			line = new ResultTextLineData ("", Color.white);
			listData.Add (line);
			line = new ResultTextLineData (text, color);
			listData.Add (line);
			ShowTextLineData (listData);
		}
		private void ShowTextLineData(List<ResultTextLineData> listData)
		{
			foreach (ResultTextLineData element in listData)
			{
				//	GameObject obj = (GameObject)Instantiate (linePref, parentListLineRT); // Save to recent Unity version
				GameObject lineObj = (GameObject)Instantiate (linePref);
				lineObj.transform.SetParent (parentListLineRT);
				lineObj.transform.localScale = Vector3.one; 
				ResultTextLineIniter lineIniter = lineObj.GetComponent<ResultTextLineIniter> ();
				lineIniter.Init (element);
				lineIniter.Show ();				
			}
		}
		public void InitRecord()
		{
		 
		}
		#region Public
		public void Init(Action onBack, Action onPlay, bool hasMedal)
		{
			OnBackCallback = onBack;
			OnPlayCallback = onPlay;
			InitButtons (hasMedal);
			InitHeadLine ();
			InitRecord ();
		}

		public void OnBack()
		{
			OnBackCallback ();
		}
		public void OnPlay()
		{
			OnPlayCallback ();
		}
		#endregion
	}
}