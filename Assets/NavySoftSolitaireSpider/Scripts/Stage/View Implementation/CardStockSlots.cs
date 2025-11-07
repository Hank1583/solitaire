using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStockSlots : MonoBehaviour
{

    public static CardStockSlots instance;

    [SerializeField]
    private CardItem  cardItem;


    private CardItem[] cards;

    public bool undo = false;


 

 

    private List<Vector3> placeCard = new List<Vector3>();
    
    private void Start()
    {
        instance = this;
    }

    public void  Initialized()
    {
      StartCoroutine(GetCardItem());
    }

    public void ConvertPosition()
    {
        float dst = 20;
        cards = GetComponentsInChildren<CardItem>();
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] == null) continue;
            if (cards[i].slot == -1) continue;

            if (!DeviceOrientationHandler.instance.isVertical)
            {
                cards[i].transform.position = new Vector3(transform.position.x, transform.position.y + cards[i].slot * dst, transform.position.z);
            }
            else
            {
                if (GameSettings.Instance.isHandSet)
                {
                    cards[i].transform.position = new Vector3(transform.position.x + cards[i].slot * dst, transform.position.y, transform.position.z);
                }
                else
                {
                    cards[i].transform.position = new Vector3(transform.position.x - cards[i].slot * dst, transform.position.y, transform.position.z);
                }
                
            }
        }
    }

    private void Update()
    {
        if(isVertical!= DeviceOrientationHandler.instance.isVertical)
        {
            ConvertPosition();
            isVertical = DeviceOrientationHandler.instance.isVertical;
        }
    }


    private bool isVertical = false;
    private IEnumerator GetCardItem()
    {
        yield return new WaitForSeconds(1.5f);
        /*
        Debug.Log("ID " +cardItem.Id);
        cardItem = transform.GetChild(1).GetComponent<CardItem>();
        CardItem card = cardItem;
    
        CardItem childCard = cardItem.childCard;
        int slot = 0;
        int count = 0; 
        while (childCard!=null)
        {
            card.slot = slot;
            card = childCard;
           
            card.slot = slot;
            childCard = card.childCard;
            count++;
            if (count >= 10)
            {
                slot++;
                if (slot > 4) slot = -1;
                count = 0;
            }

        }
        isVertical = DeviceOrientationHandler.instance.isVertical;
        ConvertPosition();
        */

    }
}
