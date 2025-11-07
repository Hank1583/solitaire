using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardItemsDeck : MonoBehaviour, IPointerDownHandler {

 

    [SerializeField]
    private int slotCard;
    public CardItem deckHiddenContainer;
    public CardItem deckOpenedContainer;
 
    public static CardItemsDeck instance;
	void Awake(){
		instance = this;
	}

	private ICardActions manager;
	void Start(){
		manager = StageManager.instance;

         
	}


    private void OnEnable()
    {
        StartCoroutine(ResetOffSet());
    }

    private IEnumerator ResetOffSet()
    {
        yield return new WaitForSeconds(.5f);
        CardItem[] cardItems = deckHiddenContainer.GetComponentsInChildren<CardItem>();
        for (int i = 1; i < cardItems.Length; i++)
        {
            cardItems[i].rect.anchoredPosition = Vector2.zero;
        }
    }

  

	public CardItem initDeck(int containerId){
		deckHiddenContainer.initCard (containerId, false, -1, -1);
       
		return deckHiddenContainer;
	}
	public CardItem initDeckFaceUp(int containerId){
		deckOpenedContainer.initCard (containerId, false, -1, -1);
		return deckOpenedContainer;
	}
 

    private void FixedUpdate()
    {
        
        transform.position= deckHiddenContainer.transform.position;
    }
   
 

 


	#region OnPointerDown handler
	public void OnPointerDown(PointerEventData eventData){
        SolitaireStageViewHelperClass.instance.DealCardInDeck(slotCard);
       
    }
	#endregion

	 
}
