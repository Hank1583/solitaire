namespace UserWindow
{
	using System;
	using UnityEngine;
	public class ResultTextLineData
	{
		private string text1;
		private Color colorText1;
		private bool isImageMark;
		private string text2;
		private Color colorText2;

		public string Text1 {get{ return text1;}}
		public Color ColorText1{get{ return colorText1;}}
		public bool IsImageMark {get{ return isImageMark;}}
		public string Text2 {get{ return text2;}}
		public Color ColorText2{get{ return colorText2;}}

		private bool isOneString;
		public bool IsOneString {get{return isOneString;}}

		public ResultTextLineData (string text1, Color colorText1, bool isImageMark, string text2, Color colorText2)
		{
			this.text1 = text1;
			this.colorText1 = colorText1;
			this.isImageMark = isImageMark;
			this.text2 = text2;
			this.colorText2 = colorText2;
			this.isOneString = false;
		}
		public ResultTextLineData (string text1, Color colorText1)
		{
			this.text1 = text1;
			this.colorText1 = colorText1;
			this.isOneString = true;
		}
	}
}