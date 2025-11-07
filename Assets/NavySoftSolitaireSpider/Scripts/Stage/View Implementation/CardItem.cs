using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
public class CardItem : MonoBehaviour {
   private const int limitCardLandSpace = 12;
    private const  int limitCardPortrait = 18;

    // 
    //  *********************************
    //  ***********  FIELDS  ************
    //  *********************************
    // 
    public bool isRoot = false;
 
    public bool isTableu = false;
   
    public bool isFoundation = false;
	public bool isDeck = false;
	public int debugZoneIndex = 0;

	[SerializeField]
	private Image faceImage;
	[SerializeField]
	private Image backImage;
	[SerializeField]
	private SimpleEffectAbstract highlightEffect;
	[SerializeField]
	private SimpleEffectAbstract blinkEffect;
    [SerializeField]
    private SaluteAnimator salute;
    [SerializeField]
	private Vector2 offsetForAllChilds;
	[SerializeField]
	private Vector2 offsetForAllChildsOpened;

    private BetweenDistanceCard betweenDistance;
    private CardMove cardMove;
    [Header("Debug")]
	public CardItem parentCard;	
 
	public CardItem _childCard;
    
    public CardItem rootCard;

    [SerializeField]
    private List<bool> visibleCard = new List<bool>();
	public int Id;
	public bool isOppened;
 
    public int Suit;
	public int Rank;
    public bool Hide;

    public bool scrollCard;
    public bool MoveComplete;
    [HideInInspector]
    public bool skipAddVisibleCard = false;
  
    public int slot = -1;
    private bool prevOpen = false;
    private bool fisrtUseAnimation = true;
    private SolitaireEngine.Model.Card card;
    private float originScale = 1;
    private float rangeBetweenCard;
    private bool isSetup = false;
    [SerializeField]
    private bool highLightMode = false;
    public bool GetSetUpStartGame { get { return isSetup; } }
    public CardMove GetCardMove
    {
        get
        {
            if (cardMove == null) cardMove = GetComponent<CardMove>();
            return cardMove;
        }
    }
    public BetweenDistanceCard GetBetweenDistance
    {
        get
        {
            if (betweenDistance == null) betweenDistance = GetComponent<BetweenDistanceCard>();
            return betweenDistance;
        }
    }
    public SolitaireEngine.Model.Card GetCard
    {
        get
        {
            return card;
        }
    }
    private RectTransform _rect;

	public bool clickable{
		get{ 
			return isOppened;
		}
	}


    public float RangeCard
    {
        get
        {
            return rangeBetweenCard;
        }
    }
 

// 
//	*********************************
//	*******  INITIALISATION  ********
//	*********************************
//
 

	public void initCard(int id, bool isOpen, int suit, int rank)//(Card card)
	{
		Id = id;
		isOppened = isOpen;
		Suit = suit;
		Rank = rank;
     
        //		cardModel = card;
        if (!isRoot) {
			backImage.sprite = ImageSettings.Instance.cardbackHiResolurion[GameSettings.Instance.visualCardBacksSet];
		}
        prevOpen = !isOpen;
        
         _rect = gameObject.GetComponent<RectTransform>();
		 gameObject.name = "Card id: " + Id;
        CardImage cardImage = GetComponent<CardImage>();
        if (cardImage != null)
            cardImage.SetCard(suit, rank - 1);
        Vector3 localScale = SolitaireStageViewHelperClass.instance.ConvertSizeCard(DeviceOrientationHandler.instance.isVertical);
        _rect.transform.localScale = localScale;
        isSetup = true;

    }


    public void AnimationScale()
    {

        Sound.Instance.MissCard();
        CardItem card =   SolitaireStageViewHelperClass.instance.FindTableu(this);
        if (DeviceOrientationHandler.instance.isVertical)
        {
            if (LengthInGroup() < limitCardPortrait)
            {
                SolitaireStageViewHelperClass.instance.ShakeCard(this);
                return;
            }
        }
        else
        {
            if (LengthInGroup() < limitCardLandSpace)
            {
                SolitaireStageViewHelperClass.instance.ShakeCard(this);
                return;
            }
        }

        card.GetComponent<BetweenDistanceCard>().SetDistanceNormal(scrollCard);
        scrollCard = !scrollCard;
    }

