    namespace UserWindow
{
	using UnityEngine;
	using UnityEngine.UI;
	using System;
	using System.Collections.Generic;
	public class InputWindowScreen : MonoBehaviour
	{
		[SerializeField]
		private Text headText;
		[SerializeField]
		private InputField inputField;
		[SerializeField]
		private Text hintText;

		[SerializeField]
		private GameObject buttonPref;
		[SerializeField]
		private RectTransform buttonsRT;

		private List<ResultButtonData> buttonData;
		private Action<string> inputCallback;

//		private string input;

		public void Init (string head, string input, string hint, List<ResultButtonData> buttons, Action<string> inputCallback)
		{
			this.headText.text = head;
//			this.input = input;
			this.inputField.text = input;
			this.hintText.text = hint;
			this.buttonData = buttons;
			this.inputCallback = inputCallback;
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
		public void OnEndEdit()
		{
			inputCallback (inputField.text);
		}
		public void OnDelete()
		{
			inputField.text = "";//input;
		}
	}	
}