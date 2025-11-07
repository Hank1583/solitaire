namespace UserWindow
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;
    using System;
    using System.Collections;
    using TMPro;
    public class ResultWindowScreen : MonoBehaviour
    {
  
      
   

        [SerializeField]
        private TextMeshProUGUI  currentMovesText;
        [SerializeField]
        private TextMeshProUGUI bestMovesText;
        [SerializeField]
        private TextMeshProUGUI currentScoreText;
        [SerializeField]
        private TextMeshProUGUI bestScoreText;
        [SerializeField]
        private TextMeshProUGUI currentTimeText;
        [SerializeField]
        private TextMeshProUGUI bestTimeText;








        [SerializeField]
        private GameObject buttonPref;
        [SerializeField]
        private RectTransform buttonsRT;
    


        [SerializeField]
        private GameObject eff_HighScrore;
 

        private ResultTextLineData titleData;
        private List<ResultTextLineData> listData;
        private bool isVertical;
        private List<ResultButtonData> buttonData;
     

        private bool isTitle;
        private bool isList;
        private bool isWin;
 
        private List<GameObject> buttons = new List<GameObject>();
        public void Init(ResultTextLineData title, List<ResultTextLineData> list, bool isAwarded, List<ResultButtonData> buttons)
        {

          

            this.titleData = title;
            this.listData = list;
       
            this.buttonData = buttons;

            
            isTitle = (title == null) ? false : true;
            isList = (list == null || list.Count.Equals(0)) ? false : true;
            isWin = GameSettings.Instance.isGameWon;
          
           
          



        }
        public IEnumerator Build()
        {
      
         
        
            BuildTitleAndList();
            BuildButtons();





            if (StageManager.instance.GetBestTime() && isWin)
            {

                if (eff_HighScrore == null) yield break;
                yield return new WaitForSeconds(.5f);
                eff_HighScrore.SetActive(true);
                eff_HighScrore.GetComponent<ParticleSystem>().Play();

            }

            PlayerPrefAPI.Set();

        }

        private void Update()
        {
            if (isVertical != DeviceOrientationHandler.instance.isVertical)
            {
                if (!DeviceOrientationHandler.instance.isVertical)
                {
                    transform.localScale = Vector3.one * .7f;
                }
                else
                {
                    transform.localScale = Vector3.one;
                }
                isVertical = DeviceOrientationHandler.instance.isVertical;
            }
         
        }

        
        private void BuildTitleAndList()
        {
            if (isTitle)
            {
               
            }
            if (isList)
            {
                int step = 0;




                currentScoreText.text = listData[0].Text1;
                bestScoreText.text = listData[0].Text2;

                currentTimeText.text = listData[1].Text1;
                bestTimeText.text = listData[1].Text2;

                currentMovesText.text = listData[2].Text1;
                bestMovesText.text = listData[2].Text2;

           
            }
        }










        private void BuildButtons()
        {
            if (buttons.Count > 0) return;
            foreach (ResultButtonData element in buttonData)
            {
              
                GameObject buttonObj = (GameObject)Instantiate(buttonPref);

                buttonObj.transform.SetParent(buttonsRT);
                buttonObj.transform.localScale = Vector3.one;
                buttons.Add(buttonObj);

                ResultButtonIniter buttonIniter = buttonObj.GetComponent<ResultButtonIniter>();

                buttonIniter.Init(element);
                buttonIniter.Show();


            }
        }

    }
}
