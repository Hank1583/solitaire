using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolitaireSpiderCheck : MonoHandler
{
    public static SolitaireSpiderCheck instance;

    [SerializeField]
    private int[] positionPlaceCardomplete;
    [SerializeField]
    private  List<CardItem> cards = new   List<CardItem>();

    [SerializeField]
    private List<CardItem> cardsPosition = new List<CardItem>();

 
    public int RowsComplete
    {
        get
        {

            int count = 0;
            for (int i = 0; i < SolitaireStageViewHelperClass.instance.GetFoundationStacks.Count; i++)
            {
                CardItem found = SolitaireStageViewHelperClass.instance.GetFoundationStacks[i];
                if (found.hasChildCard)
                {
                    count++;
                }
            }
            return count;
        }
    }



   
    public List<DataCardResumeGroup> DataCardCompleteGroup = new List<DataCardResumeGroup>();
    public List<DataCardResumeGroup> DataDealCardGroup = new List<DataCardResumeGroup>();
    public bool completeRowCard= false;
 
 

  
    public void RemoveLastGroup()
    {
        DataCardCompleteGroup.RemoveAt(DataCardCompleteGroup.Count - 1);
        string result = JsonHelper.ToJson(DataCardCompleteGroup.ToArray(), true);
        PlayerPrefAPI.SaveFoundCard(result);

    }

    public void RemoveDealCardGroup()
    {
        if (DataDealCardGroup.Count == 0) return;
        DataDealCardGroup.RemoveAt(DataDealCardGroup.Count - 1);
        string result = JsonHelper.ToJson(DataDealCardGroup.ToArray(), true);
        PlayerPrefAPI.SaveStockCard(result);

    }
    
    private void Awake()
    {
        instance = this;

        
    }
    
 

    public void CompleteSpider(CardItem item, bool addCardData =true )
    {

        cards.Clear();
      

        CardItem tableu = item ;
        CardItem[] cardsInTableu = tableu.GetComponentsInChildren<CardItem>();
     
        if (cardsInTableu.Length == 0 || cardsInTableu.Length<12) return;

        CardItem lastCard = cardsInTableu[cardsInTableu.Length - 1];
        cards.Add(lastCard);

        for (int i = cardsInTableu.Length - 2; i >= 0; i--)
        {
            if (cardsInTableu[i].MoveComplete) continue;
            if (!cardsInTableu[i].isOppened) continue;
            if (lastCard.Rank - cardsInTableu[i].Rank == -1 && lastCard.Suit == cardsInTableu[i].Suit)
            {
                lastCard = cardsInTableu[i];
                lastCard.MoveComplete = false;
                cards.Add(lastCard);
            }
            else
            {
                break;
            }
        }






        if (cards.Count >= 13)
        {

            if (addCardData)
            {
                ContinueModeGame.instance.AddDataCard(0, 0, cards[cards.Count - 1].parentCard.Id, cards[cards.Count - 1].parentCard.isOppened);

            }
            for (int i = 0; i < cards.Count; i++)
            {
               
                cards[i].MoveComplete = true;
                cards[i].Hide = true;
            }
            //Place card
            PlaceCard(cards);

        }
 
    }


    public void Undo()
    {
        
        
        StartCoroutine(ResetPlaceCard());
 

    }

    private IEnumerator ResetPlaceCard()
    {
        yield return new WaitForSeconds(0.5f);
        completeRowCard = false;
    }

   
    private void PlaceCard(List<CardItem> cardItems ,float waitTimeMoveCurve = 0.04f)
    {
       
        MoveBackRowCardCommands moveBackRow = new MoveBackRowCardCommands(true,true, waitTimeMoveCurve);
        DataCardCompleteGroup.Add(new DataCardResumeGroup());
    
        Sound.Instance.FoundRowSpider();
        cardsPosition.Clear();
        for (int j = 0; j < cardItems.Count; j++)
        {
            cardsPosition.Add(cardItems[j]);
            if (cardItems[j] != null)
                SolitaireStageViewHelperClass.instance.CompleteCardSpider(cardItems[j], positionPlaceCardomplete[RowsComplete], moveBackRow, true);
        }


        //set offset card to zero
        solitaireTimer.Schedule(this, 1.2f, () => {
            for (int j = 0; j < cardsPosition.Count; j++)
            {
                cardsPosition[j].rect.anchoredPosition = Vector2.zero;

            }
        });
    
    }

     

     public void VisibleClickedCard(CardItem [] tableuCards)
    {
        CardItem lastCard = tableuCards[tableuCards.Length - 1];
        lastCard.VisibleCard(false);
        lastCard.Hide = false;
        bool flag = true;
        for (int i = tableuCards.Length - 2; i >= 1; i--)
        {
            if (lastCard.Rank - tableuCards[i].Rank == -1 && lastCard.Suit == tableuCards[i].Suit && flag)
            {
                lastCard.Hide = false;
                tableuCards[i].Hide = false;
                lastCard = tableuCards[i];
                tableuCards[i].VisibleCard(false);
            }
            else
            {
                flag = false;
                tableuCards[i].Hide = true;
                tableuCards[i].VisibleCard(true);
            }
           
        }
        
    }
  
}
