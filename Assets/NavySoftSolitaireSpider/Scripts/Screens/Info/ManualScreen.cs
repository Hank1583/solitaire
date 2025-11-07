using UnityEngine;
using UserWindow;

public class ManualScreen : MonoBehaviour
{
	private void Start ()
	{
		//if (GameSettings.Instance.isSoundSet) Sound.Instance.Shift ();
	}
	public void OnRule ()
	{
		PopUpManager.Instance.ShowRule ();
	}
	public void OnControl ()
	{
		PopUpManager.Instance.ShowControl ();
	}
	public void OnScoring ()
	{
		PopUpManager.Instance.ShowScoring ();
	}
	public void OnDaily()
	{
		PopUpManager.Instance.ShowDaily ();
	}
	public void OnTip ()
	{
		PopUpManager.Instance.ShowTips ();
	}
	public void OnReviewApp ()
	{
		PopUpReviewApp ();
	}
	public void OnBack ()
	{
		PopUpManager.Instance.Close ();
	}
	#region PopUpDialogWindow
	private void PopUpReviewApp()
	{
		RateUsController.instance.PopUpReviewApp ();
	}
	#endregion
}