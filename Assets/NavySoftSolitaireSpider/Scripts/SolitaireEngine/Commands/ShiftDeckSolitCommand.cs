public class ShiftDeckSolitCommand : ICommand
{
	private bool executed = false;
	private IManagerBaseCommands manager;

	public ShiftDeckSolitCommand (IManagerBaseCommands managerContext)
	{
		this.manager = managerContext;
	}
	#region ICommand implementation
	public void execute ()
    {
#if UNITY_EDITOR
        if (executed) throw new UnityEngine.UnityException ("Cant execute command already executed");
#endif
        manager.ShiftDeck ();
		executed = true;
	}
	public void unexecute ()
    {
#if UNITY_EDITOR
        if (!executed) throw new UnityEngine.UnityException ("Cant undo command not executed yet");
#endif
        manager.UndoShiftDeck ();
		executed = false;
	}
	#endregion
}