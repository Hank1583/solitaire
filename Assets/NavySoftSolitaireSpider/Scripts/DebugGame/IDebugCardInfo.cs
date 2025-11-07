using UnityEngine;
using System.Collections;

public class IDebugCardInfo {
	
	// Fields
	public int id;
	public bool isOpen;
	public Rank rank;
	public Suit suit;
	public Zone zone;
	public int zoneIndex;
	public int cardIndexInStack;



	// Enums
	public enum Rank{
		Ace = 1,
		Two = 2,
		Three,
		Four,
		Five,
		Six,
		Seven,
		Eight,
		Nine,
		Ten,
		Jack,
		Quen,
		King
	}

	public enum Suit{
		Diamonds = 0,
		Heart = 1,
		Clubs = 2,
		Spades = 3
	}

	public enum Zone{
		Deck,
		Foundation,
		Tableu
	}

	public override string ToString ()
	{
		return string.Format ("Card Id: {0} Open: {1} Rank: {2} Suit: {3} Zone: {4} ZoneIndex: {5} InStackIndex: {6}",
			id, isOpen, rank, suit, zone, zoneIndex, cardIndexInStack);
	}
}
