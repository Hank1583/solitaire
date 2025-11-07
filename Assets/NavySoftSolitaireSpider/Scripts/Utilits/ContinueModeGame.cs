using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DataCardResumeGroup
{
    public List<DataCardResume> dataCardResumes = new List<DataCardResume>();
}
[System.Serializable]
public class DataCardResume
{
    public int id;
    public int bestPlaceId;
    public int parentID;
    public bool isOpen;
    public string GetData()
    {
        return string.Format("{0}+{1}+{2}+{3}*", id, bestPlaceId, parentID,isOpen==true?1:0) ;
    }

    public bool DealCard()
    {
        return id == -99 && bestPlaceId == -99;
    }

    public bool CompleteOneRow()
    {
        return id == 0 && bestPlaceId == 0;
    }
}
public class ContinueModeGame : MonoBehaviour
{
    public static ContinueModeGame instance;

  
    [SerializeField]
    private List<DataCardResume> dataCardSave = new List<DataCardResume>();

    [SerializeField]
    private List<DataCardResume> dataCardLoad = new List<DataCardResume>();


    [SerializeField]
    private bool loadSuccess = false;
    public bool LoadSuccess { get { return loadSuccess; } }
    private void Start()
    {
        instance = this;
    }
    public void SetLoadSuccess(bool value)
    {
        loadSuccess = value;
    }
    public void AddDataCard(int id, int best_place_id, int parent_id,bool isOpen)
    {
        
        DataCardResume dataCard = new DataCardResume();
        dataCard.id = id;
        dataCard.bestPlaceId = best_place_id;
        dataCard.parentID = parent_id;
        dataCard.isOpen = isOpen;
        dataCardSave.Add(dataCard);
        SaveDataTable();
    }


  

    public void ClearAllDataCard(bool resetGameData = true)
    {

        if (resetGameData)
        {
            GameSettings.Instance.calendarData = string.Empty;
            PlayerPrefAPI.Set();

        }
        dataCardSave.Clear();
        dataCardLoad.Clear();
        SaveTimeAndMove(0, 0,0);
        SaveDataTable();
        PlayerPrefAPI.SaveFoundCard(string.Empty);
        PlayerPrefAPI.SaveStockCard(string.Empty);
        PlayerPrefAPI.SaveDataStep(string.Empty);
        PlayerPrefAPI.SaveDataRemainCardInSlot(string.Empty);
        SolitaireSpiderCheck.instance.DataDealCardGroup.Clear();
        SolitaireSpiderCheck.instance.DataCardCompleteGroup.Clear();

    }
    public void RemoveDataCard()
    {
        if (dataCardSave.Count > 0)
        {
            dataCardSave.RemoveAt(dataCardSave.Count - 1);
        }
        SaveDataTable();
    }


    private void SaveDataTable()
    {

        string data = JsonHelper.ToJson<DataCardResume>(dataCardSave.ToArray(),true);
        StartCoroutine(SolitaireStageViewHelperClass.instance.GetRemainCardInSlots());
        PlayerPrefAPI.SaveDataStep(data);
    }

    public void SaveTimeAndMove(int move, int timer, int score)
    {
        if (!loadSuccess) return;
#if UNITY_EDITOR
        Debug.Log(string.Format("Save :{0}-{1}-{2} ", move, timer, score));
#endif
        PlayerPrefAPI.SaveMove(move);
        PlayerPrefAPI.SaveTimer(timer);
        PlayerPrefAPI.SaveScore(score);
    }

