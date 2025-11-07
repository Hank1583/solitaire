namespace Calendar
{
	using System.Collections.Generic;
	using SolitaireEngine.Model;
	public class CalendarParser
	{
		public List<Card> DeckParse(string deck)
		{
			List<Card> convertedDeck = new List<Card> ();
			string[] cardStr = deck.Split ('*');
			foreach (string element in cardStr)
			{
				string[] cardOne = element.Split (':');
				int id = int.Parse (cardOne [0]);
				int rank = int.Parse (cardOne [1]);
				int suit = int.Parse (cardOne [2]);
				bool isOpen = (cardOne [3].Equals ("1"));
				convertedDeck.Add (new Card (id, rank, suit, isOpen));
			}
			return convertedDeck;
		}
		public List<ContractCommand> SolutionCommandsParse(string commands)
		{
			List<ContractCommand> convertedCommands = new List<ContractCommand> ();
			string[] commandStr = commands.Split ('*');
			foreach (string element in commandStr)
			{
				string[] commandOne = element.Split (':');
				int action = int.Parse (commandOne [0]);
				int idFrom = int.Parse (commandOne [1]);
				int idTo = int.Parse (commandOne [2]);
				convertedCommands.Add (new ContractCommand((ContractCommand.State)action,idFrom,idTo));
			}
			return convertedCommands;
		}
	}
}