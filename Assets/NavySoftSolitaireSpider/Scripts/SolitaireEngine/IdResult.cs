namespace SolitaireEngine
{
	using System.Collections.Generic;
	using SolitaireEngine.Model;
	public class IdResult
	{
		public int id;
		public bool isFind;
		public bool isBase;
		public bool isInThronBase;
		public bool isInCommunityBase;
		public bool isCard;
		public bool isInDeck;
		public bool isInThron;
		public bool isInCommunity;
		public Card card;
		public int conteinerIndex;
		public int elementIndex;
		public bool isFirst;
		public bool isLast;
		public int parentId;
		public IdResult()
		{
			id = -1;
			isFind = false;
			isBase = false;
			isInThronBase = false;
			isInCommunityBase = false;
			isCard = false;
			isInDeck = false;
			isInThron = false;
			isInCommunity = false;
			card = new Card(-1,-1,-1); 
			conteinerIndex = -1;
			elementIndex = -1;
			isFirst = false;
			isLast = false;
			parentId = -1;
		}
	}
}