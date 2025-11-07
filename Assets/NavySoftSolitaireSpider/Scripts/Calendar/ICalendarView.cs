public interface ICalendarView
{
	void ShowMainPanel (bool isCalendar);
	void InitView ();
	void HideAllCells ();

	void CellObjSetActive (int index, bool isActive);
	void CellViewHideComponents (int index);
	void CellViewSetDate (int index, int data);
	void CellViewSetActiveCell (int index, bool isActive);
	void CellViewShowDouble (int index);

	void CellViewShowSilverMedal (int index);
	void CellViewShowGoldMedal (int index);

	void CellViewAnimateSilverMedal (int index);
	void CellViewAnimateGoldMedal (int index);
	void CellViewShowHalo (int index);

	bool CellViewIsActiveCell (int index);
	int CellViewGetDate (int index);

	void ShowMonthYear(string info);
	void ShowArrows (bool isPrev, bool isNext);

	void ShowRankingPanel (string name, bool isMonthRanking, UnityEngine.Sprite medalSpr, string rankText, string progressText);

	void ShowRankNeed (bool isActive, string text);

	void InitTournamentManager(System.Action OnChalengeBack, System.Action OnChalengePlay);
}