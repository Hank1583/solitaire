using UnityEngine;
public class WindowSettings : ScriptableObject
{
	private static WindowSettings _instance = null;
	public static WindowSettings Instance
	{
		get 
		{
			if (_instance == null)
			{
				_instance = (WindowSettings)Resources.Load ("WindowsSettings");
				if (_instance == null)
				{
					throw new UnityException ("Asset can't found");
				}
			}
			return _instance;
		}
	}
	public GameObject CenterPref;
	public GameObject StatsPref;
	public GameObject SettingsPref;
	public GameObject CardBackPref;
    public GameObject CardFacePref;
    public GameObject PlayBackgroundPref;
	public GameObject ManualPref;
	public GameObject RulePref;
	public GameObject ControlPref;
	public GameObject ScoringPref;
	public GameObject DailyPref;
	public GameObject TipsPref;
	public GameObject ResultPref;
	public GameObject ScrollPref;
	public GameObject DialogPref;
	public GameObject InputPref;
	public GameObject CollectionPref;
	public GameObject EarnMedalPref;
	public GameObject MenuPref;
	public GameObject CalendarPref;
	public GameObject WinPref;
}