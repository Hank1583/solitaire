namespace UserWindow
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;
    using System;
    using TMPro;
    public class ScrollWindowScreen : MonoBehaviour
    {
       
        [SerializeField]
        RectTransform mainWindowRT;

        [SerializeField]
        private GameObject winLogoObj;




 
 
        [SerializeField]
        private TextMeshProUGUI dateText;


 


   





 


        [SerializeField]
        private GameObject linePref;
   

    
        [SerializeField]
        private GameObject buttonPref;
        [SerializeField]
        private RectTransform buttonsRT;





        private ResultTextLineData titleData;
        private List<ResultTextLineData> listData;
        private bool isAwarded;
        private List<ResultButtonData> buttonData;


        private bool isTitle;
        private bool isWin;
        private List<GameObject> buttons = new List<GameObject>();
        public void Init(ResultTextLineData title, List<ResultTextLineData> list, bool isAwarded, List<ResultButtonData> buttons)
        {
            this.titleData = title;
            this.listData = list;
            this.isAwarded = isAwarded;
            this.buttonData = buttons;

            isTitle = (title == null) ? false : true;
            isWin = GameSettings.Instance.isGameWon;
        }
        public void Build()
        {
            InitLogo();


            BuildCalendarPanel();
            BuildTitleAndList();
            BuildButtons();

   
        }
        private void InitLogo()
        {
            if (isWin) winLogoObj.SetActive(true);

        }
        private void BuildCalendarPanel()
        {
            string[] monthName = { "January", "Fabruary", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            string data = monthName[GameSettings.Instance.calendarGameMonth - 1].ToUpper() + " " + GameSettings.Instance.calendarGameDay.ToString();
            dateText.text = data;
            if (isWin)
            {
               
                SolitaireStageViewHelperClass.instance.SaveMedalCurrentMonth();

            }

        }
        private void BuildTitleAndList()
        {
            if (isTitle)
            {
      

                 
            }
            foreach (ResultTextLineData element in listData)
            {
               /*
                GameObject lineObj = (GameObject)Instantiate(linePref);
               
                ResultTextLineIniter lineIniter = lineObj.GetComponent<ResultTextLineIniter>();
                lineIniter.Init(element);
                lineIniter.Show();
                */
            }

        }
        private void BuildButtons()
        {
            if (buttons.Count > 0) return;
            foreach (ResultButtonData element in buttonData)
            {

                GameObject buttonObj = (GameObject)Instantiate(buttonPref);
                buttonObj.transform.SetParent(buttonsRT, true);
                buttonObj.transform.localScale = Vector3.one;
             
                buttons.Add(buttonObj);
                ResultButtonIniter buttonIniter = buttonObj.GetComponent<ResultButtonIniter>();
                buttonIniter.Init(element);
                buttonIniter.Show();
            }
        }

      
       
    }
}
