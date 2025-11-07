using System.Collections.Generic;

public class CommandExecutor {

	List<ICommand> executedCommands = new List<ICommand>();


	public void Execute (ICommand command, bool canUndo = true) {
		command.execute ();
		if (canUndo) {
//            UnityEngine.Debug.Log("Add Command");
			executedCommands.Add (command);
		}
	}
	public void Unexecute () {
		if (!HasCommands ()) {
			throw new System.Exception ("no commands found");
		}

        if(executedCommands[executedCommands.Count - 1] is MoveBackRowCardCommands)
        {
            MoveBackRowCardCommands moveBackRowCardCommands = executedCommands[executedCommands.Count - 1] as MoveBackRowCardCommands;
            if (moveBackRowCardCommands.Complete)
            {
                SolitaireStageViewHelperClass.instance.prevRowComplete = true;
                executedCommands[executedCommands.Count - 1].unexecute();
              
                executedCommands.RemoveAt(executedCommands.Count - 1);
                ContinueModeGame.instance.RemoveDataCard();
                SolitaireSpiderCheck.instance.RemoveLastGroup();
            }
            else
            {
                ContinueModeGame.instance.RemoveDataCard();
                SolitaireSpiderCheck.instance.RemoveDealCardGroup();
            }
           
        }
      //  SolitaireStageViewHelperClass.instance.prevRowComplete = false;
        executedCommands [executedCommands.Count - 1].unexecute ();

		executedCommands.RemoveAt (executedCommands.Count - 1);
        ContinueModeGame.instance.RemoveDataCard();

    }


	public void Reset(){
		executedCommands.Clear ();
	}

 

	public bool HasCommands(){
		return executedCommands.Count > 0;
	}
 
}
