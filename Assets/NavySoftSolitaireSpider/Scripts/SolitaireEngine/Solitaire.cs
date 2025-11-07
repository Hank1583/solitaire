namespace SolitaireEngine
{
	using System.Collections.Generic;
	using SolitaireEngine.Model;
	public class Solitaire : IDebugGame
	{
		private Data data;
		private Logic logic;
		private Solver solver;
		private List<ContractCommand> solutionCommands;
		private Solitaire () {}

		public Solitaire(List<Card> deck, List<ContractCommand> solutionCommands)
		{
			data = new Data (deck);

			
			logic = new Logic (data);



			this.solutionCommands = solutionCommands;		
		}
			
		public Solitaire (bool isSolvedDeck, bool isOneCardSet)
		{
			if (isSolvedDeck)
			{
				do
				{
					data = new Data ();
					solver = new Solver (data, isOneCardSet, false);
				}
				while (!solver.SolveAndBuild ());
				data = solver.GetData ();

					//solver.GetData ();
				logic = new Logic (data);
				solutionCommands = solver.GetSolution ();
			}
			else
			{
				data = new Data ();

					//data.Copy();
				logic = new Logic (data);
				solutionCommands = new List<ContractCommand> ();
			}
		}	
		#region Public
		public ConstantContainer GetStackHolderId()
		{
			return data.GetStackHolderId ();
		}
		public List<int> GetStartingCardsId()
		{
			return data.GetStartingCardsId ();
		}
		public List<Card> GetStartingCards()
		{
			return data.GetStartingCards ();
		}


        public void AddCard(List<Card> cards)
        {
            data.AddCard(cards);
        }
        [System.Obsolete("This is a Old Get")]
		public Conteiner GetDeck()
		{
			return data.GetDeck ();
		}
		[System.Obsolete("This is a Old Get")]
		public Conteiner[] GetThron()
		{
			return data.GetThron();
		}
		[System.Obsolete("This is a Old Get")]
		public Conteiner[] GetCommunity()
		{
			return data.GetCommunity();
		}
		#endregion
	
		#region Commands
		public bool CanAttach (int idFrom, int idTo)
		{
			return logic.CanAttach (idFrom, idTo);
		}

		public int ShouldOpenDownCard (int id)
		{
			return logic.ShouldOpenDownCard (id);
		}

		public bool HasBetterPlace (int id)
		{
			return logic.HasBetterPlace (id);
		}

		public int GetBetterPlace (int id)
		{
			return logic.GetBetterPlace (id);
		}
		public List<ContractCommand> GetHints ()
		{
			return logic.GetHints ();
		}
			
		public void ReverseDeck(bool isOpenCard)
		{
			logic.ReverseDeck (isOpenCard);
		}
		public void Move (int idFrom,int  idTo)
		{
			logic.Move (idFrom, idTo); // 
		}
		public void TurnCard (int id)
		{
			logic.TurnCard (id);
		}
		public int ShiftDeck ()
		{
			return logic.GetNextDeckCard ();
		}
		public void UndoShiftDeck ()
		{
			logic.UndoShiftDeck ();
		}
		public List<ContractCommand> GetSolution()
		{
			return solutionCommands;
		}

		public List<ContractCommand> GetCurrentSolution(bool isOneCardSet)
		{
			Data currentData = data.Copy ();
			Solver currentSolver = new Solver (currentData, isOneCardSet, true);
			if (currentSolver.SolveAndBuild ()) return currentSolver.GetSolution ();
			return new List<ContractCommand> ();
		}
			
		public void Restart ()
		{
			data.Restart ();
		}
		public bool IsComplete ()
		{
			return logic.IsComplete ();
		}
		public bool IsCommunityHasAllOpenCard()
		{
			return logic.IsCommunityHasAllOpenCard ();
		}
		public bool IsDeckEmpty()
		{
			return logic.IsDeckEmpty ();
		}

		public bool IsDeckOpenedAllCards()
		{
			return logic.IsDeckOpenedAllCards ();
		}

		public int DeckRestCardsCount()
		{
			return logic.DeckRestCardsCount ();
		} 
		public int DeckTotalCardsCount()
		{
			return logic.DeckTotalCardsCount ();
		} 
		public bool HasHintInDeck(bool isOneCardSet)
		{
			return logic.HasHintInDeck (isOneCardSet);
		}
			
		public bool IsIdInThronOrThronBase(int id)
		{
			IdResult resultId = data.IdAnalizator (id);
			return (resultId.isInThron || resultId.isInThronBase);
		}
		public bool IsIdInCommunityOrCommunityBase(int id)
		{
			IdResult resultId = data.IdAnalizator (id);
			return (resultId.isInCommunity || resultId.isInCommunityBase);
		}

		public void SetThronCardMove (int id, bool isMoved)
		{
			IdResult resultId = data.IdAnalizator (id);
			resultId.card.IsPayedForThron = isMoved;
		}

		public void SetCommunityCardMove (int id, bool isMoved)
		{
			IdResult resultId = data.IdAnalizator (id);
			resultId.card.IsPayedForCommunity = isMoved;
		}

		public bool GetThronCardMove (int id)
		{
			IdResult resultId = data.IdAnalizator (id);
			return resultId.card.IsPayedForThron;
		}
		public bool GetCommunityCardMove (int id)
		{
			IdResult resultId = data.IdAnalizator (id);
			return resultId.card.IsPayedForCommunity;
		}
		public int GetLastClosedCardInDeck ()
		{
			return logic.GetLastClosedCardInDeck ();
		}
		public int GetParentID(int id)
		{
			IdResult resultId = data.IdAnalizator (id);
			return resultId.parentId;
		}
		#endregion
		#region IDebugGame implementation


		public List<IDebugCardInfo> GetAllCards ()
		{
			List<IDebugCardInfo> cardDebugInfo = new List<IDebugCardInfo> ();
			for (int conteinerIndex = 0; conteinerIndex < data.Holder.Length; conteinerIndex++)
			{			
				for (int elementIndex = 0; elementIndex < data.Holder[conteinerIndex].Element.Count; elementIndex++)
				{
					Card cardSource = data.Holder [conteinerIndex].Element [elementIndex];

					IDebugCardInfo cardDebug = new IDebugCardInfo ();
					cardDebug.id = cardSource.Id;
					cardDebug.isOpen = cardSource.IsOpened;
					cardDebug.rank = DebugRank(cardSource.Rank);
					cardDebug.suit = DebugSuit(cardSource.Suit);
					cardDebug.zone = DebugZone(conteinerIndex);
					cardDebug.zoneIndex = DebugZoneIndex(conteinerIndex, cardSource);
					cardDebug.cardIndexInStack = (conteinerIndex.Equals (0)) ? DebugDeckIndexInStack (elementIndex) : elementIndex;
					cardDebugInfo.Add (cardDebug);
				}
			}
			return cardDebugInfo;
		}
		private IDebugCardInfo.Rank DebugRank(int rank)
		{
			return (IDebugCardInfo.Rank) rank;
		}
		private IDebugCardInfo.Suit DebugSuit(int suit)
		{
			return (IDebugCardInfo.Suit) suit;
		}
		private IDebugCardInfo.Zone DebugZone(int zone)
		{
			if (zone.Equals (0))
				return IDebugCardInfo.Zone.Deck;
			if (zone>=1 && zone<=4)
				return IDebugCardInfo.Zone.Foundation;
			if (zone>=5 && zone<=11)
				return IDebugCardInfo.Zone.Tableu;
			
			throw new System.Exception("Out of raange zone");
		}
		private int DebugZoneIndex(int zone, Card card)
		{
			if (zone.Equals (0))
			{
				return (card.IsOpened) ? 1 : 0;
			}
		
			if (zone >= 1 && zone <= 4)
				return (zone - 1);
			if (zone >= 5 && zone <= 11)
				return (zone - 5);
			
			throw new System.Exception("Out of range zone index");
		}
		private int DebugDeckIndexInStack (int elementIndex) 
		{
			bool isOpen = data.Holder [0].Element [0].IsOpened;
			int count = 0;
			int i = 0;
			while(i < elementIndex)
			{
				i++;
				count++;
				if (data.Holder [0].Element [i].IsOpened != isOpen)
				{
					count = 0;
					isOpen = data.Holder [0].Element [i].IsOpened;
				}
			}
			return count;
		}
		#endregion
	}
}