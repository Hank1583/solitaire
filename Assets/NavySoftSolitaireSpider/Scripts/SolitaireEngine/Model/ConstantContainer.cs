namespace SolitaireEngine.Model 
{
	public class ConstantContainer
	{
		private int nullCard;
		private int deckStackHolder;
		private int[] foundationStackHolder;
		private int[] tableauStackHolder;

		private ConstantContainer(){}
		public ConstantContainer(int null_card, int deck_stack_holder, int[] foundation_stack_holder, int[] tableau_stack_holder)
		{
			this.nullCard = null_card;
			this.deckStackHolder = deck_stack_holder;
			this.foundationStackHolder = foundation_stack_holder;
			this.tableauStackHolder = tableau_stack_holder;
		}
		public int NULL_CARD {get{return nullCard;}}
		public int DECK_STACK_HOLDER {get{return deckStackHolder;}}
		public int[] FOUNDATION_STACK_HOLDER {get{return foundationStackHolder;}}
		public int[] TABLEAU_STACK_HOLDER{get{return tableauStackHolder;}}
	}
}