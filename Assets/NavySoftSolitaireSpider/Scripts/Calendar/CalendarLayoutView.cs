namespace Calendar
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;
    using System.Collections;
    using TMPro;
    public class CalendarLayoutView : AbstractCalendarView
    {

        [SerializeField]
        private GameObject[] cellObj;
        private List<CellView> cellView;

        [SerializeField]
        GameObject calendarObj;
        [SerializeField]
        GameObject tournamentObj;

        [SerializeField]
        private TextMeshProUGUI infoMonthYearText;

        [SerializeField]
        private GameObject arrowPrev;
        [SerializeField]
        private GameObject arrowNext;




        [SerializeField]
        private Text rankProgressText;
        [SerializeField]
        private Text[] rankLastPoint;
        [SerializeField]
        private Image[] rankFills;




        [SerializeField]
        private RectTransform rectWidthFill;
        [SerializeField]
        private RectTransform positionBeginMedal;
        [SerializeField]
        private RectTransform[] rectMedals;



        [SerializeField]
        TournamentManager tournamentManager;

        #region implemented abstract members of AbstractCalendarView
        public override void ShowMainPanel(bool isCalendar)
        {
            calendarObj.SetActive(isCalendar);
        }

        public override void InitView()
        {

            int cellLength = cellObj.Length;
            cellView = new List<CellView>();
            for (int cellIndex = 0; cellIndex < cellLength; cellIndex++)
            {
                cellView.Add(cellObj[cellIndex].GetComponent<CellView>());
                cellView[cellIndex].Awake1();
            }





        }

        private void FixedUpdate()
        {
            float[] fill = new float[3] { 0.3f, 0.6f, 1f };

            float max = Mathf.Max(Screen.height, Screen.width);
            float min = Mathf.Min(Screen.height, Screen.width);
            float ratioResolution = max / min;
            float width = min;


            width = rectWidthFill.sizeDelta.x;



            for (int i = 0; i < rectMedals.Length; i++)
            {
                int addWidth = 15;
                if (i == rectMedals.Length - 1) addWidth = 0;

                rectMedals[i].localPosition = new Vector3((fill[i] * width) + positionBeginMedal.localPosition.x + addWidth, -30, 0);

            }

        }



        public override void HideAllCells()
        {
            for (int i = 0; i < cellObj.Length; i++)
            {
                CellObjSetActive(i, false);
            }

        }

        public override void CellObjSetActive(int index, bool isActive)
        {
            float alpha = (isActive) ? 1 : 0;
            CanvasGroup canvas = cellObj[index].GetComponent<CanvasGroup>();
            canvas.alpha = alpha;
            canvas.interactable = isActive;
            canvas.blocksRaycasts = isActive;

        }

        public override void CellViewHideComponents(int index)
        {
            cellView[index].HideComponents();
        }
        public override void CellViewSetDate(int index, int data)
        {
            cellView[index].SetDate(data);
        }
        public override void CellViewSetActiveCell(int index, bool isActive)
        {
            cellView[index].SetActiveCell(isActive);
        }



        public override void CellViewShowGoldMedal(int index)
        {
            cellView[index].ShowGoldMedal();
        }


        public override void CellViewAnimateGoldMedal(int index)
        {
            cellView[index].AnimateGoldMedal();
        }
        public override void CellViewShowHalo(int index, bool visible)
        {
            cellView[index].ShowHalo(visible);
        }
        public override bool CellViewIsActiveCell(int index)
        {
            return cellView[index].IsActiveCell;
        }
        public override int CellViewGetDate(int index)
        {
            return cellView[index].GetDate();
        }

        public override void ShowMonthYear(string info)
        {
            infoMonthYearText.text = info;
        }
        public override void ShowArrows(bool isPrev, bool isNext)
        {
            CanvasGroup prevCanvasGroup = arrowPrev.GetComponent<CanvasGroup>();
            prevCanvasGroup.alpha = (isPrev) ? 1 : 0;
            prevCanvasGroup.blocksRaycasts = isPrev;
            prevCanvasGroup.interactable = isPrev;

            CanvasGroup nextCanvasGroup = arrowNext.GetComponent<CanvasGroup>();
            nextCanvasGroup.alpha = (isNext) ? 1 : 0;
            nextCanvasGroup.blocksRaycasts = isNext;
            nextCanvasGroup.interactable = isNext;


        }

        public override void ShowRankingPanel(string name, bool isMonthRanking, UnityEngine.Sprite medalSpr, string rankText, string progressText)
        {



            if (name == string.Empty) name = SystemInfo.deviceName;







            rankProgressText.text = progressText;
            string[] datas = progressText.Split('/');
            float current = int.Parse(datas[0].ToString());
            float total = int.Parse(datas[1].ToString());

            for (int i = 0; i < rankLastPoint.Length; i++)
            {

                //  rankLastPoint[i].text = total.ToString();

                rankFills[i].fillAmount = current / total;
            }


        }



        public override void InitTournamentManager(System.Action OnChalengeBack, System.Action OnChalengePlay, bool hasMedal)
        {
            tournamentManager.Init(OnChalengeBack, OnChalengePlay, hasMedal);
        }

        public override void InitTournamentRecord()
        {
            tournamentManager.InitRecord();
        }
        #endregion
    }
}