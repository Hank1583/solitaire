using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class CommandListUpdater : MonoBehaviour {

	private CommandListExecutor executor = new CommandListExecutor();

	private UnityAction onCommandExecuted = null;
	private UnityAction onCommandsListExecuted = null;

	public static CommandListUpdater instanse;
	void Awake(){
		instanse = this;
	}


	// Use this for initialization
	void Start () {
	
	}

	public void ExecuteList (List<ICommand> listToExecute, UnityAction onCommandExecuted = null, UnityAction onListExecuted = null)
	{
		this.onCommandExecuted = onCommandExecuted;
		this.onCommandsListExecuted = onListExecuted;

		//		SolitaireStageViewHelperClass.instance.loker.SetActive (true);
//		HUDController.instance.setTrigger(false, null);

		foreach (var c in listToExecute) {
			executor.Add (c);
		}
		pause = false;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateCommandExecute ();
	}

	bool pause = false;

	bool is_hints_anim = false;
	private void UpdateCommandExecute()
	{
        if (SolitaireStageViewHelperClass.instance == null)
            return;
        if (SolitaireStageViewHelperClass.instance.movementManager.isMoving)
			return;

		if(pause)
			return;

		if (executor.HasNext ()) {
			showNext ();
			is_hints_anim = true;
		}
		else {
			if (is_hints_anim) {
				stop ();
			}
		}
	}
	void showNext(){
		//		int last_command_index = 0;//hint_commands.Count - 1;
		executor.ExecuteNext();

		if (onCommandExecuted != null)
			onCommandExecuted ();
		//		ICommand command = hint_commands [last_command_index];
		//		command.execute ();
		//		hint_commands.Remove (command);

		// ***** Block touches whilst hints showing
		// TODO: remove it to better place or find another way to prevent bugs
//		SolitaireStageViewHelperClass.instance.loker.SetActive (true);

	}

	public void Pause ()
	{
		pause = true;
	}

	public void Continue ()
	{
		pause = false;
	}

	void stop(){
		is_hints_anim = false;

		if (onCommandsListExecuted != null)
			onCommandsListExecuted ();
		
		// restore game time
//		SolitaireStageViewHelperClass.instance.movementManager.Play ();

		// ***** Enable touches when hints showing finished
		// TODO: remove it to better place or find another way to prevent bugs

		//		SolitaireStageViewHelperClass.instance.loker.SetActive (false);	

//		HUDController.instance.triggerLess.SetActive(false);
	}





	public void Reset(){
		executor.Reset ();
		onCommandExecuted = null;
    	onCommandsListExecuted = null;

		// ??"??
	}
}
