using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlayGame : MonoBehaviour
{
    public static AutoPlayGame instance;


    public List<int> hints = new List<int>();

    public List<CardItem> AutoPlay()
    {
     
        CardItem[] cards = MonoHandler.FindObjectsOfType<CardItem>();

        hints.Clear();




     



        for (int h = 0; h < cards.Length; h++)
        {
            if (cards[h].Hide) continue;
            if (!cards[h].isOppened) continue;
            if (cards[h].isRoot) continue;

            bool addHint = false;
            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i].Id == cards[h].Id)
                {
                    if (cards[i].parentCard != null)
                    {
                        if (cards[i].parentCard.Rank - cards[i].Rank == 1 && cards[i].parentCard.isOppened)
                        {
                            addHint = true;
                            break;
                        }
                    }
                }


 
            }


            if (!addHint)
            {
                for (int i = 0; i < cards.Length; i++)
                {
                    if (cards[i].isOppened && cards[i].childCard == null && !cards[i].Hide)
                    {
                        if (cards[i].Rank - cards[h].Rank == 1)
                        {

                            hints.Add(cards[h].Id);
                        }
                    }
                }


               
            }
        }

        for (int i = 0; i < hints.Count ; i++)
        {
            for (int j = i+1; j < hints.Count; j++)
            {
                if (hints[i] == hints[j])
                {
                    hints.RemoveAt(j);
                    j--;
                }
            }
        }

        List<CardItem> cardItems = new List<CardItem>();

        for (int j = 0; j < hints.Count; j++)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                if (hints[j] == cards[i].Id)
                {
                      
                    cardItems.Add(cards[i]);

                }
            }
        }

        return cardItems;
    }


    private void Start()
    {
        instance = this;
    }
}
