public class ScoringCommand : ICommand
{
	private bool executed = false;
	private IManagerBaseCommands manager;
	private int score;
	public ScoringCommand(IManagerBaseCommands managerContext, int score)
	{
		this.manager = managerContext;
		this.score = score;
	}
	#region ICommand implementation
	public void execute ()
    {
#if UNITY_EDITOR
        if (executed) throw new UnityEngine.UnityException ("Cant execute command already executed");
#endif
        manager.Score (SolitaireStageViewHelperClass.ScoreBeginGame);
		executed = true;
	}
	public void unexecute ()
    {
#if UNITY_EDITOR
        if (!executed) throw new UnityEngine.UnityException ("Cant undo command not executed yet");
#endif
        manager.Score (SolitaireStageViewHelperClass.ScoreBeginGame);
		executed = false;
	}
	#endregion
}