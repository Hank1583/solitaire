using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToHelpScreen : MonoBehaviour
{
	private void Start ()
	{
        if (GameSettings.Instance.isSoundSet)
        {
          
            Sound.Instance.Shift(); // Change Sound
        }
	}
	public void OnBack ()
	{
		PopUpManager.Instance.Close ();
	}
}