    public int LengthInGroup()
    {
        int length = 1;
        CardItem parent = parentCard;
        while (parent != null)
        {
            length++;
            parent = parent.parentCard;
        }
        CardItem child = childCard;
        while (child != null)
        {
            length++;
            child = child.childCard;
        }

        return length;

    }


    public int LengthConnectCardInGroup()
    {
        int length = 1;
        CardItem mainCard = this;
        CardItem parent = parentCard;
        while (parent != null && parent.Rank -mainCard.Rank==1 )
        {
            length++;
            mainCard = parent;
            parent = parent.parentCard;
        }

        return length;
    }
    public void Card(SolitaireEngine.Model.Card card)
    {
        this.card = card;
    }
 




    public void VisibleCard( bool visible)
    {

        float color = 1;
        highLightMode = visible;
        if (visible == true && GameSettings.Instance.isHighLightMode)
        {
            color = 0.7f;
           
        }
       
        if (faceImage == null) return;



        faceImage.transform.GetChild(0).GetComponent<Image>().color = new Color(color, color, color, 1);

      
       if(!MoveComplete)
        Hide = visible;

 
    }
    public void ActiveHighLightCard(bool visible)
    {
        float color = 1;
        if (visible && Hide)
        {
            color = 0.7f;
        }

        faceImage.transform.GetChild(0).GetComponent<Image>().color = new Color(color, color, color, 1);
    }
    private CardTouchHandler touchHandler; 
    private CardTouchHandler GetCardTouchHandler
    {
        get
        {
            if (touchHandler == null)
            {
                touchHandler = GetComponent<CardTouchHandler>();

            }
            return touchHandler;
        }
    }
    public void attachListener(ICardItemActions listener){
       
        if (GetCardTouchHandler == null)
        {

             touchHandler = gameObject.AddComponent<CardTouchHandler>();
           
        }
        GetCardTouchHandler.Init(this, listener);
    }

	public void UpdateCardBack (){
#if UNITY_EDITOR
        if (isRoot) {
			throw new UnityException ("Root card hasn't back image");
		}
#endif

        backImage.sprite = ImageSettings.Instance.cardbackHiResolurion[GameSettings.Instance.visualCardBacksSet];
	}


// 
//	*********************************
//	*********  PROPERTIES  **********
//	*********************************
// 

	public bool allowClick{
		get{ 
			return faceImage.raycastTarget || backImage.raycastTarget;
		}
		set{ 
			faceImage.raycastTarget = backImage.raycastTarget = value;
		}
	}

 
	public RectTransform rect {
		get {
			return _rect;
		}
	}

	public bool hasChildCard {
		get {
			return _childCard != null;
		}
	}

	public CardItem childCard{
		get{
			return _childCard;
		}
	}
	public CardItem []  getChildCardsList(){
        CardItem[] childCards;
		if(!hasChildCard){
			return new CardItem[0];
		}else{

            childCards = GetComponentsInChildren<CardItem>();
			return childCards;
		}
	}
    public CardItem getChildestCard()
    {
        if (!hasChildCard)
        {
            return this;
        }
        else
        {
            CardItem[] childs = GetComponentsInChildren<CardItem>();

            CardItem c = childs[childs.Length - 1];

            return c;
        }
    }

    public float OffSetChildCard(int length ,bool close)
    {
        float range = 0;

        int portrait = limitCardPortrait;
        int landSpace = limitCardLandSpace;
    
       
        
        if (SolitaireStageViewHelperClass.instance.RatioResolution() <= 1.5)
        {
            portrait = landSpace;
        }
        if (DeviceOrientationHandler.instance.isVertical && length< portrait)
        {

            if (close)
            {
      
                return SolitaireStageViewHelperClass.rangeBetweenCloseCard;
            }
            else
            {

                return SolitaireStageViewHelperClass.rangeBetweenOpenCard;
            }
        }

      else  if (length < landSpace)
        {
       
            if (close)
            {
                return SolitaireStageViewHelperClass.rangeBetweenCloseCard;
            }
            else
            {

                return SolitaireStageViewHelperClass.rangeBetweenOpenCard;
            }

        }

        float ratio = (DeviceOrientationHandler.instance.isVertical) ? 50 : 9;
 
        if (close)
        {
        
            range = (SolitaireStageViewHelperClass.rangeBetweenCloseCard * ratio) / (length * 3);
          
            range = (Mathf.Abs(range) > Mathf.Abs(SolitaireStageViewHelperClass.rangeBetweenCloseCard)) ? (float)SolitaireStageViewHelperClass.rangeBetweenCloseCard : range;
        
        }
        else
        {

            range = ((SolitaireStageViewHelperClass.rangeBetweenOpenCard * ratio * 1.1f) / (length * ((!highLightMode) ? .5f : 10)));
           
            range = (Mathf.Abs(range) > Mathf.Abs(SolitaireStageViewHelperClass.rangeBetweenOpenCard)) ? (float)SolitaireStageViewHelperClass.rangeBetweenOpenCard : range;
 

        }
 
        return range;
    }
  
