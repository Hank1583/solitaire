using System.Collections.Generic;

public interface ICardActions
{
	void OnClickCard(CardItem card);
	void OnDropCard(int id, int parent_id, List<int> nearest_cards_id, CardItem cardItem);

    void OnClickDeck();
	void OnTurnDeck();

	void OnClickAnywhere();


    //Spider
    ICommand CompleteCardSpider(int id, int parent_id,int best_place_id,bool moveCurve);
}