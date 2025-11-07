using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugGame : MonoBehaviour {
	
	IDebugGame game1;
	IDebugGame game2;

	public static DebugGame instance;
	void Awake(){
		instance = this;
	}

	float t = 0;
	public void Update(){
		if (!init)
			return;
		t += Time.deltaTime;
		if (t > 1) {
			t = 0;
//			Start1 ();
			StartUnitTests ();
		}
	}

	bool init = false;
	public void StartTesting(IDebugGame view, IDebugGame logic)
	{
		game1 = view;
		game2 = logic;

		// we wait few seconds when cards are dealing
		StartCoroutine (StartAfter(2f));

	}


	private IEnumerator StartAfter(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		init = true;
		//Debug.Log ("DEBUG: started");
	}

	public void StopTesting(){
		if (!init)
			return;
		init = false;
//		Debug.Log ("DEBUG: stopped");
	}
		
	void StartUnitTests()
	{
		CountCardsTest ();

		int m_count = MachedCardsCount ();
		if (m_count < 52) {
			Debug.Log ("DEBUG: Mached Cards Test Error: matched count " + m_count);
		}
	}
		
	private bool CountCardsTest ()
	{
		int game1CardCount = game1.GetAllCards ().Count;
		int game2CardCount = game2.GetAllCards ().Count;
//		print ("DEBUG: Count of cards " + game1CardCount+ "  " + game2CardCount);
		bool isPassed = (game1CardCount.Equals (game2CardCount));
		if (!isPassed)
			throw new UnityException ("Error count cards View cards: " + game1CardCount + " Logic: " + game2CardCount);
		return isPassed;
	}
	private int MachedCardsCount()
	{
		List<IDebugCardInfo> cardsView = game1.GetAllCards ();
		List<IDebugCardInfo> cardsLogic = game2.GetAllCards ();

		int matched_cards_count = 0;

		foreach (var item in cardsView) {
			bool isFounded = false;

			foreach (var item2 in cardsLogic) {
				if (item.id == item2.id && IsCardsEqual(item, item2)) {
					isFounded = true;
				}
			}

			if (!isFounded) {
//				print ("View card id: " + item.id + " cant be found in logic!");
			} else {
				matched_cards_count++;
			}

		}


		return matched_cards_count;
	}


	private bool IsCardsEqual(IDebugCardInfo card1, IDebugCardInfo card2){
		bool isEqual = false;

		if (card1.id == card2.id &&

			card1.isOpen == card2.isOpen &&

			card1.rank == card2.rank &&

			card1.suit == card2.suit &&

			card1.zone == card2.zone &&

			card1.zoneIndex == card2.zoneIndex &&

			card1.cardIndexInStack == card2.cardIndexInStack ) 
		{
			isEqual = true;
		} else {
			Debug.Log ("Cards not equal: \nView:   " + card1.ToString() + "\nLogic: " + card2.ToString());
		}

		return isEqual;
	}
}