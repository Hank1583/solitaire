public interface IManagerBaseCommands
{
	void Move (int idFrom, int idTo );
	void TurnCard (int id);
	void ShiftDeck ();
	void UndoShiftDeck ();
	void ReverseDeck (bool isOpenCard);
	void Score (int score);
	void SetThronCardMove (int id, bool isMoved);
	void SetCommunityCardMove (int id, bool isMoved);
}