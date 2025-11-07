 namespace SolitaireEngine
{
	using System;
	using System.Collections.Generic;
	using SolitaireEngine.Model;
	using SolitaireEngine.Utility;
    using UnityEngine;
	public class Data
	{
		public readonly ConstantContainer constants;
		private const int THRON_SLOTS_COUNT = 4;
		private const int COMMUNITY_SLOTS_COUNT = 10;
		private const int DECK_INDEX = 0;
		private const int THRON_MIN_INDEX = 1;
		private const int THRON_MAX_INDEX = 8;
		private const int COMMUNITY_MIN_INDEX = 5;
		private const int COMMUNITY_MAX_INDEX = 11;
		private const int CONTAINER_COUNT = 10;
		public int DeckIndex { get { return DECK_INDEX; }} 
		public int ThronMin {get{return THRON_MIN_INDEX; }}
		public int ThronMax {get{return THRON_MAX_INDEX; }}
		public int CommunityMin {get{return COMMUNITY_MIN_INDEX; }}
		public int CommunityMax {get{return COMMUNITY_MAX_INDEX; }}

		public static readonly int NULL_CARD = -1;
		private readonly int DECK_PIVOT = -100;
		private readonly int[] THRON_PIVOTS = {-101,-102,-103,-104,-105,-106,-107,-108};
		private readonly int[] COMMUNITY_PIVOTS = {-109,-110,-111,-112,-113,-114, -115, -116, -117, -118, };

		private Conteiner[] holder;
		private Conteiner[] fixHolder;
		private Utils utils;
        private CardItem[] cards;
		public Data ()
		{
			utils = new Utils ();
			holder = new Conteiner[CONTAINER_COUNT];
           
            SetDeck (CreateTotalDeck ());

			fixHolder = CopyHolder (holder);
		}
		public Data (Conteiner[] holder)
		{
			utils = new Utils ();
			this.holder = CopyHolder (holder);
			this.fixHolder = CopyHolder (holder);
		}
		public Data (List<Card> deck)
		{
			utils = new Utils ();
			holder = new Conteiner[CONTAINER_COUNT];
           
			SetDeck (deck);
			fixHolder = CopyHolder (holder);
		}

		private void SetDeck (List<Card> deck)
		{
			List<Card>[] communityList;
			List<Card>[] thronList;
			List<Card> deckList;
			List<Card> workDeck = deck;

			communityList = CreateCommunityList (workDeck);
			thronList = CreateThronList ();
			deckList = workDeck;
			SetConteiners (deckList, thronList, communityList);
			//OpenAllCommunityLastCard ();
		}
 

		private void SetConteiners(List<Card> deckList, List<Card>[] thronList, List<Card>[] communityList)
		{
			holder [DECK_INDEX] = CreateDeckContainer (deckList);
 
			Conteiner[] communityContainer = CreateCommunityContainer (communityList);
        
			Array.Copy (communityContainer, 0, holder, 0, COMMUNITY_SLOTS_COUNT);
		}
		private Conteiner[] CopyHolder(Conteiner[] workHolder)
		{
			Conteiner[] copyHolder = new Conteiner[CONTAINER_COUNT];
           
            for (int containerIndex = 0; containerIndex < CONTAINER_COUNT; containerIndex++) copyHolder [containerIndex] = workHolder [containerIndex].Copy ();
           
             
			return copyHolder;
		}
		#region CreateList
		private List<Card> CreateTotalDeck ()
		{
			List<Card> totalDeck = new List<Card> ();
			for (int suitIndex = 0; suitIndex < 4; suitIndex++)
				for (int rankIndex = 1; rankIndex < 14; rankIndex++)
					totalDeck.Add (new Card (utils.GenerateUniqueID (), rankIndex, suitIndex));
			List<Card> shuffleDeck = utils.ShuffleList (totalDeck);

           
			return shuffleDeck;
		}
		private List<Card>[] CreateCommunityList (List<Card> _workDeck)
		{
			List<Card>[] communityList = new List<Card>[COMMUNITY_SLOTS_COUNT];

            int[] slot = new int[10] { 6, 6, 6, 6, 5, 5, 5, 5, 5, 5 };


			for (int communityIndex = 0; communityIndex < COMMUNITY_SLOTS_COUNT; communityIndex++) {
				communityList [communityIndex] = new List<Card> ();
                for (int cardIndex = 0; cardIndex < slot[communityIndex]; cardIndex++)

                {
                    Card card = _workDeck[0].Copy();  
                    _workDeck.RemoveAt(0);
 
                    communityList[communityIndex].Add(card);


                }
			}
			return communityList;
		}
		private List<Card>[] CreateThronList ()
		{
			List<Card>[] thronList = new List<Card>[THRON_SLOTS_COUNT];
			for (int thronIndex = 0; thronIndex < THRON_SLOTS_COUNT; thronIndex++)
				thronList [thronIndex] = new List<Card> ();
			return thronList;
		}
		#endregion
		#region CreateContainer
		private Conteiner CreateDeckContainer (List<Card> deckList)
		{
             
			return new Conteiner (DECK_PIVOT, deckList);
		}
        
		private Conteiner[] CreateCommunityContainer (List<Card>[] communityList)
		{
             


			Conteiner[] communityContainer = new Conteiner[COMMUNITY_SLOTS_COUNT];
            for (int communityIndex = 0; communityIndex < COMMUNITY_SLOTS_COUNT; communityIndex++)
            {
               
                communityContainer[communityIndex] = new Conteiner(COMMUNITY_PIVOTS[communityIndex], communityList[communityIndex]);
            }
			return communityContainer;
		}
		#endregion
		#region CreateSupport
		private void OpenAllCommunityLastCard ()
		{
			for (int communityIndex = COMMUNITY_MIN_INDEX; communityIndex <= COMMUNITY_MAX_INDEX; communityIndex++) {
				int length = holder [communityIndex].Element.Count;
				holder [communityIndex].Element [length - 1].IsOpened = true;
			}
		}
		private Card TakeRandomCard (List<Card> _totalDeck)
		{
			Card randomCard = utils.RandomElement (_totalDeck);
			Card card = randomCard.Copy ();
			_totalDeck.Remove (randomCard);
			return card;
		}
		#endregion

		#region Public

		public ConstantContainer GetStackHolderId()
		{
			return new ConstantContainer (NULL_CARD, DECK_PIVOT, THRON_PIVOTS, COMMUNITY_PIVOTS);
		}

		public List<int> GetStartingCardsId()
		{
			List<int> cardsId = new List<int> ();
            for (int i = 0; i < StageManager.instance.GetCardContainer.Count; i++)
            {
                cardsId.Add(StageManager.instance.GetCardContainer[i].Id);
            }
			return cardsId;
		}


        public void AddCard(List<Card> cards)
        {
           
            for (int i = 0; i < cards.Count; i++)
            {
                holder[i].Element.Add(cards[i]);
            }
        }
		public List<Card> GetStartingCards()
		{
            List<Card> cards = new List<Card> ();
            List<Card> cardsTemp = new List<Card>();
           
 
            return StageManager.instance.GetCardContainer;
		}
 
        public Conteiner[] Holder {get{ return holder;}}


		[System.Obsolete("This is a Old Get")]
		public Conteiner GetDeck()
		{
			return holder[DECK_INDEX].Copy();
		}
		[System.Obsolete("This is a Old Get")]
		public Conteiner[] GetThron()
		{
			Conteiner[] thron = new Conteiner[THRON_SLOTS_COUNT];
			Array.Copy (holder, THRON_MIN_INDEX , thron, 0, THRON_SLOTS_COUNT); 
			return thron;
		}
		[System.Obsolete("This is a Old Get")]
		public Conteiner[] GetCommunity()
		{
			Conteiner[] tempCont = CopyHolder(fixHolder);
			Conteiner[] community = new Conteiner[COMMUNITY_SLOTS_COUNT];
			Array.Copy (tempCont, COMMUNITY_MIN_INDEX, community, 0, COMMUNITY_SLOTS_COUNT); 
			return community;
		}
			
		public Data Copy()
		{
			return new Data (holder);
		}
		public void Restart()
		{
			holder = CopyHolder (fixHolder);
		}
		#endregion
		#region Utiliy
		public IdResult IdAnalizator (int id)
		{
			IdResult IdResult = new IdResult ();
            if (cards == null) cards = MonoBehaviour.FindObjectsOfType<CardItem>();
          


            for (int communityIndex = 0; communityIndex < holder.Length; communityIndex++)
            {
            

                if (holder[communityIndex].Id.Equals(id))
                {
                   
                    IdResult.id = id;
                    IdResult.isFind = true;
                    IdResult.isBase = true;
                    IdResult.isInCommunityBase = true;
                    IdResult.conteinerIndex = communityIndex;
                    return IdResult;
                }
            }
 
			for (int containerIndex = 0; containerIndex < CONTAINER_COUNT; containerIndex++)
            {
                for (int elementIndex = 0; elementIndex < holder [containerIndex].Element.Count; elementIndex++)
                {


                
                    if (holder [containerIndex].Element [elementIndex].Id.Equals (id))
                    {
                   

                        IdResult.id = id;
                        IdResult.isFind = true;
                        IdResult.isCard = true;
                        IdResult.card = holder[containerIndex].Element[elementIndex];
                        IdResult.conteinerIndex = containerIndex;
                        IdResult.elementIndex = elementIndex;
                        if (containerIndex.Equals(DECK_INDEX)) IdResult.isInDeck = true;
                        if (containerIndex >= THRON_MIN_INDEX && containerIndex <= THRON_MAX_INDEX) IdResult.isInThron = true;

                        if (containerIndex >= COMMUNITY_MIN_INDEX && containerIndex <= COMMUNITY_MAX_INDEX) IdResult.isInCommunity = true;
                        if (elementIndex.Equals(0)) IdResult.isFirst = true;
                        if (elementIndex.Equals(holder[containerIndex].Element.Count - 1)) IdResult.isLast = true;
                        IdResult.parentId = (IdResult.isFirst) ? holder[IdResult.conteinerIndex].Id : holder[IdResult.conteinerIndex].Element[IdResult.elementIndex - 1].Id;
                    }
                }
            }
       
            return IdResult;
		}



        public int GetCountElementsInHolder(int column)
		{
           
    		return holder [column].Element.Count;
		}

		public void ChangeTwoCards (int id1, int id2)
		{
			IdResult resultId1 = IdAnalizator (id1);
			IdResult resultId2 = IdAnalizator (id2);

			Card copyCard1 = holder [resultId1.conteinerIndex].Element [resultId1.elementIndex].Copy ();
			Card copyCard2 = holder [resultId2.conteinerIndex].Element [resultId2.elementIndex].Copy ();
	
			holder[resultId1.conteinerIndex].Element[resultId1.elementIndex] = new Card (copyCard2.Id, copyCard2.Rank, copyCard2.Suit, copyCard1.IsOpened); // exept IsOpened
			holder[resultId2.conteinerIndex].Element[resultId2.elementIndex] = new Card (copyCard1.Id, copyCard1.Rank, copyCard1.Suit, copyCard2.IsOpened); // exept IsOpened
		}
		public bool SetOpenCard(int id, bool value)
		{
			IdResult resultId = IdAnalizator (id);
			if (resultId.isCard)
			{
				holder [resultId.conteinerIndex].Element [resultId.elementIndex].IsOpened = value;
				return true;
			}
			return false;
		}
		public bool IsDeckOpen()
		{
			int length = holder [DECK_INDEX].Element.Count;
			if (length.Equals (0)) return false;
			return holder [DECK_INDEX].Element [length - 1].IsOpened;
		}
		public bool IsDeckOneCardFinished()
		{
			int length = holder [DECK_INDEX].Element.Count;
			if (length.Equals (0)) return false;
			return holder [DECK_INDEX].Element [0].IsOpened;
		}
		public bool IsDeckThreeCardsFinished()
		{
			int length = holder [DECK_INDEX].Element.Count;
			if (length<3) return false;
			return holder [DECK_INDEX].Element [0].IsOpened;
		}
		#endregion
		#region Commands
		public void TurnCard(int id)
		{
			IdResult resultId = IdAnalizator (id);
			resultId.card.IsOpened = !resultId.card.IsOpened;
		}
	
		public bool Move (int idFrom, int idTo) // Check Deck copy from and to
		{
         
			IdResult resultFrom = IdAnalizator (idFrom);
			IdResult resultTo = IdAnalizator (idTo);
			if (!resultFrom.isFind || !resultTo.isFind) return false;		
			if (idFrom.Equals (idTo)) return false;
			if (!resultFrom.isCard) return false;
			int length = holder [resultFrom.conteinerIndex].Element.Count;
			for (int copyIndex = resultFrom.elementIndex; copyIndex < length; copyIndex++) 
				holder [resultTo.conteinerIndex].Element.Add (holder [resultFrom.conteinerIndex].Element[copyIndex].Copy ());
		
			int countDelete = holder [resultFrom.conteinerIndex].Element.Count - resultFrom.elementIndex;
			holder [resultFrom.conteinerIndex].Element.RemoveRange (resultFrom.elementIndex, countDelete);
          
			return true;
		}
		public bool MoveToEndList(int id)
		{
			IdResult resultFrom = IdAnalizator (id);
			if (!resultFrom.isFind || !resultFrom.isCard) return false;
			Card copyCard = holder [resultFrom.conteinerIndex].Element [resultFrom.elementIndex].Copy ();
			holder [resultFrom.conteinerIndex].Element.RemoveAt (resultFrom.elementIndex);
			holder [resultFrom.conteinerIndex].Element.Add (copyCard);
			return true;
		}
		public bool MoveToHomeList(int id)
		{
			IdResult resultFrom = IdAnalizator (id);
			if (!resultFrom.isFind || !resultFrom.isCard) return false;
			Card copyCard = holder [resultFrom.conteinerIndex].Element [resultFrom.elementIndex].Copy ();
			holder [resultFrom.conteinerIndex].Element.RemoveAt (resultFrom.elementIndex);
			List<Card> deck = new List<Card> (); 
			deck.Add (copyCard);
			foreach (Card element in holder [resultFrom.conteinerIndex].Element) deck.Add (element.Copy());
			holder [resultFrom.conteinerIndex].SetElement (deck);
			return true;		
		}
        #endregion





        //Method for spider Game
        public IdResult FindCardItem(int id)
        {
            IdResult IdResult = new IdResult();

            if (cards ==null) cards = MonoBehaviour.FindObjectsOfType<CardItem>();

            for (int i = 0; i < cards.Length; i++)
            {
                if(cards[i].Id == id)
                {
                    IdResult.id = id;
                    IdResult.card.Rank = cards[i].Rank;
                    IdResult.card.Suit = cards[i].Suit;
                    IdResult.isInCommunity = true;
                    IdResult.isCard = true;
                    break;
                }
            }


            if(id < 0)
            {
                IdResult.id = id;
                IdResult.isInCommunity = false;
                IdResult.isCard = true;
                IdResult.isInCommunityBase = true;
            }


            return IdResult;
        }

    }
}
