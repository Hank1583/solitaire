namespace UserWindow
{
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections.Generic;
    using TMPro;
	public class DialogWindowScreen : MonoBehaviour
	{
		[SerializeField]
		private  TextMeshProUGUI headText;
		[SerializeField]
		private TextMeshProUGUI infoText;


		[SerializeField]
		private GameObject buttonPref;
		[SerializeField]
		private RectTransform buttonsRT;

		private string headData;
		private string infoData;
		private List<ResultButtonData> buttonData;
        private List<GameObject> buttons = new List<GameObject>();

		public void Init (string head,string info, List<ResultButtonData> buttons)
		{
			this.headData = head;
			this.infoData = info;
			this.buttonData = buttons;
            for (int i = 0; i < this.buttons.Count; i++)
            {
                Destroy(this.buttons[i]);
            }
            this.buttons.Clear();
		}
		public void Build ()
		{
			BuildTexts ();
			BuildButtons ();
		}
		private void BuildTexts()
		{
			headText.text = headData;
			infoText.text = infoData;
		}
		private void BuildButtons()
		{
			foreach (ResultButtonData element in buttonData)
			{
				GameObject buttonObj = (GameObject)Instantiate (buttonPref);
				buttonObj.transform.SetParent (buttonsRT);
                buttonObj.transform.localScale = Vector3.one;
                ResultButtonIniter buttonIniter = buttonObj.GetComponent<ResultButtonIniter> ();
				buttonIniter.Init (element);
				buttonIniter.Show ();
                buttons.Add(buttonObj);

            }
		}
	}
}