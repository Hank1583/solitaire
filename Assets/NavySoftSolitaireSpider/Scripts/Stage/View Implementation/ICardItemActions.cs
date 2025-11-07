
public interface ICardItemActions {

	void pressByCard (CardItem targetCard);

	void clickByCard (CardItem clicked);

	void startDragCard (CardItem clicked);

	void dragCard (UnityEngine.Vector3 position);

	void endDragCard (CardItem clicked);
}
