namespace UserWindow
{
	using UnityEngine;
	using UnityEngine.UI;
	public class ResultTextLineIniter : MonoBehaviour
	{
		[SerializeField]
		private Text text0;
		[SerializeField]
		private Text text1;
		[SerializeField]
		private Text text2;
		[SerializeField]
		private GameObject cupObj;

		private ResultTextLineData data;
		public void Init (ResultTextLineData data)
		{
			this.data = data;
		}
		public void Show ()
		{
			if (data.IsOneString)
			{
				text0.text = data.Text1;
				text0.color = data.ColorText1;
			}
			else
			{
				text1.text = data.Text1;
				text1.color = data.ColorText1;
				cupObj.SetActive (data.IsImageMark);
				text2.text = data.Text2;
				text2.color = data.ColorText2;
			}
		}
	}
}