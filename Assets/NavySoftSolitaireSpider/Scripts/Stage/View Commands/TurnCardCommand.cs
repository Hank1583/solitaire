public class TurnCardCommand : ICommand
{
	private bool executed = false;
	private IViewBaseCommands viewer;
	private int id;
    private bool open;
    private bool lockOpen = false;
    public TurnCardCommand (IViewBaseCommands viewContext, int id ,bool open )
	{
		this.viewer = viewContext;
		this.id = id;
        this.open = open;
        this.lockOpen = false;
    }

    public void DisableOpen()
    {
        lockOpen = true;
    }
	#region ICommand implementation
	public void execute ()
    {
#if UNITY_EDITOR
        if (executed) throw new UnityEngine.UnityException ("Cant execute command already executed");
#endif
        //        UnityEngine.Debug.Log(string.Format("Turn Card executed  ID :{0}- Open: {1}", id, open));
        if (lockOpen)
        {
            viewer.TurnCard(id, false);
        }
        else
        {
            viewer.TurnCard(id, true);
        }
     
        executed = true;
	}
	public void unexecute ()
    {
#if UNITY_EDITOR
        if (!executed) throw new UnityEngine.UnityException ("Cant undo command not executed yet");

#endif
        CardItem card = SolitaireStageViewHelperClass.instance.FindCardItem(id);
        viewer.TurnCard (id, open);
		executed = false;
	}
	#endregion
}