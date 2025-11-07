using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CardItem))]
public class CardTouchHandler : MonoBehaviour, 
								IPointerDownHandler, 
								IPointerUpHandler, 
								IPointerClickHandler, 
								IBeginDragHandler, 
								IDragHandler, 
								IEndDragHandler 
{
	private const bool PRINT_DEBUG = false;

	private bool draggable = true;
	// find view
	private CardItem targetCard;
	private ICardItemActions view;

	public void Init(CardItem ownerCard, ICardItemActions listener){
		
 
		targetCard = ownerCard;
		view = listener;
	}
	
// 
//  *********************************
//  *******  TOUCH  HANDLERS  ******* 
//  *********************************
// 

    private void AnimationScale(CardItem card)
    {
       

        card.AnimationScale() ;
      

        
    }
	bool pressedIn = false;
	#region IPointerDownHandler and IPointerUpHandler implementation for onClick
	void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
        
        if (targetCard.Hide)
        {
          
            AnimationScale(targetCard);
            return;
        }
        view.pressByCard (targetCard);
		pressedIn = true;
        if (GameSettings.Instance.isSoundSet)
        {

            Sound.Instance.TouchCard();
        }
    }
	void IPointerUpHandler.OnPointerUp (PointerEventData eventData)	{
		if(pressedIn){
			pressedIn = false;

            if (targetCard.Hide) return;
            view.clickByCard (targetCard);
		}

	}
	void IPointerClickHandler.OnPointerClick (PointerEventData eventData) {}
	#endregion

	#region IBeginDragHandler implementation
	void IBeginDragHandler.OnBeginDrag (PointerEventData eventData)
	{
		pressedIn = false;
		if (draggable) {
			log ("OnBeginDrag");


            if (targetCard.Hide ) return;

            view.startDragCard (targetCard);
		}
	}
	#endregion

	#region IDragHandler implementation
	void IDragHandler.OnDrag (PointerEventData eventData)
    {
        if (targetCard.Hide  ) return;
        if (draggable) {
			//log ("OnDrag");
			view.dragCard (eventData.position);
		}
	}
	#endregion

	#region IEndDragHandler implementation
	void IEndDragHandler.OnEndDrag (PointerEventData eventData)
    {
        if (targetCard.Hide  ) return;
        if (draggable) {
			log ("OnEndDrag");
			view.endDragCard (targetCard);
		}
	}
	#endregion


	private void log(string msg){
		if (!PRINT_DEBUG)
			return;

		string c = "card id: " + targetCard.Id + " ";
		Debug.Log(c + msg);
	}
}
