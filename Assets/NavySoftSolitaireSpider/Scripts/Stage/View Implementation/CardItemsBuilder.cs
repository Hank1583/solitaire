using UnityEngine;

/// <summary>
/// Card items builder provides class that can build new instances of the card by origin card.
/// </summary>
public class CardItemsBuilder {
	
	/// <summary>
	/// The card origin.
	/// </summary>
	private CardItem cardOrigin;

	/// <summary>
	/// The parent transform.
	/// </summary>
	private UnityEngine.Transform parentTransform;

	/// <summary>
	/// Initializes a new instance of the <see cref="CardItemsBuilder"/> class.
	/// </summary>
	/// <param name="originCard">Origin card.</param>
	/// <param name="parent">Parent.</param>
	public CardItemsBuilder(CardItem originCard, UnityEngine.Transform parent){
		cardOrigin = originCard;
		parentTransform = parent;
	}
	

	/// <summary>
	/// Template method that builds the new item card.
	/// </summary>
	/// <returns>The new item card.</returns>
	/// <param name="cardModel">Card model with all data.</param>
	public CardItem BuildNewCardItem (int id, bool isOpen, int suit, int rank) {
		CardItem item = createNewInstanceOf (cardOrigin);
		setParent(item, parentTransform);
		//item.initCard (id, isOpen, suit, rank);
		return item;
	}


	/// <summary>
	/// Creates the new instance of card.
	/// </summary>
	/// <returns>The new instance of CardItem object.</returns>
	/// <param name="origin">Origin Card GameObject.</param>
	private CardItem createNewInstanceOf(CardItem origin){
#if UNITY_EDITOR
        if (origin == null) {
			throw new UnityException ("Can't create card instance from null");
		}
#endif
        return (CardItem) MonoBehaviour.Instantiate<CardItem>(origin);
	}

	/// <summary>
	/// Sets the new parent.
	/// </summary>
	/// <param name="origin">Origin.</param>
	/// <param name="parent">Parent.</param>
	private void setParent(CardItem cardItem, Transform parent){
#if UNITY_EDITOR
        if (cardItem == null || parent == null) {
			throw new UnityException ("Can't set new parent to card");
		}
#endif
        cardItem.transform.SetParent(parent, false);	
	}
}
