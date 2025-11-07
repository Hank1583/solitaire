using UnityEngine;
using UserWindow;

public class CenterScreen : MonoBehaviour
{
	private void Start ()
	{
        if (GameSettings.Instance.isSoundSet)
        {
            
            Sound.Instance.Shift();
        }
	}

	public void OnBack ()
	{
		PopUpManager.Instance.Close ();
	}

	public void OnLeaderbord()
	{
		 
	}

	public void OnAchivement()
	{
		 
	}
 

}