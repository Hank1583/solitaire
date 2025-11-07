public interface IViewBaseCommands {
	void Init (int null_card, int deck_stack_holder, int[] foundation_stack_holder, int[] tableau_stack_holder, 
		System.Collections.Generic.List<SolitaireEngine.Model.Card> startingCards, string[,] dealContract);

	void Restart (System.Collections.Generic.List<int> list, UnityEngine.Events.UnityAction callback);

	void MoveCard (int id, int destination_id, bool animation, bool move_back_to_parent ,bool moveCurve );
	void TurnCard (int id,bool movePrevCard = false);
	void ShakeCard (int id);
	void BlinkCard (int id);
	void UnblinkAll ();
	int ShiftDeck (bool forward);
	void TurnDeck (bool forward);
}