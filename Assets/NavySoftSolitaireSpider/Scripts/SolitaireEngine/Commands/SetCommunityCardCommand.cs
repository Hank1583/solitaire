public class SetCommunityCardCommand : ICommand
{
	private bool executed = false;
	private IManagerBaseCommands manager;
	private int id;
	public SetCommunityCardCommand (IManagerBaseCommands managerContext, int id)
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
        manager.SetCommunityCardMove (id, true);
		executed = true;
	}
	public void unexecute ()
    {
#if UNITY_EDITOR
        if (!executed) throw new UnityEngine.UnityException ("Cant undo command not executed yet");
#endif
        manager.SetCommunityCardMove (id, false);
		executed = false;
	}
	#endregion
}