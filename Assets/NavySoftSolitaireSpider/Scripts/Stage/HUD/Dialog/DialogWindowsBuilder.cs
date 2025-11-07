using System.Collections.Generic;

using HUD;

using UnityEngine;
using UnityEngine.Events;

public class DialogWindowsBuilder : MonoBehaviour {

	[SerializeField]
	private DialogWindowsBase dialogBase;



	private static DialogWindowsBuilder _instance = null;
	public static DialogWindowsBuilder instance{
		get{ 
			if (_instance == null) {
				CreateInstance ();
			}
			return _instance;
		}
	}

	static void CreateInstance(){
		Object dialogPrefab = Resources.Load ("DialogWindow");
		_instance = ((GameObject)Instantiate (dialogPrefab)).GetComponent<DialogWindowsBuilder>();
	}


	/* -------------
	 * "massage" 
	 * ------------- 
	 */
	public void ShowSimpleMessage(string message){
		List<ButtonModel> emplyList = new List<ButtonModel>();
		dialogBase.show (message, emplyList);
	}

	/* -------------
	 * "massage" 
	 *   [OK]
	 * ------------- 
	 */
	public void ShowSimpleOkMessage(string message){
		List<ButtonModel> buttons = new List<ButtonModel> ();
		// create ok dummy button
		ButtonModel btn = new ButtonModel ();
		buttons.Add (
			btn.setTitle("OK").setAction(() => {dialogBase.hide();})
		);
		dialogBase.show (message, buttons);
	}

	/* -------------
	 *    		"massage" 
	 * [My button 1] [My button 2]
	 * ------------- 
	 */
	public void ShowTwoButtonsWindow(string message, string btn1_name, UnityAction btn1_action, string btn2_name, UnityAction btn2_action){
		List<ButtonModel> buttons = new List<ButtonModel> ();
		// create ok dummy button
		ButtonModel btn1 = new ButtonModel ();
		buttons.Add (
			btn1.setTitle(btn1_name).setAction(btn1_action)
		);
		ButtonModel btn2 = new ButtonModel ();
		buttons.Add (
			btn2.setTitle(btn2_name).setAction(btn2_action)
		);
		dialogBase.show (message, buttons);
	}

	/* -------------
	 *   "massage" 
	 * [OK] [Cancel]
	 * ------------- 
	 */
	public void ShowOkCancelMessage(string message, UnityAction on_ok_action, UnityAction on_cancel_action){
		List<ButtonModel> buttons = new List<ButtonModel> ();
		// create ok button
		ButtonModel btn = new ButtonModel ();
		buttons.Add (
			btn.setTitle("OK").setAction(on_ok_action)
		);
		// create cancel button
		ButtonModel btn_cancel = new ButtonModel ();
		buttons.Add (
			btn_cancel.setTitle("Cancel").setAction(on_cancel_action)
		);
		dialogBase.show (message, buttons);
	}

	/* -------------
	 *   "massage" 
	 * [OK] [Cancel] later
	 * ------------- 
	 */
	public void ShowLaterMessage(string message, UnityAction on_ok_action, UnityAction on_cancel_action, UnityAction on_later_action){
		List<ButtonModel> buttons = new List<ButtonModel> ();
		// create ok button
		ButtonModel btn = new ButtonModel ();
		buttons.Add (
			btn.setTitle("OK").setAction(on_ok_action)
		);
		// create cancel button
		ButtonModel btn_cancel = new ButtonModel ();
		buttons.Add (
			btn_cancel.setTitle("Cancel").setAction(on_cancel_action)
		);
		// create cancel button
		ButtonModel btn_later = new ButtonModel ();
		buttons.Add (
			btn_later.setTitle("Later").setAction(on_later_action)
		);
		dialogBase.show (message, buttons);
	}

	// NOTE: You could add your own builder here
}
