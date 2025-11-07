using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetweenDistanceCard : MonoBehaviour
{


    private bool trigger = false;
    private int length = 0;
    public void CheckDistanceCard(bool resetLength)
    {
        StopAllCoroutines();
        if (resetLength)
        {
            length = 0;
        }
      StartCoroutine(CaculatorDistance());
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        CardItem[] cardItems = GetComponentsInChildren<CardItem>();
        length = cardItems.Length;
    }

    public void SetDistanceNormal(bool scroll)
    {
        if (scroll)
        {
            length = 0;
            StartCoroutine(CaculatorDistance());
        }
        else
        {
            CardItem[] cardItems = GetComponentsInChildren<CardItem>();
            for (int j = 2; j < cardItems.Length; j++)
            {
                if (cardItems[j].MoveComplete) continue;
                
                cardItems[j].scrollCard = true;
                Vector2 offSet = Vector2.zero;
                offSet.y = (cardItems[j].isOppened) ? SolitaireStageViewHelperClass.rangeBetweenOpenCard : SolitaireStageViewHelperClass.rangeBetweenCloseCard;
                offSet.y = (cardItems[j].parentCard !=null && cardItems[j].parentCard.isOppened) ? SolitaireStageViewHelperClass.rangeBetweenOpenCard : SolitaireStageViewHelperClass.rangeBetweenCloseCard;

         
                SolitaireStageViewHelperClass.instance.movementManager.Move(cardItems[j].Id, cardItems[j].transform, cardItems[j].parentCard.transform, offSet, false, () =>
                { });
 




            }
        }
    }



    private IEnumerator CaculatorDistance()
    {
        yield return new WaitForSeconds(.2f);
      
        CardItem[] cardItems = GetComponentsInChildren<CardItem>();
        //Debug.Log(string.Format("Length : {0}-{1}", length, cardItems.Length));
        if (length != cardItems.Length)
        {
            length = cardItems.Length;
            for (int j = 2; j < cardItems.Length; j++)
            {
                if (cardItems[j].MoveComplete) continue;

                Vector2 offSet = Vector2.zero;
                bool close = (!cardItems[j].isOppened  ) || (!cardItems[j].parentCard.isOppened);


                offSet.y = cardItems[j].OffSetChildCard(cardItems.Length, close);
 
                if (close)
                {
                    cardItems[j].childOffset = offSet;
                }
                else
                {
                    cardItems[j].childOffsetOpened = offSet;
                }
                if (!ContinueModeGame.instance.LoadSuccess)
                    cardItems[j].rect.localPosition = offSet;
                else
                {
                    //LeanTween.move(cardItems[j].rect, offSet, 0.1f);
                }

            
                cardItems[j].scrollCard = false;
            }
        }
    }
}

