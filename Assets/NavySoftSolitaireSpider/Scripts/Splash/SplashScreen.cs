using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    private void Start()
    {
        GameProgress gp = new GameProgress();
        gp.InitPlayerPref();
        gp.InitNamePref();
        gp.InitCalendarPref();



        StartCoroutine(LoadStage());

    }


	private IEnumerator LoadStage ()
	{

		yield return new WaitForSeconds (.1f);
 
		SceneManager.LoadScene("Stage");
 
	}
}