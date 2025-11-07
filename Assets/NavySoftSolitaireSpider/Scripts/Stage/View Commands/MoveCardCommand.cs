public class MoveCardCommand : ICommand
{
	private bool executed = false;
	private IViewBaseCommands viewer;
	private int id;
	private int dest_id;
	private int parent_id;
	private bool animation;
    private bool moveCurve;
    public MoveCardCommand (IViewBaseCommands viewContext, int id, int destination_id, int parent_id,bool moveCurve =false, bool animation = false)
	{
		this.viewer = viewContext;
		this.id = id;
		this.dest_id = destination_id;
		this.parent_id = parent_id;
		this.animation = animation;
        this.moveCurve = moveCurve;
    }

	#region ICommand implementation
	public void execute ()
    {
#if UNITY_EDITOR
        if (executed) throw new UnityEngine.UnityException ("Cant execute command already executed");

#endif
        if(ContinueModeGame.instance.LoadSuccess  && !SolitaireStageViewHelperClass.instance.prevMoveCardInDeck)
        viewer.MoveCard (id, dest_id, animation, false,moveCurve);

        if (SolitaireStageViewHelperClass.instance.AddScoreMove)
        {
            StageManager.instance.AddScore(SolitaireStageViewHelperClass.ScoreBeginGame);
        }

        executed = true;
	}
	public void unexecute ()
    {
#if UNITY_EDITOR
        if (!executed) throw new UnityEngine.UnityException ("Cant undo command not executed yet");
#endif
        SolitaireStageViewHelperClass.instance.FindCardItem(id).MoveComplete = false;

        viewer.MoveCard (id, parent_id,true, false, false);
     
        StageManager.instance.AddScore(-SolitaireStageViewHelperClass.ScoreBeginGame);
        executed = false;
	}
	#endregion
}