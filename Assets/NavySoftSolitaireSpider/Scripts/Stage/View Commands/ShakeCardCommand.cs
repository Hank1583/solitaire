public class ShakeCardCommand : ICommand
{
	private IViewBaseCommands viewer;
	private int id;

	public ShakeCardCommand (IViewBaseCommands viewContext, int id)
	{
		this.viewer = viewContext;
		this.id = id;
	}
	#region ICommand implementation
	public void execute ()
	{
		viewer.ShakeCard (id);
	}
	public void unexecute ()
	{
		throw new UnityEngine.UnityException ("Shake can't be unexecuted!");
	}
	#endregion
}