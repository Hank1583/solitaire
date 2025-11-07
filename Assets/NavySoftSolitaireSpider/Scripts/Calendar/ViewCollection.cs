namespace Calendar
{
	using UnityEngine;
	
	public class ViewCollection : AbstractCalendarView
	{
		[SerializeField]
		private AbstractCalendarView[] collection;

		#region implemented abstract members of AbstractCalendarView
		public override void ShowMainPanel (bool isCalendar)
		{
			foreach (AbstractCalendarView element in collection)
				element.ShowMainPanel (isCalendar);
		}
		public override void InitView()
		{
			foreach (AbstractCalendarView element in collection)
				element.InitView ();
		}

		public override void HideAllCells()
		{
			foreach (AbstractCalendarView element in collection)
				element.HideAllCells ();
		}

		public override void CellObjSetActive (int index, bool isActive)
		{
			foreach (AbstractCalendarView element in collection)
				element.CellObjSetActive (index, isActive);
		}
			
		public override void CellViewHideComponents (int index)
		{
			foreach (AbstractCalendarView element in collection)
				element.CellViewHideComponents (index);
		}
		public override void CellViewSetDate (int index, int data)
		{
			foreach (AbstractCalendarView element in collection)
				element.CellViewSetDate (index, data);
		}
		public override void CellViewSetActiveCell (int index, bool isActive)
		{
			foreach (AbstractCalendarView element in collection)
				element.CellViewSetActiveCell (index, isActive);
		}
	 
	 
		public override void CellViewShowGoldMedal (int index)
		{
			foreach (AbstractCalendarView element in collection)
				element.CellViewShowGoldMedal (index);
		}
			
	 
		public override void CellViewAnimateGoldMedal (int index)
		{
			foreach (AbstractCalendarView element in collection)
				element.CellViewAnimateGoldMedal (index);
		}
		public override void CellViewShowHalo (int index,bool visible)
		{
			foreach (AbstractCalendarView element in collection)
				element.CellViewShowHalo (index, visible);
		}
		public override bool CellViewIsActiveCell (int index)
		{
			foreach (AbstractCalendarView element in collection)
				return element.CellViewIsActiveCell (index); // TODO: one view result
            return false;
        }
		public override int CellViewGetDate (int index)
		{
			foreach (AbstractCalendarView element in collection)
				return element.CellViewGetDate (index); // TODO: one view result
            return 0;
        }
		public override void ShowMonthYear(string info)
		{
			foreach (AbstractCalendarView element in collection)
				element.ShowMonthYear (info);
		}
		public override void ShowArrows (bool isPrev, bool isNext)
		{
			foreach (AbstractCalendarView element in collection)
				element.ShowArrows (isPrev, isNext);
		}

		public override void ShowRankingPanel (string name, bool isMonthRanking, UnityEngine.Sprite medalSpr, string rankText, string progressText)
		{
			foreach (AbstractCalendarView element in collection)
				element.ShowRankingPanel (name, isMonthRanking, medalSpr, rankText, progressText);
		}

	 

		public override void InitTournamentManager(System.Action OnChalengeBack, System.Action OnChalengePlay, bool hasMedal)
		{
			foreach (AbstractCalendarView element in collection)
				element.InitTournamentManager(OnChalengeBack, OnChalengePlay, hasMedal);
		}

		public override void InitTournamentRecord()
		{
			foreach (AbstractCalendarView element in collection)
				element.InitTournamentRecord();
		}

		#endregion
	}
}