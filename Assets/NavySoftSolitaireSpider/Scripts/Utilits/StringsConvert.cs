namespace Convert
{
	using System;
	public static class StringsConvert
	{
		public static string ConvertToMinutesSeconds(int value)
		{
			const int secInMinutes = 60;
			const int secInHour = 3600;
			int totalSeconds = value;
			string hoursStr = "";
			string result = "";
			if (totalSeconds >= secInHour)
			{
				int hours = (int)((float)totalSeconds / secInHour);
				totalSeconds = totalSeconds - hours * secInHour;
				hoursStr = hours.ToString()+"h:";
			}
			if (totalSeconds > (secInMinutes-1))
			{
				int minutes = (int)((float)totalSeconds / secInMinutes);
				int seconds = totalSeconds - minutes * secInMinutes;
				result = DoubleDigit (minutes) + ":" + DoubleDigit (seconds);
			}
			else
			{
				result = "00:" + DoubleDigit (totalSeconds);
			}
			return hoursStr+result;
		}
		private static string DoubleDigit(int value)
		{
			string digit = value.ToString ();
			return (digit.Length.Equals (1)) ? "0" + digit : digit;
		}
		public static string ConvertToComaFormat(int value)
		{
			string digitalStr = value.ToString ();
			return ComaCuter (digitalStr);
		}
		private static string ComaCuter(string value)
		{
			string result = value;
			int length = value.Length;
			if ( length > 3)
			{
				string higher = value.Substring (0, (length - 3));
				string lower = value.Substring ((length - 3), 3);
				result = ComaCuter (higher) + "," + lower;
			}
			return result;
		}
		public static string ConcatPersent(int value)
		{
			return value.ToString()+"%";
		}
	}
}