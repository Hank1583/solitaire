namespace SolitaireEngine
{
	using UnityEngine.Events;
	using System.Collections.Generic;
	using SolitaireEngine.Model;
	using SolitaireEngine.Utility;


	public class Logic
	{
		private Data data;
		private Utils utils;
		private Logic(){}
		public Logic (Data _data)
		{
			this.data = _data;
			utils = new Utils ();
		 
		}

        private CardItem[] cards;


        #region CanAttach
        private bool CanPutCardToCommunityBase (IdResult resultFrom, IdResult resultTo)
		{
         
            if (resultTo.isInCommunityBase && resultFrom.isCard ) return true;
			return false;
		}
		private bool CanPutCardToThronBase (IdResult resultFrom, IdResult resultTo)
		{
           //UnityEngine.Debug.Log("CanPutCardToThronBase");
			 if (resultTo.isInThronBase && resultFrom.isCard && resultFrom.card.IsAce) return true;
			return false;
		}
		private bool CanPutCardToCommunityCards (IdResult resultFrom, IdResult resultTo)
        {
           
            if (resultTo.isInCommunity && resultFrom.isCard && (resultTo.card.Rank - resultFrom.card.Rank).Equals (1)) return true;
            // && resultFrom.isCard
            return false;
		}
		private bool CanPutCardToThronCards (IdResult resultFrom, IdResult resultTo)
        {
 
            if (resultTo.isInThron && resultFrom.isCard && resultTo.card.Suit.Equals(resultFrom.card.Suit) && (resultFrom.card.Rank - resultTo.card.Rank).Equals (1))
			{
				if (resultFrom.isInCommunity && !resultFrom.isLast) return false;
				return true;
			}
			return false;
		}
		private bool SetOpenCard(int id, bool value)
		{
			bool isSet = data.SetOpenCard (id, value);
			return isSet;
		}

		#endregion

		#region GetBetterPlace
		private List<int> FindSolverPlace (int id) // attantion solver use it
		{
 
			 
		 
	 

			IdResult resultTab = data.FindCardItem (id);


            List<int> sencePlaces = new List<int>();


            if (cards == null) cards = MonoHandler.FindObjectsOfType<CardItem>();


            bool addHint = false;
            for (int i = 0; i < cards.Length; i++)
            {
                if (!cards[i].isOppened) continue;
                if ( cards[i].Hide) continue;
                if (cards[i].Id == resultTab.id)
                {
                    if (cards[i].parentCard != null)
                    {
                        if (cards[i].parentCard.Rank - cards[i].Rank == 1 && cards[i].parentCard.isOppened)
                        {
                            addHint = true;
                            break;
                        }
                    }
                }


 
            }


            if (!addHint)
            {
                for (int i = 0; i < cards.Length; i++)
                {
                    if (cards[i].isOppened && cards[i].childCard == null && !cards[i].Hide)
                    {
                        if (cards[i].Rank - resultTab.card.Rank == 1)
                        {

                            sencePlaces.Add(cards[i].Id);
                        }
                    }
                }


                if (sencePlaces.Count == 0)
                {
                    for (int i = 0; i < SolitaireStageViewHelperClass.instance.GetTableuStacks.Count; i++)
                    {
                        CardItem tableuStack = SolitaireStageViewHelperClass.instance.GetTableuStacks[i];
                        if (tableuStack.childCard == null)
                        {
                            sencePlaces.Add(tableuStack.Id);
                            break;
                        }
                    }
                }
            }
          



            return sencePlaces;
		}

		private List<int> FindUserPlace (int id)
		{
            List<int> sencePlaces = new List<int>();
          
            if (cards == null) cards = MonoHandler.FindObjectsOfType<CardItem>();

            IdResult resultTab = data.FindCardItem(id);
            CardItem cardTableu = SolitaireStageViewHelperClass.instance.FindTableu(SolitaireStageViewHelperClass.instance.FindCardItem(resultTab.id));
            int cardConnectLength = 0;
            bool flagConnect = false;
            for (int i = 0; i < SolitaireStageViewHelperClass.instance.GetTableuStacks.Count; i++)
            {
                CardItem [] cardsChild = SolitaireStageViewHelperClass.instance.GetTableuStacks[i].GetComponentsInChildren<CardItem>();
                CardItem lastCard = cardsChild[cardsChild.Length - 1];
                if (cardTableu.Id == SolitaireStageViewHelperClass.instance.GetTableuStacks[i].Id) continue;
                if (lastCard.MoveComplete) continue;
                if ( lastCard.Rank - resultTab.card.Rank == 1)
                {
                  
                    if (!flagConnect)
                    {
                    
                        cardConnectLength = lastCard.LengthConnectCardInGroup();
                        sencePlaces.Add(lastCard.Id);
                        flagConnect = true;
                
                    }
                    else
                    {
                    
                        if (cardConnectLength < lastCard.LengthConnectCardInGroup())
                        {
                            sencePlaces.Clear();
                            sencePlaces.Add(lastCard.Id);
                        }
                    }
                 
                }

            }





            if(sencePlaces.Count==0)
            {

                for (int i = 0; i < SolitaireStageViewHelperClass.instance.GetTableuStacks.Count; i++)
                {
                    if(!SolitaireStageViewHelperClass.instance.GetTableuStacks[i].hasChildCard)
                        sencePlaces.Add(SolitaireStageViewHelperClass.instance.GetTableuStacks[i].Id);
                }
            }


                return sencePlaces;
		}



		private List<int> FindAllPlaces (int id)
		{

       
            List<int> totalPlaces = new List<int> ();
			IdResult resultTab = data.IdAnalizator (id);
          
             if (!resultTab.isCard) return totalPlaces;
           
            List<int> community = FindPlacesInCommunity (resultTab);
            totalPlaces = utils.AddTo (community, totalPlaces);
 
            return totalPlaces;
		}
		private List<int> FindPlacesInThron (IdResult resultTab)
		{


          //  UnityEngine.Debug.Log("FindPlacesInThron");
            List<int> result= new List<int>();
			for (int thronIndex = data.ThronMin; thronIndex <= data.ThronMax; thronIndex++) 
			{
				int length = data.Holder[thronIndex].Element.Count;
				if (length > 0)
				{
					Card lastCard = data.Holder [thronIndex].Element [length - 1];
					if ((resultTab.card.Rank-lastCard.Rank).Equals(1) && resultTab.card.Suit.Equals( lastCard.Suit)) result.Add (lastCard.Id);
				}
			}
			return result;
		}
		private List<int> FindPlacesInCommunity (IdResult resultTab)
		{
 
          
            List<int> result = new List<int>();

            if (cards == null) cards = MonoHandler.FindObjectsOfType<CardItem>();





            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i].Id == resultTab.id)
                {
                    CardItem current = cards[i];
                    if (current.childCard != null)
                    {
                        CardItem next = cards[i].childCard;

                        while (true)
                        {
                            if(current.Rank - next.Rank == 1 && current.Suit!=next.Suit)
                            {
                                current = next;
                                if (next.childCard == null)
                                    break;
                                next = next.childCard;
                            }
                            else
                            {
                                return result;
                            }
                        }
                    }

                    break;
                   
                }
            }

 
          
            for (int communityIndex = 0; communityIndex < data.CommunityMax - 1; communityIndex++)
            {
                int length = data.Holder[communityIndex].Element.Count;
                if (length > 0)
                {
                    Card lastCard = data.Holder[communityIndex].Element[length - 1];
                    if ((lastCard.Rank - resultTab.card.Rank).Equals(1) ) result.Add(lastCard.Id);
                }
            }
            return result;
		}
		private List<int> FindPlacesInThronBase () 
		{
   
            List<int> result= new List<int>();
			for (int thronIndex = data.ThronMin; thronIndex <= data.ThronMax; thronIndex++)
			{
				if (data.Holder [thronIndex].Element.Count.Equals (0)) result.Add (data.Holder [thronIndex].Id);
			}
			return result;
		}
		private List<int> FindPlacesInCommunityBase ()
		{


          
            List<int> result= new List<int>();
			for (int communityIndex = data.CommunityMin; communityIndex <=  data.CommunityMax; communityIndex++)
			{
				if (data.Holder [communityIndex].Element.Count.Equals (0)) result.Add (data.Holder [communityIndex].Id);
			}
			return result;
		}
		#endregion
		#region GetHints
		private List<ContractCommand> GetDeckCardHints() // attantion solver use it
		{
			List<ContractCommand> hintsCommand = new List<ContractCommand> ();
			int length = data.GetCountElementsInHolder (data.DeckIndex);
			if (length.Equals(0)) return hintsCommand;
			if (data.IsDeckOpen())
			{
				int deckCardId = data.Holder [data.DeckIndex].Element [length - 1].Id;
				List<int> deckHints = FindSolverPlace (deckCardId);
				foreach (int idTo in deckHints) hintsCommand.Add(new ContractCommand(ContractCommand.State.Move,deckCardId, idTo));
			}
			return hintsCommand;
		}
			
		private List<ContractCommand> GetFirstAndLastOpenCommunityCardHints( ) // attantion solver use it
		{


           

            List<ContractCommand> hintsCommand = new List<ContractCommand> ();
            if (cards==null ) cards = MonoHandler.FindObjectsOfType<CardItem>();





            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i].isOppened && !cards[i].Hide)
                {
                    CardItem card = cards[i];
                    CardItem childCard = cards[i].childCard;
                    bool flag = true;

                    while (childCard != null)
                    {
 
                      
                        if ((card.Rank - childCard.Rank > 1 || card.Rank - childCard.Rank <= 0))
                        {
                           
                            flag = false;
                            break;
                        }

                      
                        card = childCard;
                        childCard = card.childCard;

                    }
 

                    if (flag)
                    {
                        int firstOpenCardId = cards[i].Id;



                        List<int> firstCommunityHints = FindSolverPlace(firstOpenCardId);

                        foreach (int idTo in firstCommunityHints)
                        {
                            hintsCommand.Add(new ContractCommand(ContractCommand.State.Move, firstOpenCardId, idTo));
                        }
                    }
                }
            }

          
            if (hintsCommand.Count == 0 && !SolitaireStageViewHelperClass.instance.EmptySlot)
            {
                hintsCommand.Add(new ContractCommand(ContractCommand.State.ShiftDeckOnece, SolitaireStageViewHelperClass.instance.GetCurrentStock.Id, SolitaireStageViewHelperClass.instance.GetCurrentStock.Id));
            }

            for (int i = 1; i < hintsCommand.Count; i++)
            {
                hintsCommand.RemoveAt(i);
                i--;
            }


            return hintsCommand;

 
		}
		#endregion

		#region Public
		public bool CanAttach (int idFrom, int idTo)
		{
         //   UnityEngine.Debug.Log("idFrom " + idFrom);
         //   UnityEngine.Debug.Log("idTo " + idTo);
            /*
             * code origin of game
            IdResult resultFrom = data.IdAnalizator (idFrom);
			IdResult resultTo = data.IdAnalizator (idTo);
            */


            IdResult resultFrom = data.FindCardItem(idFrom);
            IdResult resultTo = data.FindCardItem(idTo);


//            UnityEngine.Debug.Log("CanAttach resultFrom " + resultFrom.card.Rank) ;
           // UnityEngine.Debug.Log("CanAttach resultTo " + resultTo.card.Rank);


            if (CanPutCardToCommunityBase (resultFrom, resultTo) ||
			    CanPutCardToThronBase (resultFrom, resultTo) ||
			    CanPutCardToCommunityCards (resultFrom, resultTo) ||
			    CanPutCardToThronCards (resultFrom, resultTo))
				return true;
			return false;
		}

		public int ShouldOpenDownCard (int id)
		{
			IdResult resultId = data.FindCardItem (id);
			int parentId = resultId.parentId;

			IdResult resultParentId = data.FindCardItem (parentId);
			bool isHaveToTurn = (resultParentId.isFind && resultParentId.isCard && resultParentId.isInCommunity && !resultParentId.card.IsOpened);
			if (isHaveToTurn) return parentId;
			return Data.NULL_CARD;
		}
        public List<ContractCommand> GetHints() // attantion solver use it
        {
            List<ContractCommand> hints = new List<ContractCommand>();
 

            List<ContractCommand> communityHint = GetFirstAndLastOpenCommunityCardHints();

 

            return communityHint;
        }


		public bool HasBetterPlace (int id)
		{
          
			List<int> allCombs = FindUserPlace (id);
			return (allCombs.Count.Equals(0)) ? false : true;
		}
		public int GetBetterPlace (int id)
		{
			List<int> allCombs = FindUserPlace (id);
			if (allCombs.Count>0) return utils.RandomElement(allCombs);
			return Data.NULL_CARD;
		}


		public void ReverseDeck(bool isOpenCard)
		{
			foreach (Card element in data.Holder [data.DeckIndex].Element) element.IsOpened = isOpenCard;
		}

		public bool Move (int idFrom, int  idTo)
		{
			return data.Move (idFrom, idTo);
		}

		public void TurnCard (int id)
		{
			data.TurnCard (id);
		}
		public int GetNextDeckCard ()
		{
			int nextCardId = Data.NULL_CARD;
			int deckLength = data.GetCountElementsInHolder (data.DeckIndex);
			if (deckLength > 0)
			{
				nextCardId = data.Holder [data.DeckIndex].Element [0].Id;
				if (deckLength > 1) data.MoveToEndList (nextCardId);
				data.Holder [data.DeckIndex].Element [deckLength-1].IsOpened = true;
			}
			return nextCardId;
		}
		public void UndoShiftDeck ()
		{
			int deckLength = data.GetCountElementsInHolder (data.DeckIndex);
			Card undoCard = data.Holder [data.DeckIndex].Element [deckLength - 1];
			undoCard.IsOpened = false;
			data.MoveToHomeList (undoCard.Id);
		}
		public bool IsComplete ()
		{
       
             return (  SolitaireSpiderCheck.instance.RowsComplete >= 8);//&& isThronFull);
		}
		public bool IsCommunityHasAllOpenCard()
		{
			bool isCommunityHasAllOpenCard = true;
			for (int communityIndex = 0; communityIndex <  data.CommunityMax-1; communityIndex++)
				foreach (Card element in data.Holder [communityIndex].Element)
					if (!element.IsOpened) isCommunityHasAllOpenCard = false;
			return (isCommunityHasAllOpenCard);
		}
		public bool IsDeckEmpty()
		{
			bool isDeckEmpty = (data.Holder [data.DeckIndex].Element.Count.Equals (0));
			return isDeckEmpty;
		}
		public bool IsDeckOpenedAllCards()
		{
			if (IsDeckEmpty ())
				return true;
			bool isDeckOpenedAllCards = true;
			foreach (Card element in data.Holder [data.DeckIndex].Element)
				if (!element.IsOpened)
					isDeckOpenedAllCards = false;
			return isDeckOpenedAllCards;
		}
		public int DeckRestCardsCount()
		{
			int lastClosedCardIndex = GetLastClosedCardInDeck ();
			IdResult result = data.IdAnalizator (lastClosedCardIndex);
			if (result.isBase)
				return 0;
			return result.elementIndex + 1;
		} 
		public int DeckTotalCardsCount()
		{
			return data.GetCountElementsInHolder (data.DeckIndex);
		}
		public bool HasHintInDeck (bool isOneCardSet)
		{
			int length = data.GetCountElementsInHolder (data.DeckIndex);
			if (length.Equals(0)) return false;

			if (isOneCardSet)
						
			for (int index = 0; index < length; index++)
			{
				int deckCardId = data.Holder [data.DeckIndex].Element [index].Id;
				List<int> deckHints = FindSolverPlace (deckCardId);
				if (!deckHints.Count.Equals (0))
					return true;
			}
			
			else
			{
				int index = -1;
				do {
					index = (index + 3 < length) ? index + 3 : length - 1;

					int deckCardId = data.Holder [data.DeckIndex].Element [index].Id;
					List<int> deckHints = FindSolverPlace (deckCardId);
					if (!deckHints.Count.Equals (0))
						return true;
						//UnityEngine.Debug.Log (data.IdAnalizator (deckCardId).card.Rank);
				}
				while (index < (length - 1));
			}
			return false;
		}			
		#endregion
		public int GetLastClosedCardInDeck()
		{
			int deckCloseId = data.GetStackHolderId ().DECK_STACK_HOLDER;
			int deckIndex = data.DeckIndex;

            foreach (Card element in data.Holder[deckIndex].Element)
            {
                if (!element.IsOpened) deckCloseId = element.Id;
            }




			return deckCloseId;
		}
	}
}
