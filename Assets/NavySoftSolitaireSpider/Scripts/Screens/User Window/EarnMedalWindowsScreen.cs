namespace UserWindow
{
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections.Generic;
	public class EarnMedalWindowsScreen : MonoBehaviour
	{
		[SerializeField]
		private Image medalImage;
		[SerializeField]
		private Text infoText;
		[SerializeField]
		private GameObject buttonPref;
		[SerializeField]
		private RectTransform buttonsRT;
		private List<ResultButtonData> buttonData;
		public void Init (Sprite medalSprite, string infoText, List<ResultButtonData> buttons)
		{
			this.infoText.text = infoText;
			this.medalImage.sprite = medalSprite;
			this.buttonData = buttons;
		}
		public void Build ()
		{
			BuildButtons ();
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
}