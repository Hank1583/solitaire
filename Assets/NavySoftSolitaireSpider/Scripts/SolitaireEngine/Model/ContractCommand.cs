namespace SolitaireEngine.Model 
{
	public class ContractCommand
	{
		public enum State {Nothing=0, Move, TurnCard, ShiftDeckOnece, ShiftDeckTwice, ShiftDeckThird, ReverseDeck};
		private State action;
		private int idFrom;
		private int idTo;
		private ContractCommand ()
		{
			this.action = State.Nothing;
			this.idFrom = -1;
			this.idTo = -1;
		}
		public ContractCommand (State state, int idFrom, int idTo) 
			: this()
		{
			this.action = state;
			this.idFrom = idFrom;
			this.idTo = idTo;
		}
		public ContractCommand (State state, int idFrom)
			: this()
		{
			this.action = state;
			this.idFrom = idFrom;
		}
		public ContractCommand (State state)
			: this()
		{
			this.action = state;
		}
		#region Public
		public State Action{get{return action;}}
		public int IdFrom{get{return idFrom;}}
		public int IdTo{get{return idTo;}}
		public ContractCommand Copy()
		{
			return new ContractCommand (action, idFrom, idTo);
		}
		#endregion
	}
}