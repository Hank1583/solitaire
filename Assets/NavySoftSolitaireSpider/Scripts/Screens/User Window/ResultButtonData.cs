namespace UserWindow
{
	using System;
	public class ResultButtonData
	{
		private string name;
		private int color;
		private Action callback;
		public string Name {get{ return name;}}
		public int Color {get{ return color;}}
		public Action Callback {get{ return callback;}}
		public ResultButtonData (string name, int color, Action callback)
		{
			this.name = name;
			this.color = color;
			this.callback = callback;
		}
	}
}