using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class StageView : MonoBehaviour, IViewBaseCommands {

	// create singleton
	public static StageView instance;
	void Awake(){
		instance = this;
	}

	// find view
	private SolitaireStageViewHelperClass view;
	private CardItemsDeck cardsDeck;

	void Start(){
		view = SolitaireStageViewHelperClass.instance;
		cardsDeck = CardItemsDeck.instance;
	}
		

	#region What can we do with stage view
	/// <summary>
	/// Init the specified deck, thron and community with starting cards.
	/// </summary>
	void IViewBaseCommands.Init(int null_card, int deck_stack_holder, int[] foundation_stack_holder, int[] tableau_stack_holder, 
		List<SolitaireEngine.Model.Card> startingCards, string[,] contract_to_deal) {
		  view.Init (null_card, deck_stack_holder, foundation_stack_holder, tableau_stack_holder, startingCards, contract_to_deal) ;
	}


	// TODO ADD: DealCards(contract, cards, callback) - will spawn all cards and deal it



	// TODO ADD: Reset() - will remove all cards. Maybe back all cards into the deck




	// TODO: Remove it because view sould not know about restart logic
	public void Restart (List<int> list, UnityAction callback) {
		view.Reset ( callback);
	}

	/// <summary>
	/// Moves the card.
	/// </summary>
	/// <param name="id">Identifier.</param>
	/// <param name="destination_id">Destination identifier.</param>
	/// <param name="immediately">If set to <c>true</c> immediately.</param>
	/// <param name="and_back">If set to <c>true</c> and back.</param>
	void IViewBaseCommands.MoveCard (int id, int destination_id, bool animation, bool move_back,bool moveCurve  ) {
        if (!ContinueModeGame.instance.LoadSuccess)
            return;

#if UNITY_EDITOR
        if (!view.hasCardById (id) || !view.hasCardById (destination_id)) {
			throw new UnityException ("can't find card!");
		}

#endif

        CardItem c_from = view.getCardById(id);
		CardItem c_to = view.getCardById (destination_id);

		if (move_back) {
			CardItem parent_card = c_from.parentCard;
        //    Debug.Log("Show Hint");
			view.ShowMoveHint (c_from, c_to, parent_card);		
		} else {
			if (!c_from.isOppened) {
//				throw new UnityException ("Closed cards can't be moved! id: " + c_from.Id);
			}

        // Debug.Log("MoveCard 5 " + animation);
          view.MoveCard (c_from, c_to,moveCurve, animation );
		}
	}

    /// <summary>
    /// Turns the card.
    /// </summary>
    /// <param name="id">Identifier.</param>
    void IViewBaseCommands.TurnCard(int id, bool visible  ) {
#if UNITY_EDITOR
        if (!view.hasCardById (id)) {
       		 throw new UnityException ("can't find card!");
          
		}
#endif
        CardItem card = view.getCardById (id);

		view.TurnCard (card, visible);
	}

	/// <summary>
	/// Shake the card.
	/// </summary>
	/// <param name="id">Identifier.</param>
	void IViewBaseCommands.ShakeCard (int id) {
		if (!view.hasCardById (id)) {
			//throw new UnityException ("can't find card!");
            return;
		}
		view.ShakeCard (view.getCardById(id));
	}

	/// <summary>
	/// Highlight the card.
	/// </summary>
	/// <param name="id">Identifier.</param>
	void IViewBaseCommands.BlinkCard (int id)

	{
      //  Debug.Log("ID " +id);
        if (SolitaireStageViewHelperClass.instance.FindCardItem(id)!=null)
        SolitaireStageViewHelperClass.instance.ShakeCard(SolitaireStageViewHelperClass.instance.FindCardItem(id));
	//	view.HighlightCard (view.getCardById(id));
	}

	/// <summary>
	/// Unblink all the cards.
	/// </summary>
	void IViewBaseCommands.UnblinkAll ()
	{
		 
	}

	/// <summary>
	/// Shifts the deck.
	/// </summary>
	/// <returns>The deck.</returns>
	/// <param name="forward">If set to <c>true</c> forward.</param>
	int IViewBaseCommands.ShiftDeck(bool forward){
//		int card_id = cardsDeck.ShiftDeck (forward);
		return 0;
	}

	/// <summary>
	/// Turns the deck.
	/// </summary>
	/// <param name="forward">If set to <c>true</c> forward.</param>
	void IViewBaseCommands.TurnDeck(bool forward){
		//cardsDeck.TurnDeck (forward);
	}

	#endregion

}
