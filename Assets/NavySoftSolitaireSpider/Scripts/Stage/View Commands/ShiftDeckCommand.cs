public class ShiftDeckCommand : ICommand
{
	private bool executed = false;
	private IViewBaseCommands viewer;
	private bool forward;

	public ShiftDeckCommand (IViewBaseCommands viewContext, bool forward = true)
	{
		this.viewer = viewContext;
		this.forward = forward;
	}
	#region ICommand implementation
	public void execute ()
    {
#if UNITY_EDITOR
        if (executed) throw new UnityEngine.UnityException ("Cant execute command already executed");
#endif
        //		int id = 
        viewer.ShiftDeck (forward);
		executed = true;
	}
	public void unexecute ()
    {
#if UNITY_EDITOR
        if (!executed) throw new UnityEngine.UnityException ("Cant undo command not executed yet");
#endif
        viewer.ShiftDeck (!forward);
		executed = false;
	}
	#endregion
}