    public IEnumerator LoadResumeTable()
    {

        
     
        DataCardResume[] dataCards = (PlayerPrefAPI.LoadDataStep() == string.Empty) ? new DataCardResume[0]: JsonHelper.FromJson<DataCardResume>(PlayerPrefAPI.LoadDataStep());
       
        if (dataCards.Length == 0)
        {
            yield return new WaitForSeconds(1.5f);
            loadSuccess = true;
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(.02f);
        }


   
    

        for (int i = 0; i < dataCards.Length ; i++)
        {
            dataCardLoad.Add(dataCards[i]);
            dataCardSave.Add(dataCards[i]);
        }

        int countStockCard = 0;
        int countFoundCard = 0;
        for (int i = 0; i < dataCardLoad.Count; i++)
        {
            List<int> iDs = new List<int>();
            List<int> bestPlaceIDs = new List<int>();
            List<int> parentIDs= new List<int>();
            List<bool> openCards = new List<bool>();
            if (dataCardLoad[i].CompleteOneRow())
            {
               // Debug.Log("Found " + PlayerPrefAPI.LoadFoundCard());
                DataCardResumeGroup [] dataFounds= JsonHelper.FromJson<DataCardResumeGroup>(PlayerPrefAPI.LoadFoundCard());
                SolitaireSpiderCheck.instance.DataCardCompleteGroup.Add(new DataCardResumeGroup());
                for (int j = 0 ; j <dataFounds[countFoundCard].dataCardResumes.Count ; j++)
                {
                   
                    SolitaireSpiderCheck.instance.DataCardCompleteGroup[countFoundCard].dataCardResumes.Add(dataFounds[countFoundCard].dataCardResumes[j]);
                 //   Debug.Log(dataFounds[countFoundCard].dataCardResumes[j].GetData());
                    iDs.Add(dataFounds[countFoundCard].dataCardResumes[j].id);
                    bestPlaceIDs.Add(dataFounds[countFoundCard].dataCardResumes[j].bestPlaceId);
                    parentIDs.Add(dataFounds[countFoundCard].dataCardResumes[j].parentID);

                    CardItem cardA = SolitaireStageViewHelperClass.instance.FindCardItem(dataFounds[countFoundCard].dataCardResumes[j].id);
                    cardA.Hide = true;
                    cardA.MoveComplete = true;
                }
        

           
                openCards.Add(dataCardLoad[i].isOpen);
                TurnCardCommand turnCardCommand = new TurnCardCommand(StageManager.instance.GetViewBaseCommands, dataCardLoad[i].parentID, dataCardLoad[i].isOpen);
            
                StageManager.instance.ContinueGame(iDs, bestPlaceIDs, parentIDs, turnCardCommand, true, true);
                countFoundCard++;
            

            }
            else
            {
                if (!dataCardLoad[i].DealCard())
                {
                  //  Debug.Log("Data Add Normal : " + dataCardLoad[i].GetData());
                    iDs.Add(dataCardLoad[i].id);
                    bestPlaceIDs.Add(dataCardLoad[i].bestPlaceId);
                    parentIDs.Add(dataCardLoad[i].parentID);
                    openCards.Add(dataCardLoad[i].isOpen);
                    TurnCardCommand turnCardCommand = new TurnCardCommand(StageManager.instance.GetViewBaseCommands, dataCardLoad[i].parentID, dataCardLoad[i].isOpen);
                    StageManager.instance.ContinueGame(iDs, bestPlaceIDs, parentIDs, turnCardCommand, false);
                }
                else
                {
                    DataCardResumeGroup[] dataStocks = JsonHelper.FromJson<DataCardResumeGroup>(PlayerPrefAPI.LoadStockCard());
                //    Debug.Log("Add Deal card 1");
                    SolitaireSpiderCheck.instance.DataDealCardGroup.Add(new DataCardResumeGroup());



                  //  Debug.Log("countStockCard " + countStockCard);
                    for (int h = 0; h < dataStocks[countStockCard].dataCardResumes.Count; h++)
                    {
                        SolitaireSpiderCheck.instance.DataDealCardGroup[countStockCard].dataCardResumes.Add(dataStocks[countStockCard].dataCardResumes[h]);
                        DataCardResume dataCard = new DataCardResume();
                        dataCard.id = dataStocks[countStockCard].dataCardResumes[h].id;
                        dataCard.bestPlaceId = dataStocks[countStockCard].dataCardResumes[h].bestPlaceId;
                        dataCard.parentID = dataStocks[countStockCard].dataCardResumes[h].parentID;

                        iDs.Add(dataCard.id);
                        bestPlaceIDs.Add(dataCard.bestPlaceId);
                        parentIDs.Add(dataCard.parentID);
                      
                       // Debug.Log(string.Format("Found : ID :{0} - Best ID:{1} - ParentID :{2} ", dataCard.id, dataCard.bestPlaceId,dataCard.parentID));
                    }

                    StageManager.instance.ContinueGame(iDs, bestPlaceIDs, parentIDs, null, true);
                    countStockCard++;

                    SolitaireStageViewHelperClass.instance.DealCardInDeck(dataCardLoad[i].parentID, false, 0.01f);

                }
            }
        }
      
        yield return new WaitForSeconds(1.5f);
        SolitaireStageViewHelperClass.instance.SetAllDistanceBetweenCard(true);
        loadSuccess = true;
      
        int statsScore = PlayerPrefAPI.LoadScore();
        int statsMove = PlayerPrefAPI.LoadMove();
        int statsTimer = PlayerPrefAPI.LoadTime();
#if UNITY_EDITOR
        Debug.Log(string.Format("Load :{0}-{1}-{2} ", statsMove, statsTimer, statsScore));
#endif
        HUDController.instance.SetMove(statsMove);
        
        HUDController.instance.SetTime(statsTimer);
        StageManager.instance.AddStatusGame(statsScore, statsTimer, statsMove);
   
       

       
     
      
       
     
    }

   
}