    public void childCardNull () {
		_childCard = null;
	}

	public CardItem getParentCard(){
		Transform t = gameObject.transform.parent;
		CardItem c = t.GetComponentInParent<CardItem>();
		return c;
	}

	public CardItem getRootCard (){
		CardItem c = (rootCard==null)?this:rootCard;
		 
		return c;
	}
	public Vector2 childOffset{
		set{ 
			offsetForAllChilds = value;
		}
		get{ 
			return offsetForAllChilds;
		}
	}
	public Vector2 childOffsetOpened{
		set{ 
			offsetForAllChildsOpened = value;
		}
		get{ 
			return offsetForAllChildsOpened;
		}
	}




    // 
    //	*********************************
    //	***********  ACTIONS  *********** 
    //	*********************************
    // 
    public void showSalute()
    {

        Assert.IsFalse(Suit < 0 || Suit > 3);
        //        Debug.Log("Show 2");
        salute.Show(Suit);
    }


    public void SetOpen(bool isOpen)
    {
        isOppened = isOpen;

 
    }
    public void openCard(bool isOpen)
    {


        if (turn) return;


        isOppened = isOpen;


        if (faceImage != null)
        {
      
            faceImage.gameObject.SetActive(isOppened);
        }
        if (backImage != null)
        {
        
            backImage.gameObject.SetActive(!isOppened);
        }
    }

	public void openCardAnim(bool isOpen = true){
 
        if (!ContinueModeGame.instance.LoadSuccess)
        {
            openCard(true);
        }
        else
        {
          
            turn = true;
            turnOpen = isOpen;
         
        }
        
	}

	public bool turn = false;
	bool turnOpen = false;
	private float speed = 8f;
	private float time = 1f;
	// ANIMATION UPDATE
	private void Update()
	{

        if (!turn)
			return;
		
		// increase timer
		time -= Time.deltaTime * speed;

		// first part of animation
		if (time > 0f) {
			SetScale (time);
		} 
		// second part of animation
		else {
         
            isOppened = true;

           
            if (faceImage != null)
                faceImage.gameObject.SetActive(isOppened);
            if (backImage != null)
                backImage.gameObject.SetActive(!isOppened);

            // shift time for 1 second back 
            float new_time01 = time + 1f;
			// change time from 0-1 to 1-0
			float inverted_time = 1f - new_time01;
			// fade in
			SetScale (inverted_time);
			// finish
			if (time < -1f) {
				time = 1f;
                

                turn = false;
				SetScale (1f);
                
            }
		}

	}


	public IEnumerator DrawBezierPath(Vector3 a, Vector3 b, Vector3 c )
	{
	 
	 
        float t = 0;
		while (t <= 1)
		{

            t += Time.deltaTime;
         
			yield return new WaitForSeconds(.02f);
			transform.position = CalculateBezierPoint(t,a,b,c,c);
		}
    
        transform.position = c;
    }

    public Vector3 CalculateBezierPoint(float t,
          Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1.0f - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        var p = uuu * p0; //first term
        p += 3 * uu * t * p1; //second term
        p += 3 * u * tt * p2; //third term
        p += ttt * p3; //fourth term

        return p;

    }

    public Vector2 CalculateBezierPoint(float t, Vector2 a, Vector2 b, Vector2 c)
    {
        var ab = Vector2.Lerp(a, b, t);
        var bc = Vector2.Lerp(b, c, t);
 
  
        return Vector2.Lerp(ab, bc, t);
    }

    private void SetScale(float value)
	{
        if(faceImage!=null)
		faceImage.transform.localScale = new Vector3 (value, 1f);
        if (backImage != null)
            backImage.transform.localScale = new Vector3 (value, 1f);
	}
}