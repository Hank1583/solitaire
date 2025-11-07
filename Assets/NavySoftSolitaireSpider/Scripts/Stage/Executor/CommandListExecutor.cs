using System.Collections.Generic;

public class CommandListExecutor {


	int iterator = 0;

	List<ICommand> listCommands = new List<ICommand>();


	public void Add(ICommand command){
		listCommands.Add (command);
	}

	public void ExecuteNext () {
		if (!HasNext ()) {
			throw new System.Exception ("no next command");
		}

		listCommands[iterator].execute ();
		iterator++;
	}

//	public void UnexecutePrevious () {
//	}


	public void Reset(){
		listCommands.Clear ();
		iterator = 0;
	}

	public bool HasNext(){
		return listCommands.Count > iterator;
	}

	//	public void Execute (ICommand command, bool addToHistory) {
	//		command.execute ();
	//		if (addToHistory) {
	//			executedCommands.Add (command);
	//		}
	//	}

	//	public void ExecuteNext(){
	//		executedCommands [executedCommands.Count - 1].execute ();
	//		executedCommands.RemoveAt [executedCommands.Count - 1];
	//	}

//	public bool HasCommands(){
//		return listCommands.Count > 0;
//	}
}
