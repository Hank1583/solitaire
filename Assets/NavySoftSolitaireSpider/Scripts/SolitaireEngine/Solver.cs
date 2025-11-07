namespace SolitaireEngine
{
	using System.Collections.Generic;
	using SolitaireEngine.Model;
	using SolitaireEngine.Utility;
	public class Solver
	{
		private Data data;
		private bool isOneCardSet;
		private bool isAutocomplete;
		private Data dataOriginal;
		private Logic logic;
		private Utils utils;
		private List<ContractCommand> commandStream;
		private Solver() {}
		public Solver (Data _data, bool _isOneCardSet, bool _isAutocomplete)
		{
			data = _data.Copy ();;
			isOneCardSet = _isOneCardSet;
			dataOriginal = data.Copy ();
			isAutocomplete = _isAutocomplete;
			logic = new Logic (data);
			utils = new Utils ();
			commandStream = new List<ContractCommand> ();
		}
		#region Public
		public List<ContractCommand> GetSolution()
		{
			return commandStream;
		}
		public Data GetData()
		{
			return dataOriginal.Copy();
		}
		private const int VEGAS_MODE_TURN_DECK_LIMIT = 3;
		private const int CARDS_TRICK_ATTEMPS = 2;
		public bool SolveAndBuild ()
		{
			int splitCount = 0;
			do
			{ // split
				bool isAttachDeckCard = false;
				do
				{ // attachDeckCard
					while (DeckAndCommunityMove ()){splitCount = 0;}
					if (IsAllGameOkSolved ()) return true;
					
					int deckLength = data.GetCountElementsInHolder (data.DeckIndex);
					int turnDeckCounter = 0;
					isAttachDeckCard = false;
					if (isOneCardSet && deckLength.Equals (0)) return false;
					if (!isOneCardSet && deckLength <= VEGAS_MODE_TURN_DECK_LIMIT) return false;
					do 
					{
						if (DeckMove ())
						{
							splitCount = 0;
							isAttachDeckCard = true;
							break;
						}
						turnDeckCounter++;
					}
					while (turnDeckCounter < deckLength);
				}
				while (isAttachDeckCard);				
				if (!SplitTrick ()) return false;
				splitCount++;
			}
			while (splitCount < CARDS_TRICK_ATTEMPS);
			return false;
		}
		#endregion
		#region Utility
		private bool SplitTrick()
		{
			if (IsDeckOver ()) return false;
			int totalCardInDeck = data.GetCountElementsInHolder (data.DeckIndex);
			List<int> cardCloseInCommunity = FindAllCloseCardsInCommunity ();
			int totalCardCloseInCommunity = cardCloseInCommunity.Count;
			int posibleChange = (totalCardInDeck < totalCardCloseInCommunity) ? totalCardInDeck : totalCardCloseInCommunity;

			for (int index = 0; index < posibleChange; index++)
			{
				int idDeckCard = data.Holder [data.DeckIndex].Element [index].Id;
				int randomCloseId = utils.RandomElement (cardCloseInCommunity);
				cardCloseInCommunity.Remove (randomCloseId);
				data.ChangeTwoCards (idDeckCard, randomCloseId);
				dataOriginal.ChangeTwoCards (idDeckCard, randomCloseId);
			}
			return true;		
		}
		private List<int> FindAllCloseCardsInCommunity()
		{
			List<int> cardCloseInCommunity = new List<int> ();
			for (int communityIndex = data.CommunityMin; communityIndex <= data.CommunityMax; communityIndex++)
			{
				int length = data.GetCountElementsInHolder (communityIndex);
				if (length > 0)
				{
					for (int index = 0; index < length; index++)
					{
						Card workCard = data.Holder [communityIndex].Element [index];
						if (!workCard.IsOpened) cardCloseInCommunity.Add (workCard.Id);
					}
				}
			}
			return cardCloseInCommunity;
		}
			
		private bool DeckAndCommunityMove()
		{
			List<ContractCommand> hints = logic.GetHints ();

			if (isAutocomplete) // Delete deck to community move
				hints = AutocompleteFilter (hints);

			if (hints.Count > 0)
			{
				ContractCommand randomMove = utils.RandomElement (hints);
		
				int shouldOpenDownCardId = logic.ShouldOpenDownCard (randomMove.IdFrom);
				if (!shouldOpenDownCardId.Equals (-1))
				{
					logic.TurnCard (shouldOpenDownCardId);
					commandStream.Add (new ContractCommand(ContractCommand.State.TurnCard,shouldOpenDownCardId));
				}
				logic.Move (randomMove.IdFrom, randomMove.IdTo);
				commandStream.Add (randomMove);

				return true;
			}
			return false;
		}

		private List<ContractCommand> AutocompleteFilter(List<ContractCommand> hints)
		{
			bool isDone = false;
			while (!isDone)
			{
				isDone = true;
				for (int index = 0; index < hints.Count; index++)
				{
//					IdResult resultFrom = data.IdAnalizator (hints [index].IdFrom);
					IdResult resultTo = data.IdAnalizator (hints [index].IdTo);

					if (!resultTo.isInThron) // chech turn deck command
					{
						hints.RemoveAt (index);
						isDone = false;
						break;
					}
				}
			}
			return hints;
		}
			
		private const int THREE_CARD_DEAL = 3;
		private bool DeckMove ()
		{
			if (isOneCardSet)
			{
				if (data.IsDeckOneCardFinished())
				{
					logic.ReverseDeck (false);
					commandStream.Add (new ContractCommand (ContractCommand.State.ReverseDeck));
				}
				logic.GetNextDeckCard ();
				commandStream.Add (new ContractCommand (ContractCommand.State.ShiftDeckOnece));
			}
			else
			{
				if (data.IsDeckThreeCardsFinished())
				{
					logic.ReverseDeck (false);
					commandStream.Add (new ContractCommand (ContractCommand.State.ReverseDeck));
				}

				int countCardsToTurn = logic.DeckRestCardsCount ();
				countCardsToTurn = (countCardsToTurn > THREE_CARD_DEAL) ? THREE_CARD_DEAL : countCardsToTurn;
				for (int turnCount = 0; turnCount < countCardsToTurn; turnCount++)
					logic.GetNextDeckCard ();
				switch (countCardsToTurn)
				{
				case 1:
					commandStream.Add (new ContractCommand (ContractCommand.State.ShiftDeckOnece));
					break;
				case 2:
					commandStream.Add (new ContractCommand (ContractCommand.State.ShiftDeckTwice));
					break;
				case 3:
					commandStream.Add (new ContractCommand (ContractCommand.State.ShiftDeckThird));
					break;
				default:
					break;
				}
			}
			return DeckAndCommunityMove ();
		}
		private bool IsAllGameOkSolved()
		{
			if (IsDeckOver () && IsCommunityOver ()) return true;
			return false;
		}
		private bool IsDeckOver()
		{
			return (data.GetCountElementsInHolder (data.DeckIndex).Equals (0));
		}
		private bool IsCommunityOver()
		{
			for (int communityIndex = data.CommunityMin; communityIndex <= data.CommunityMax; communityIndex++)
				if (!data.GetCountElementsInHolder (communityIndex).Equals (0)) return false;
			return true;
		}
		#endregion
	}	
}