public class TurnCardSolitCommand : ICommand
{
	private bool executed = false;
	private IManagerBaseCommands manager;
	private int id;
   
	public TurnCardSolitCommand (IManagerBaseCommands managerContext, int id )
	{
		this.manager = managerContext;
		this.id = id;
 
	}


	#region ICommand implementation
	public void execute ()
    {
#if UNITY_EDITOR
        if (executed) throw new UnityEngine.UnityException ("Cant execute command already executed");
#endif
        manager.TurnCard (id);
		executed = true;
	}
	public void unexecute ()
    {
#if UNITY_EDITOR
        if (!executed) throw new UnityEngine.UnityException ("Cant undo command not executed yet");
#endif
        manager.TurnCard (id);
		executed = false;
	}
	#endregion
}