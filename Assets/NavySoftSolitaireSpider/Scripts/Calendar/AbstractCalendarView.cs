using UnityEngine;
public abstract class AbstractCalendarView : MonoBehaviour//, ICalendarView
{
    public abstract void ShowMainPanel(bool isCalendar);
    public abstract void InitView();
    public abstract void HideAllCells();

    public abstract void CellObjSetActive(int index, bool isActive);

    public abstract void CellViewHideComponents(int index);
    public abstract void CellViewSetDate(int index, int data);
    public abstract void CellViewSetActiveCell(int index, bool isActive);



    public abstract void CellViewShowGoldMedal(int index);

    public abstract void CellViewAnimateGoldMedal(int index);
    public abstract void CellViewShowHalo(int index, bool visible);

    public abstract bool CellViewIsActiveCell(int index);
    public abstract int CellViewGetDate(int index);

    public abstract void ShowMonthYear(string info);
    public abstract void ShowArrows(bool isPrev, bool isNext);

    public abstract void ShowRankingPanel(string name, bool isMonthRanking, UnityEngine.Sprite medalSpr, string rankText, string progressText);



    public abstract void InitTournamentManager(System.Action OnChalengeBack, System.Action OnChalengePlay, bool hasMedal);
    public abstract void InitTournamentRecord();
}