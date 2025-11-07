public class CommandsPair : ICommand
{
	private bool executed = false;
	ICommand command1; 
	ICommand command2;
	public CommandsPair(ICommand command1, ICommand command2)
	{
		this.command1 = command1;
		this.command2 = command2;
	}
    #region ICommand implementation
    public void execute()
    {
#if UNITY_EDITOR
        if (executed) throw new UnityEngine.UnityException("Cant execute command already executed");
     //   UnityEngine.Debug.Log("command1 " + command1);
      //  UnityEngine.Debug.Log("command2 " + command2);

#endif

        command1.execute();

        if (command2 != null)
            command2.execute();



        executed = true;
    }
    public void unexecute()
    {
#if UNITY_EDITOR
     
        if (!executed)
        {
            UnityEngine.Debug.Log("command1 " + command1);
            UnityEngine.Debug.Log("command2 " + command2);
            throw new UnityEngine.UnityException("Cant undo command not executed yet");
        }

#endif
        command2.unexecute();

        command1.unexecute();
        executed = false;
    }
	#endregion
}