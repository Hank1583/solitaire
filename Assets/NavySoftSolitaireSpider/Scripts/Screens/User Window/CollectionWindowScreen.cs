namespace UserWindow
{
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections.Generic;
	public class CollectionWindowScreen : MonoBehaviour
	{
		[SerializeField]
		private Text headText;

		[SerializeField]
		private Image[] medalImage;
		[SerializeField]
		private Text[] monthText;

		[SerializeField]
		private GameObject buttonPref;
		[SerializeField]
		private RectTransform buttonsRT;

		private List<CollectionData> collectionData;
		private List<ResultButtonData> buttonData;
		public void Init (string head, List<CollectionData> collectionData, List<ResultButtonData> buttons)
		{
			this.headText.text = head;
			this.collectionData = collectionData;
			this.buttonData = buttons;
		}
		public void Build ()
		{
			BuildMedalInfo ();
			BuildButtons ();
		}
		private void BuildMedalInfo()
		{
			int medalCount = collectionData.Count;
			if (!medalCount.Equals (6)) throw new UnityException ("range error.");
			for (int index = 0; index < medalCount; index++)
			{
				medalImage [index].sprite = collectionData [index].MedalImage;
				monthText [index].text = collectionData [index].MonthText;
			}
		}
		private void BuildButtons()
		{
			foreach (ResultButtonData element in buttonData)
			{
				GameObject buttonObj = (GameObject)Instantiate (buttonPref);
				buttonObj.transform.SetParent (buttonsRT);
				ResultButtonIniter buttonIniter = buttonObj.GetComponent<ResultButtonIniter> ();
				buttonIniter.Init (element);
				buttonIniter.Show ();
			}
		}
	}
	public class CollectionData
	{
		private Sprite medalImage;
		private string monthText;
		 
		public Sprite MedalImage{get{ return medalImage;}}
		public string MonthText{get{ return monthText;}}

		public CollectionData(Sprite sprite, string text)
		{
			this.medalImage = sprite;
			this.monthText = text;
		}
	}
}