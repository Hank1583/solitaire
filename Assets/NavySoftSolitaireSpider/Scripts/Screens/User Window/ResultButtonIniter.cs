namespace UserWindow
{
	using UnityEngine;
	using UnityEngine.UI;
	using System;
    using TMPro;
	public class ResultButtonIniter : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI text;
		[SerializeField]
		private Image image;
		[SerializeField]
		private Sprite[] skin; 
		private ResultButtonData data;
		public void Init (ResultButtonData data)
		{
			this.data = data;
		}
		public void Show ()
		{
			text.text = data.Name;
			image.sprite = skin [data.Color];
			image.color = Color.white;
		}
		public void OnPointerDown()
		{
			Color dark = new Color(0.75f,0.75f,0.75f,1f);
			image.color = dark;
		}
		public void OnPointerUp()
		{
			image.color = Color.white;
		}
		public void OnClick(bool showAds)
		{


         
			image.color = Color.white;
			if(data.Callback != null) data.Callback ();
           
		}

	}
}