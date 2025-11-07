using UnityEngine;
using UserWindow;
public class GameFlowDispatcher : MonoBehaviour
{
	#region Singletone
	private static GameFlowDispatcher instance = null;
	public static GameFlowDispatcher Instance {get{return instance;}}
	#endregion

	[SerializeField]
	private StageManager stageManager;

	private void Awake()
	{
		instance = this;
	}

	#region Stage Manager
	private void NewGame()
	{
		stageManager.OnNewGame ();
	}
	private void RestartGame()
	{
		stageManager.OnRestartGame ();
	}
	private void ContinueGame()
	{
		stageManager.OnContinueGame ();
	}
	private void PauseGame()
	{
		stageManager.OnPause ();
	}
	#endregion

	#region Public Flow Logic
	// from Manager
	public void FromManagerToNewGame()
	{
		NewGame ();
	}
	public void FromManagerToRestartGame()
	{
		RestartGame ();
	}
	public void FromManagerToCalendar()
	{
		PopUpManager.Instance.ShowCalendar ();
      
	 
	}
	public void FromManagerToMenu()
	{
		GameSettings.Instance.isMenu = true;
		PopUpManager.Instance.ShowMenu ();
      //  GoogleMobileAdsScript.instance.VisbileBanner(false);
      
	}
	public void FromManagerToSettings()
	{
	GameSettings.Instance.isMenu = false;
		PopUpManager.Instance.ShowSettings ();


     
    }

	// from Menu
	public void FromMenuToGame()
	{
		GameSettings.Instance.isMenu = false;
		PopUpManager.Instance.Close ();
       
		if (GameSettings.Instance.isGameStarted)
			 ContinueGame ();
		else
			 NewGame ();


   
    }
	public void FromMenuToCalendar()
	{
		PopUpManager.Instance.Close ();
		GameSettings.Instance.isMenu = false;
		PopUpManager.Instance.ShowCalendar ();
	}
	public void FromMenuToSettings()
	{
		GameSettings.Instance.isMenu = true;
		PopUpManager.Instance.ShowSettings ();

        //AdvertisementsManager.Instance.VisibleBanner(false);
    }
	// from Calendar
	public void FromCalendarToMenu()
	{
		PopUpManager.Instance.Close ();
		GameSettings.Instance.isMenu = true;
		GameSettings.Instance.isCalendarGame = false;
        SolitaireStageViewHelperClass.instance.SaveCandelar(0);
        PopUpManager.Instance.ShowMenu ();
	}
	public void FromCalendarToGame()
	{
		PopUpManager.Instance.Close ();
		GameSettings.Instance.isGameStarted = false;
		GameSettings.Instance.isCalendarGame = true;
        SolitaireStageViewHelperClass.instance.SaveCandelar(1);
        NewGame ();

    
    }
	// from Settings
	public void FromSettingsToMenu()
	{
		PopUpManager.Instance.ShowMenu ();
	}

	public void FromSettingsToGame()
	{
		if (GameSettings.Instance.isCriticalChanges)
		{
			GameSettings.Instance.isCriticalChanges = false;
			NewGame ();
		}
		else
			ContinueGame ();

      
    }
	#endregion
}