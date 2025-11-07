public class MoveCardSolitCommand : ICommand
{
	private bool executed = false;
	private IManagerBaseCommands manager;
	private int id;
	private int dest_id;
	private int parent_id;

	public MoveCardSolitCommand (IManagerBaseCommands managerContext, int id, int destination_id, int parent_id)
	{
		this.manager = managerContext;
		this.id = id;
		this.dest_id = destination_id;
		this.parent_id = parent_id;
	}

	#region ICommand implementation
	public void execute ()
	{
#if UNITY_EDITOR
        if (executed) throw new UnityEngine.UnityException ("Cant execute command already executed");

#endif
        //        UnityEngine.Debug.Log("dest_id " + dest_id);
        //   UnityEngine.Debug.Log("id " + id);
        manager.Move (id, dest_id);

		// Ref Move from MoveCardAndTryOpenParent solitaire Attach it sends beck log is open down card
		executed = true;
	}
	public void unexecute ()
    {
#if UNITY_EDITOR
        if (!executed) throw new UnityEngine.UnityException ("Cant undo command not executed yet");
#endif
        manager.Move (id, parent_id);
		executed = false;
	}
	#endregion
}