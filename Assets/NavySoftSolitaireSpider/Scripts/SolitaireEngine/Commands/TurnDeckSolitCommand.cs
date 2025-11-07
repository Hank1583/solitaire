public class TurnDeckSolitCommand : ICommand
{
	private bool executed = false;
	private IManagerBaseCommands manager;

	public TurnDeckSolitCommand(IManagerBaseCommands managerContext)
	{
		this.manager = managerContext;
	}
	#region ICommand implementation
	public void execute ()
    {
#if UNITY_EDITOR
        if (executed) throw new UnityEngine.UnityException ("Cant execute command already executed");
#endif
        manager.ReverseDeck (false);
		executed = true;
	}
	public void unexecute ()
    {
#if UNITY_EDITOR
        if (!executed) throw new UnityEngine.UnityException ("Cant undo command not executed yet");
#endif
        manager.ReverseDeck (true);
		executed = false;
	}
	#endregion
}