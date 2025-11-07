using System.Collections.Generic;

using SmartOrientation;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class SolitaireStageViewHelperClass : MonoHandler, ICardItemActions, IDebugGame
{
    // responsabilities:
    // - drag n drop cards
    // - manage movements
    // - controll animations 


    public static SolitaireStageViewHelperClass instance;
    public const int rangeBetweenOpenCard = -100;
    public const int rangeBetweenCloseCard = -50;
    public const int ScoreBeginGame = 100;
 
    // 
    //	*********************************
    //	*****  DRAG'n'DROP FIELDS  ******
    //	*********************************
    // 
    [SerializeField]
    private CardItem originCard;
    [SerializeField]
    private CardItemsDeck deckContainer;

    [SerializeField]
    private List<CardItemsDeck> stockContainers;
    [SerializeField]
    private List<CardItem> stockStacks;
    [SerializeField]
    private List<CardItem> foundationStacks;
    [SerializeField]
    private List<CardItem> tableuStacks;
    [SerializeField]
    public SmoothMovementManager movementManager;
    [SerializeField]
    private SmartOrientationStage smartOrientation;
    [SerializeField]
    private DanceIniter dance;


    [SerializeField]
    private List<string> oneCardWining = new List<string>();
    [SerializeField]
    private List<string> twoCardWining = new List<string>();
    [SerializeField]
    private List<string> fourCardWining = new List<string>();
    public List<CardItem> GetTableuStacks { get { return tableuStacks; } }
    public List<CardItem> GetFoundationStacks { get { return foundationStacks; } }
    float maxRadiusToAttach = 90f;
    public Transform touchPoint;

    public List<string> GetOneCardWining { get { return oneCardWining; } }
    public List<string> GetTwoCardWining { get { return twoCardWining; } }
    public List<string> GetFourCardWining { get { return fourCardWining; } }
    private ICardActions manager;
    private CardItemsBuilder builder;
    private Dictionary<int, CardItem> cardsOnStage = new Dictionary<int, CardItem>();

    [SerializeField]
    private string cards;
    [SerializeField]
    private DataCardResumeGroup []  allSlotCardRemain;

    [SerializeField]
    private RectTransform sizeCardPortrait;
    [SerializeField]
    private RectTransform sizeCardLandSpace;
    public Image bg;
  
    [HideInInspector]
    public bool prevRowComplete = false;
    [HideInInspector]
    public bool loadDealCardBeginGame = false;


    private string[,] dealContract;
    private bool firstDeal = true;

    
    public bool prevMoveCardInDeck = false;
    private List<SolitaireEngine.Model.Card> addCards = new List<SolitaireEngine.Model.Card>();

    private int stockCount = 0;



    private int currentStockIndex = 4;
    public CardItem GetCurrentStock { get { return stockStacks[currentStockIndex]; } }


    public bool AddScoreMove { get; set; }

    private List<CardItem> containerCardItems = new List<CardItem>();
    [SerializeField]
    private bool isDealCardStartGame = false;
    public bool EmptySlot
    {
        get
        {
            for (int i = 0; i < stockContainers.Count; i++)
            {
                if (stockContainers[i].gameObject.activeInHierarchy)
                {
                    return false;
                }
            }
            return true;
        }
    }
    void Awake()
    {
        instance = this;
        Input.multiTouchEnabled = false;
        GameProgress gp = new GameProgress();
        gp.InitPlayerPref();
        gp.InitNamePref();
        gp.InitCalendarPref();
    }
    void Start()
    {
        manager = StageManager.instance;


       
        Application.targetFrameRate = 60;
        GameSettings.Instance.isCalendarGame = PlayerPrefs.GetInt("isCalendarGame") == 1 ? true : false;



    }


    

    private void CreateCard()
    {

        // set up settings
        UpdateViewSettings();

        originCard.gameObject.SetActive(true);
       
        // Card Items Builder
        builder = new CardItemsBuilder(originCard, deckContainer.GetComponent<Transform>());


        int deck_stack_holder = -100;
        CardItem deck_card = deckContainer.initDeck(deck_stack_holder);
        cardsOnStage.Add(deck_stack_holder, deck_card);

        int unique_id = -123123123;
        CardItem deck_card_face_up = deckContainer.initDeckFaceUp(unique_id);
        cardsOnStage.Add(unique_id, deck_card_face_up);
        if (containerCardItems.Count > 0) return;
        for (int i = 0; i < 104; i++)
        {
            CardItem card = builder.BuildNewCardItem(i, false, 3, 1);
            containerCardItems.Add(card);
        }
    }

    public void ShowDance(UnityAction action)
    {
        HUDController.instance.setTrigger(true,
            //DanceCallback(action)
            () =>
            {
                HUDController.instance.triggerLess.SetActive(false);
                HUDController.instance.triggerFull.SetActive(false);
                dance.Hide();
                action();
            }
        );

        SetActiveCards(false);
        dance.Show(
            foundationStacks[0].rect,
             foundationStacks[1].rect,
            foundationStacks[2].rect,
            foundationStacks[3].rect,
            foundationStacks[4].rect,
             foundationStacks[5].rect,
            foundationStacks[6].rect,
             foundationStacks[7].rect,
            //DanceCallback (action)

            () =>
            {
                HUDController.instance.triggerLess.SetActive(false);
                HUDController.instance.triggerFull.SetActive(false);
                dance.Hide();
                action();
            }
        );
    }
 

    public void UpdateViewSettings()
    {

        // change backgroung
        int bg_id = GameSettings.Instance.visualPlayBackgroundSet;
        bg.sprite = ImageSettings.Instance.background[bg_id];


        // update suits
        if (cardsOnStage.Count == 0)
        {
            originCard.UpdateCardBack();
        }
        else
        {
            foreach (var c in cardsOnStage)
            {
                if (!c.Value.isRoot)
                    c.Value.UpdateCardBack();
            }
        }

        // update status bar
        bool isMoveTime = GameSettings.Instance.isMoveTimeSet;
        bool isScore = GameSettings.Instance.isScoreSet;
        HUDController.instance.SetBarsVisibility(isMoveTime, isMoveTime, isScore);

        // update left-right hand layouts
        smartOrientation.Refresh();
        HUDController.instance.smartOrientationHUD.Refresh();
    }

    public void ResetView()
    {
 

    //    HUDController.instance.SetSolutionLayout(false);

    }
    public void RemoveCards()
    {
        // clear list of all cards
        cardsOnStage.Clear();

    }

    // 
    //	*********************************
    //	*******  MANAGER'S CALLS  *******
    //	*********************************
    // 
    #region IView implementation

    public string cardTest = "";

    private List<SolitaireEngine.Model.Card> startingCards = new List<SolitaireEngine.Model.Card>();



    public void SaveCandelar(int value)
    {
        PlayerPrefs.SetInt("isCalendarGame", value);

    }

    public void Init(int null_card, int deck_stack_holder, int[] foundation_stack_holder, int[] tableau_stack_holder,
        List<SolitaireEngine.Model.Card> startingCards, string[,] deal_contract)
    {


        string cardWining = string.Empty;


        cards = string.Empty;
      
        if (GameSettings.Instance.numberOfSuit == 1)
        {
            cardWining = oneCardWining[Random.Range(0, oneCardWining.Count - 1)];
        }

        else
            if (GameSettings.Instance.numberOfSuit == 2)
        {
    
            cardWining = twoCardWining[Random.Range(0, twoCardWining.Count - 1)];
        }
        else
        {
     
            cardWining = fourCardWining[Random.Range(0, fourCardWining.Count - 1)];
        }

        if (GameSettings.Instance.calendarData != string.Empty || GameSettings.Instance.isCalendarGame)
        {
         
           cardWining = GameSettings.Instance.calendarData;

        }
        else
        {
            GameSettings.Instance.calendarData = cardWining;
            PlayerPrefAPI.Set();
        }
 
        string[] index = cardWining.Split('*');
        CreateCard();
        
        allSlotCardRemain = (PlayerPrefAPI.LoadDataRemainCardInSlot() == string.Empty) ? new DataCardResumeGroup[0]: JsonHelper.FromJson<DataCardResumeGroup>(PlayerPrefAPI.LoadDataRemainCardInSlot());
        DataCardResume[] dataCards = new DataCardResume[0];
        if (PlayerPrefAPI.LoadDataStep() != string.Empty)
        {
           dataCards = JsonHelper.FromJson<DataCardResume>(PlayerPrefAPI.LoadDataStep());
        }
        CardItem cardTo = deckContainer.deckHiddenContainer.getChildestCard();
       
        for (int i = startingCards.Count - 1; i >= 0; i--)
        {
            int levelIndex = (index.Length - 1) - i;
        
            string[] str = index[levelIndex].Split('-');
            startingCards[i].Id = int.Parse(str[0].ToString());
            startingCards[i].Rank = int.Parse(str[1].ToString());
            startingCards[i].Suit = int.Parse(str[2].ToString());


            CardItem card = containerCardItems[i];
           
            card.initCard(startingCards[i].Id, startingCards[i].IsOpened, startingCards[i].Suit, startingCards[i].Rank);

            card.attachListener(this);
            cards = cards + "*" + card.Id.ToString() + card.Rank.ToString() + "-" + card.Suit.ToString();
            card.Card(startingCards[i]);

            card.gameObject.SetActive(true);
          
            card.openCard(false);
            if (dataCards.Length == 0)
            {
         
                MoveCard(card, cardTo);
                cardTo = card;
            }
            else
            {
               setOffset(card, FindCardItem(-100), Vector2.zero);
            }

            cardsOnStage.Add(startingCards[i].Id, card);
        }
        
        cardTest = string.Empty;

        this.startingCards = startingCards;

        originCard.gameObject.SetActive(false);

        // Fundation
        for (int i = 0; i < foundation_stack_holder.Length; i++)
        {
            if (!foundationStacks[i].GetSetUpStartGame) foundationStacks[i].initCard(foundation_stack_holder[i], false, -1, -1);//c);
            cardsOnStage.Add(foundation_stack_holder[i], foundationStacks[i]);
        }

        // Tableu
        for (int i = 0; i < tableuStacks.Count; i++)
        {
            //			Card c = new Card(tableau_stack_holder[i], -1,-1);
            if (!tableuStacks[i].GetSetUpStartGame) tableuStacks[i].initCard(tableau_stack_holder[i], false, -1, -1);//(c);
            cardsOnStage.Add(tableau_stack_holder[i], tableuStacks[i]);
        }


        // Stock
        int[] stock_stack_holder = new int[5] { -200, -201, -202, -203, -204 };
        for (int i = 0; i < stockStacks.Count; i++)
        {
            //			Card c = new Card(tableau_stack_holder[i], -1,-1);
          if( !stockStacks[i].GetSetUpStartGame)  stockStacks[i].initCard(stock_stack_holder[i], false, -1, -1);//(c);
            cardsOnStage.Add(stock_stack_holder[i], stockStacks[i]);
        }

        // measure radius to nearest card when we drop one
        float distance_between_cards = Vector3.Distance(tableuStacks[0].transform.position, tableuStacks[1].transform.position);
#if UNITY_EDITOR
        Assert.AreNotEqual(0, distance_between_cards);
#endif
        float magic_number = 1.5f;
        maxRadiusToAttach = distance_between_cards * magic_number;

     
        if (dataCards.Length == 0)
        {
           DealCardStartGame();
        }
        else
        {

            DealCardContinueMode();
            StartCoroutine(ContinueModeGame.instance.LoadResumeTable());
        }


    }

    private void DealCardContinueMode()
    {

        AddScoreMove = true;
        for (int i = 0; i < allSlotCardRemain.Length; i++)
        {
            for (int j = 0; j < allSlotCardRemain[i].dataCardResumes.Count -1; j++)
            {
                CardItem cardFrom = FindCardItem(allSlotCardRemain[i].dataCardResumes[j+1].id);
                CardItem cardTo = FindCardItem(allSlotCardRemain[i].dataCardResumes[j].id);
                cardFrom.openCard(allSlotCardRemain[i].dataCardResumes[j+1].isOpen);
                cardTo.openCard(allSlotCardRemain[i].dataCardResumes[j].isOpen);
             
                MoveCard(cardFrom, cardTo);
            }
        }

    }




    private void DealCardStartGame()
    {
        // dealContract = deal_contract;
        // TODO deal by contract
        StopAllCoroutines();
        isDealCardStartGame = true;
       
        StartCoroutine(ContinueModeGame.instance.LoadResumeTable());
        stockCount = 0;
  
        dealContract = new string[,]
    {
        { "0", "0", "0" ,"0", "0", "0", "0", "0", "0", "0" },
        { "0", "0", "0" ,"0", "0", "0", "0", "0", "0", "0" },
        { "0", "0", "0" ,"0", "0", "0", "0", "0", "0", "0" },
        { "0", "0", "0" ,"0", "0", "0", "0", "0", "0", "0" },
        { "0", "0", "0", "0", "1" ,"1", "1", "1", "1", "1" },
        { "1", "1", "1", "1", " " ," ", " ", " ", " ", " " },
       { "0", "0", "0" ,"0", "0", "0", "0", "0", "0", "0" },
        { "0", "0", "0" ,"0", "0", "0", "0", "0", "0", "0" },
         { "0", "0", "0" ,"0", "0", "0", "0", "0", "0", "0" },
          { "0", "0", "0" ,"0", "0", "0", "0", "0", "0", "0" },
          { "0", "0", "0" ,"0", "0", "0", "0", "0", "0", "0" },
    };


        


       StartCoroutine(DealCards(dealContract, deckContainer, null, 10, 11, false));

       

      

        for (int i = 0; i < stockContainers.Count; i++)
        {
            stockContainers[i].gameObject.SetActive(true);
        }



        currentStockIndex = 4;


        

     
      

    }






    
    IEnumerator DealCards(string[,] contract, CardItemsDeck stock, UnityAction callback = null, int x = 10, int y = 6, bool addMoveCard = false, float waitTime = 0.03f, bool useStockStack = false,bool dealCard = true)
    {
        HUDController.instance.VisibleButtonsAction(false);
        AddScoreMove = true;
        if (!useStockStack)
        {
            movementManager.SetMovingSpeed(SmoothMovementManager.Speed.Deal);
        }
        else
        {
            movementManager.SetMovingSpeed(SmoothMovementManager.Speed.Undo);
        }
        addCards.Clear();
        List<int> idFrom = new List<int>();
        List<int> idTo = new List<int>();
        List<int> idFromParent = new List<int>();
        HUDController.instance.setTrigger(true, null);

        HUDController.instance.UpdateGameMode();
        if (!ContinueModeGame.instance.LoadSuccess)
        {
            waitTime = 0;
        }

        int count = 0;
        int lengthJ = 1;

       

        while (count < y)
        {
           
            if (count > 5)
            {
                lengthJ = 10;
                x = 1;
                useStockStack = true;
                waitTime = 0;
            }

            for (int i = 0; i < x; i++)
            {

                for (int j = 0; j < lengthJ; j++)
                {
                    string c = contract[count, i];

                    if (ContinueModeGame.instance.LoadSuccess) Sound.Instance.StartNew();
                    bool deal = c == "0" || c == "1";

                    if (deal)
                    {
                        // to open last card
                        bool isFaceUp = c == "1";

                        // get last card from deck
                        CardItem cardFromDeck = stock.deckHiddenContainer.getChildestCard();
                        addCards.Add(cardFromDeck.GetCard);


                        CardItem cTo = tableuStacks[i].getChildestCard();
                        if (useStockStack)
                        {
                            // card to
                            cTo = stockStacks[stockCount].getChildestCard();
                        }
                        cardFromDeck.MoveComplete = false;
                        cTo.MoveComplete = false;
                        if (cardFromDeck.Id != -100)
                        {
                          
                            idFrom.Add(cardFromDeck.Id);
                            idTo.Add(cTo.Id);

                            idFromParent.Add(cardFromDeck.parentCard.Id);
                            
                            DealOneCard(cardFromDeck, cTo, isFaceUp);

                            cardFromDeck.transform.localScale = Vector3.one;
                        }
                      
                        // deal
                      
                     

                        if (waitTime > 0)
                            yield return new WaitForSeconds(waitTime);
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            count++;

            if (useStockStack)
            {

                stockCount++;

            }
        }

        loadDealCardBeginGame = true;




        solitaireTimer.Schedule(this, 0.43f, () => {
            if (addMoveCard)
            {
                MoveBackRowCardCommands moveBackCard = new MoveBackRowCardCommands(false, false,0);
                prevMoveCardInDeck = true;
                solitaireTimer.Schedule(this, .6f, () =>
                {
                    prevMoveCardInDeck = false;
                });

                SolitaireSpiderCheck.instance.DataDealCardGroup.Add(new DataCardResumeGroup());
                for (int i = 0; i < idFrom.Count; i++)
                {

                    ICommand command = StageManager.instance.AddMoveCard(idFrom[i], idTo[i], idFromParent[i]);
                    DataCardResume card = new DataCardResume();
                    card.id = idFrom[i];
                    card.bestPlaceId = idTo[i];
                    card.parentID = idFromParent[i];
                    SolitaireSpiderCheck.instance.DataDealCardGroup[SolitaireSpiderCheck.instance.DataDealCardGroup.Count - 1].dataCardResumes.Add(card);
                    moveBackCard.AddCommand(command);

                }
                string lastData = JsonHelper.ToJson<DataCardResumeGroup>(SolitaireSpiderCheck.instance.DataDealCardGroup.ToArray(), true);
                PlayerPrefAPI.SaveStockCard(lastData);



                StageManager.instance.GetExecutor.Execute(moveBackCard);



            }
           
                HUDController.instance.VisibleButtonsAction(true);
          

           
        });
      




        HUDController.instance.triggerLess.SetActive(false);
        HUDController.instance.triggerFull.SetActive(false);

        if (callback != null)
            callback();
        StartCoroutine(ScaleChilldCardTableu(1));
        
        yield return null;
    }

    void DealOneCard(CardItem card, CardItem cardTo, bool isFaceUp)
    {

#if UNITY_EDITOR
        if (card == null)
        {
            throw new UnityException("Card from deck is null!");
        }
#endif

        // TODO remove it
        if (card.parentCard != null)
            card.parentCard.childCardNull();

 

        // attach to tableu
        MoveCard(card, cardTo, false, true, () =>
        {
           
            if (isFaceUp)
            {

                card.openCardAnim();
            }
        });


    }


    public void Reset(UnityAction callback = null)
    {

        MoveToHeap();

        MoveToDeck();
        SetActiveCards(true);
        StopAllCoroutines();
        DealCardStartGame();


    }
    private IEnumerator ScaleChilldCardTableu(float waitTime = 5f)
    {
        yield return new WaitForSeconds(waitTime);
        for (int i = 0; i < tableuStacks.Count; i++)
        {
            CardItem[] cards = tableuStacks[i].GetComponentsInChildren<CardItem>();
            for (int j = 1; j < cards.Length; j++)
            {
                cards[j].transform.localScale = Vector3.one;
            }
        }

        for (int i = 0; i < stockStacks.Count; i++)
        {
            CardItem[] cards = stockStacks[i].GetComponentsInChildren<CardItem>();
            for (int j = 1; j < cards.Length; j++)
            {
                cards[j].transform.localScale = Vector3.one;
            }
        }
    }

    public void SetActiveCards(bool isActive)
    {
        //		int cc = 0;
        // detach all cards
        foreach (var c in cardsOnStage)
        {
            if (!c.Value.isRoot)
            {
                c.Value.gameObject.SetActive(isActive);
            }
        }
    }
    public void MoveToHeap()
    {

        foreach (var c in cardsOnStage)
        {
            if (!c.Value.isRoot)
            {
                attachCard(c.Value, bg.transform);

              
                c.Value.openCard(false);
                c.Value.transform.localPosition = new Vector3(10000f, 0);
                c.Value.childCardNull();
                c.Value.parentCard = null;
            }
        }

    }

    private void MoveToDeck()
    {
        CardItem cardTo = deckContainer.deckHiddenContainer.getChildestCard();
        for (int i = this.startingCards.Count - 1; i >= 0; i--)
        {


            // find card on stage ordered by id
 
            CardItem cardFrom = getCardById(this.startingCards[i].Id);
            // find last card in deck
           
 
            // attach card into deck
            MoveCard(cardFrom, cardTo);
            cardTo = cardFrom;
            
        }
    }






    // MOVING
    public void MoveCard(CardItem cardFrom, CardItem cardTo, bool moveCurve = false, bool animation = false, UnityAction unityAction = null)
    {

      



        bool should_show_salute_effect = false;

        if (GameSettings.Instance.isEffectSet && ContinueModeGame.instance.LoadSuccess)
        {
            // NOTE: we have to check it before attach card because card will be attached to foundation
            bool targetFromFoundation = cardFrom.getRootCard().isFoundation;
            bool destinationFromFundation = cardTo.getRootCard().isFoundation;

            // Show salute effect only if card from deck or tableu and will be attached to foundation
            if (!targetFromFoundation && destinationFromFundation)
            {
                should_show_salute_effect = true;
            }
        }

      
            attachCard(cardFrom, cardTo, true, moveCurve);
      

        cardFrom.childOffset = cardTo.childOffset;

        cardFrom.childOffsetOpened = cardTo.childOffsetOpened;


        if (animation)
        {
            if (!isDealCardStartGame)
            {
                SetTopLayerFor(cardFrom);
            }
            Vector2 offset = GetOffset(cardTo,cardFrom);

         //   Debug.Log(string.Format("Open Card Anim 1 :{0}-{1}-{2}", cardFrom.Id, cardTo.Id, unityAction));
            cardFrom.GetCardMove.Move(cardFrom.Id, cardFrom.transform, cardTo.transform, offset, moveCurve, () =>
                 {
                     if (should_show_salute_effect)
                     {
                         cardFrom.showSalute();
                     }

                  //   Debug.Log(string.Format("Open Card Anim 2 :{0}-{1}-{2}", cardFrom.Id, cardTo.Id, unityAction));

                     if (unityAction != null)
                         unityAction();
                     
                     setOffset(cardFrom, cardTo, offset);


                 }
            );
        }
        else
        {

            
            setOffset(cardFrom, cardTo, GetOffset(cardTo,cardFrom));
       
        }

    }





    public void ShowMoveHint(CardItem cardFrom, CardItem cardTo, CardItem parentCard)
    {
        cardFrom.allowClick = false;
        HUDController.instance.VisibleButtonsAction(false);
        SetTopLayerFor(cardFrom);

        Vector2 offset = GetOffset(cardTo, cardFrom);
        movementManager.Move(cardFrom.Id, cardFrom.transform, cardTo.transform, offset, false, () =>
        {

            offset = GetOffset(cardFrom.getParentCard(), cardFrom);
            setOffset(cardFrom, cardFrom.getParentCard(), offset);
            HUDController.instance.VisibleButtonsAction(true);
            cardFrom.allowClick = true;
        }
        );
    }




    private Vector2 GetOffset(CardItem cardTo,CardItem cardFrom)
    {
        Vector2 offset = (cardTo.isOppened )  ? cardTo.childOffsetOpened : cardTo.childOffset;
        if ( cardTo.isRoot || (cardTo.MoveComplete && !prevRowComplete))
        {
            offset = Vector3.zero;

        }

      


        return offset;
    }


    public CardItem FindTableu(CardItem card)
    {
        return card.getRootCard();
    }

    public void updateOffset(CardItem cardFrom, bool anim = false)
    {
        CardItem cardTo = cardFrom.getParentCard();
        Vector2 offset = GetOffset(cardTo,cardFrom);
        if (!anim)
        {

            cardFrom.rect.localPosition = offset;
        }
        else
        {
            movementManager.Move(cardFrom.Id, cardFrom.transform, cardTo.transform, offset, false, () =>
                 {
                    // ???
                }
            );
        }
    }

    public void setOffset(CardItem cardFrom, CardItem cardTo, Vector2 offset)
    {

        cardFrom.rect.localPosition = offset;

        if (isDealCardStartGame) return;



     

        
        if (cardTo.getRootCard().GetBetweenDistance != null)
        {
            SolitaireSpiderCheck.instance.CompleteSpider(cardTo.getRootCard(), true);
            cardTo.getRootCard().GetBetweenDistance.CheckDistanceCard(false);
        }








    }



    public void TurnCard(CardItem card, bool visible)
    {
       if(!card.isOppened && visible)
        {
            card.openCardAnim(visible);
        }
        else
        {
            card.openCard(visible);
        }
     

    }
    public void ShakeCard(CardItem card)
    {
        CardShakerAnim shakerAnim = card.GetComponent<CardShakerAnim>();
# if UNITY_EDITOR
        if (shakerAnim == null)
        {
            throw new UnityException("Can't find shaker anim");
        }
#endif
        shakerAnim.Shake();
    }

    

 

    #endregion




    // 
    //	*********************************
    //	*******  CARD'S ACTIONS  ********
    //	*********************************
    // 
    #region RECEIVED ACTIONS FORM PARTICULAR CARD

    CardItem lastParent;
     CardItem clickedLast;
    public CardItem ClickedLast { get { return clickedLast; } }

    private bool checkMoveCard(CardItem targetCard)
    {




        if (targetCard.childCard != null)
        {
            if (targetCard.Suit == targetCard.childCard.Suit)
            {

                if ((targetCard.Rank - targetCard.childCard.Rank > 1 || targetCard.Rank - targetCard.childCard.Rank <= 0))
                {



                    return true;
                }
                else
                {
                    CardItem card = targetCard.childCard;
                    CardItem childCard = card.childCard;
                    while (childCard != null)
                    {

                        if (childCard != null)
                        {

                            if (card.Suit == childCard.Suit && (card.Rank - childCard.Rank > 1 || card.Rank - childCard.Rank <= 0))
                            {
                                return true;
                            }
                            card = childCard;
                            childCard = card.childCard;
                        }
                        else break;
                    }
                }


            }

        }
        return false;
    }
    public void pressByCard(CardItem targetCard)
    {

        if (checkMoveCard(targetCard)) return;
        touchAnywhere();

        SetTopLayerFor(targetCard);

     
    }

   
    public void clickByCard(CardItem clicked)
    {
        if (checkMoveCard(clicked)) return;

        isDealCardStartGame = false;
        if (!clicked.clickable)
        {
            return;
        }

 
        if (clicked.childCard != null && clicked.GetComponentInParent<CardHasChild>() != null) return;


        manager.OnClickCard(clicked);

      
    }

 
    private int countComplete = 0;
    public void CompleteCardSpider(CardItem clicked, int best_place_id, MoveBackRowCardCommands moveBackRow, bool moveCurve)
    {

        ICommand command = manager.CompleteCardSpider(clicked.Id, clicked.parentCard.Id, best_place_id, moveCurve);
        DataCardResume card = new DataCardResume();
        card.id = clicked.Id;
        card.bestPlaceId = best_place_id;
        card.parentID = clicked.parentCard.Id;
        SolitaireSpiderCheck.instance.DataCardCompleteGroup[SolitaireSpiderCheck.instance.DataCardCompleteGroup.Count - 1].dataCardResumes.Add(card);
        moveBackRow.AddCommand(command);


        if (moveBackRow.Length() > 12)
        {

            countComplete++;

            string lastData = JsonHelper.ToJson<DataCardResumeGroup>(SolitaireSpiderCheck.instance.DataCardCompleteGroup.ToArray(), true);

            PlayerPrefAPI.SaveFoundCard(lastData);

            StageManager.instance.GetExecutor.Execute(moveBackRow);

        }
    }

    // START
    public void startDragCard(CardItem clicked)
    {
        if (checkMoveCard(clicked)) return;
        if (!clicked.clickable)
        {
            return;
        }
        isDealCardStartGame = false;
        clicked.allowClick = false;


        lastParent = clicked.getParentCard();
        clickedLast = clicked;

        movementManager.Follow(clicked.transform);

        HUDController.instance.triggerLess.SetActive(true);
    }

    // DRAG
    public void dragCard(Vector3 position)
    {

        if (clickedLast == null)
            return;

        touchPoint.transform.position = position;
    }

    // DROP
    public void endDragCard(CardItem clicked)
    {
        if (checkMoveCard(clicked)) return;
        if (!clicked.clickable)
        {
            return;
        }
        if (clickedLast == null)
            return;
        clicked.allowClick = true;
     
        HUDController.instance.triggerLess.SetActive(false);
        movementManager.Follow(null);

        List<int> near_cards = getNearestCards(clicked);
      


        manager.OnDropCard(clicked.Id, lastParent.Id, near_cards, clicked);

      


    }
    #endregion







    // 
    //	*********************************
    //	***********  HELPERS  ***********
    //	*********************************
    // 


    bool ableToAttachBack = true;



   
    public void attachCard(CardItem card, CardItem cardTo, bool unlockCard = false, bool MoveComplete = false)
    {

      //  Debug.Log(string.Format("ID1:{0}- ID2: {1}",card.Id, cardTo.Id));
        attachCard(card, cardTo.transform);

        SetTopLayerFor(cardTo);


        if (!isDealCardStartGame)
        {
            if (card.rootCard != null)
            {
                card.rootCard.GetBetweenDistance.CheckDistanceCard(false);
            }
        }


        cardTo._childCard = card;
        
        if (cardTo.isTableu)
        {
            card.rootCard = cardTo;

            CardItem[] childs = card.getChildCardsList();
            for (int i = 0; i < childs.Length; i++)
            {
            childs[i].rootCard = cardTo;
            }
        }
        else if (cardTo.rootCard != null)
        {
           
            CardItem[] childs = cardTo.getChildCardsList();
            for (int i = 0; i < childs.Length; i++)
            {
           childs[i].rootCard = cardTo.rootCard;
            }
        }


      
        card.parentCard = cardTo;









        if (MoveComplete)
        {
            card.Hide = true;
        }

        for (int i = 0; i < tableuStacks.Count; i++)
        {
            SolitaireSpiderCheck.instance.VisibleClickedCard(tableuStacks[i].GetComponentsInChildren<CardItem>());
        }





    }
    public void attachCard(CardItem card, Transform t)
    {
        if (card.parentCard != null)
            card.parentCard.childCardNull();
        card.rect.SetParent(t, true);
    }

    private void SetTopLayerFor(CardItem card)
    {
        if (layerTableu) return;
       card.getRootCard().transform.SetAsLastSibling();
    }

    List<CardItem> blinkCards = new List<CardItem>();

 
    public void blinkStack(CardItem card, bool b)
    {
#if UNITY_EDITOR
        if (card.Equals(null))
        {
            throw new UnityException("Can't blink card is null!");
        }

#endif
        foreach (var c in card.GetComponentsInChildren<CardItem>())
        {
            if (c.Hide) continue;

         
            if (b)
                blinkCards.Add(c);
        }
    }



    public void pressByVoid()
    {
        touchAnywhere();
    }

    public void pressByDeck()
    {
        touchAnywhere();
    }

    void touchAnywhere()
    {
        manager.OnClickAnywhere();
    }


    // 
    //	*********************************
    //	***********  GETTERS  ***********
    //	*********************************
    // 

    public bool hasCardById(int id)
    {
        return cardsOnStage.ContainsKey(id);
    }

    public CardItem getCardById(int id)
    {
#if UNITY_EDITOR
        if (!hasCardById(id))
        {
            throw new UnityException("Can't find card with id " + id);
        }
#endif
        return cardsOnStage[id];
    }

    List<CardItem> getChildestCardsInStage()
    {
        List<CardItem> c = new List<CardItem>();
        /*
		foreach (var cont in foundationStacks) {
			c.Add (cont.getChildestCard ());
		}
        */
        foreach (var cont in tableuStacks)
        {
            c.Add(cont.getChildestCard());
        }
        return c;
    }

    List<int> getNearestCards(CardItem card)
    {
        Dictionary<float, CardItem> cardsDistances = new Dictionary<float, CardItem>();
        List<float> distances = new List<float>();

        List<CardItem> cards = getChildestCardsInStage();
    
        foreach (var c in cards)
        {
            // ignore current stack
            if (c.getRootCard().Id == card.getRootCard().Id)
            {
                continue;
            }

            float dist = Vector3.Distance(c.rect.position, card.rect.position);

            cardsDistances.Add(dist, c);
            distances.Add(dist);
        }

        // sort
        distances.Sort();

        List<int> nearCards = new List<int>();

        for (int i = 0; i < distances.Count; i++)
        {
            float dist = distances[i];
            if (dist < maxRadiusToAttach)
            {
                nearCards.Add(cardsDistances[dist].Id);
            }
        }

        return nearCards;
    }




    #region IDebugGame implementation

    List<IDebugCardInfo> IDebugGame.GetAllCards()
    {
        List<IDebugCardInfo> allCards = new List<IDebugCardInfo>();

        foreach (var c in cardsOnStage)
        {
            CardItem cardView = c.Value;
            if (cardView.isRoot)
                continue;

            IDebugCardInfo cInfo = new IDebugCardInfo();
            cInfo.id = cardView.Id;
            cInfo.isOpen = cardView.isOppened;
            cInfo.rank = getDebugRank(cardView);
            cInfo.suit = getDebugSuit(cardView);
            cInfo.zone = getDebugZone(cardView);
            cInfo.zoneIndex = getDebugZoneIndex(cardView);
            cInfo.cardIndexInStack = getDebugIndexInStack(cardView);

            allCards.Add(cInfo);
        }

        return allCards;
    }

    private IDebugCardInfo.Rank getDebugRank(CardItem card)
    {
        switch (card.Rank)
        {
            case 1:
                return IDebugCardInfo.Rank.Ace;
            case 2:
                return IDebugCardInfo.Rank.Two;
            case 3:
                return IDebugCardInfo.Rank.Three;
            case 4:
                return IDebugCardInfo.Rank.Four;
            case 5:
                return IDebugCardInfo.Rank.Five;
            case 6:
                return IDebugCardInfo.Rank.Six;
            case 7:
                return IDebugCardInfo.Rank.Seven;
            case 8:
                return IDebugCardInfo.Rank.Eight;
            case 9:
                return IDebugCardInfo.Rank.Nine;
            case 10:
                return IDebugCardInfo.Rank.Ten;
            case 11:
                return IDebugCardInfo.Rank.Jack;
            case 12:
                return IDebugCardInfo.Rank.Quen;
            case 13:
                return IDebugCardInfo.Rank.King;
            default:
                throw new UnityException("Can't parse card item rank!");
        }
    }
    private IDebugCardInfo.Suit getDebugSuit(CardItem card)
    {
        switch (card.Suit)
        {
            case 0:
                return IDebugCardInfo.Suit.Diamonds;
            case 1:
                return IDebugCardInfo.Suit.Heart;
            case 2:
                return IDebugCardInfo.Suit.Clubs;
            case 3:
                return IDebugCardInfo.Suit.Spades;
            default:
                throw new UnityException("Can't parse card item suit!");
        }
    }
    private IDebugCardInfo.Zone getDebugZone(CardItem card)
    {
        if (card.getRootCard().isFoundation)
        {
            return IDebugCardInfo.Zone.Foundation;
        }
        else if (card.getRootCard().isDeck)
        {
            return IDebugCardInfo.Zone.Deck;
        }
        return IDebugCardInfo.Zone.Tableu;
    }
    private int getDebugZoneIndex(CardItem card)
    {
        return card.getRootCard().debugZoneIndex;
    }
    private int getDebugIndexInStack(CardItem carrd)
    {

        CardItem rood = carrd.getRootCard();

        int ind = 0;
        // TODO remove deck case
        if (rood.isDeck && rood.debugZoneIndex == 0)
        {
            foreach (var c in rood.getChildCardsList())
            {
                if (c.Id == carrd.Id)
                {
                    return rood.getChildCardsList().Length - ind - 1;
                }
                ind++;
            }
        }
        else
        {
            foreach (var c in rood.getChildCardsList())
            {
                if (c.Id == carrd.Id)
                {
                    return ind;
                }
                ind++;
            }
        }


        return -1;
    }

    #endregion


    public void OffAllCardInContainerCard()
    {
        for (int i = 0; i < stockStacks.Count; i++)
        {
            CardItem[] cardItems = stockStacks[i].GetComponentsInChildren<CardItem>();

            for (int j = 0; j < cardItems.Length; j++)
            {
                cardItems[j].openCard(false);
            }
        }

    }

    public void UpStockDealCard()
    {
        currentStockIndex++;

        if (currentStockIndex >= 4) currentStockIndex = 4;
        stockContainers[currentStockIndex].gameObject.SetActive(true);
    }
    public CardItem FindCardItem(int id)
    {
        if (cardsOnStage.ContainsKey(id))
        {
            return cardsOnStage[id];
        }
        return null;
    }
    public void DealCardInDeck(int slotCard, bool addDataCard = true, float waitTime = 0.03f)
    {
        if (slotCard < currentStockIndex) return;
        if (stockStacks[slotCard].transform.childCount <= 1 && addDataCard) return;


        if (currentStockIndex < 0) return;

        CardStockSlots.instance.undo = false;



        if (addDataCard)
        {
            ContinueModeGame.instance.AddDataCard(-99, -99, slotCard, true);
            string[,] dealContract = new string[,]
{
                            { "1", "1", "1" ,"1", "1", "1", "1", "1", "1", "1" },
};


            StartCoroutine(DealCards(dealContract, stockContainers[currentStockIndex], null, 10, 1, true, waitTime));
            HUDController.instance.VisibleButtonsAction(false);
        }
     
        stockContainers[currentStockIndex].gameObject.SetActive(false);
        currentStockIndex--;
    
    
    }



    public void DisableBlinkEffect()
    {
        StageManager.instance.ResetBlinkCard();
    }
    private IEnumerator AddCardHolder()
    {
        yield return new WaitForSeconds(0.4f);
        StageManager.instance.GetSolitaire.AddCard(addCards);
    }
    List<CardItem> cardItems = new List<CardItem>();

    public class CardRankDetail
    {
        public int rank;
        public List<CardSuitDetail> suits = new List<CardSuitDetail>();
        public int position;
        public int GetSuit()
        {
            for (int i = 0; i < suits.Count; i++)
            {
                if (suits[i].count == 0)
                {
                    suits.RemoveAt(i);
                    i--;
                }
            }

            position = Random.Range(0, suits.Count);

            suits[position].count--;
            return suits[position].suit;
        }
    }
    public void ScaleCard(bool isPortrait)
    {
       
        for (int i = 0; i < tableuStacks.Count; i++)
        {
            tableuStacks[i].transform.localScale = SolitaireStageViewHelperClass.instance.ConvertSizeCard(isPortrait);
        }
        for (int i = 0; i < foundationStacks.Count; i++)
        {
            foundationStacks[i].transform.localScale = SolitaireStageViewHelperClass.instance.ConvertSizeCard(isPortrait);
        }

        for (int i = 0; i < stockStacks.Count; i++)
        {
            stockStacks[i].transform.localScale = SolitaireStageViewHelperClass.instance.ConvertSizeCard(isPortrait);
        }

        for (int i = 0; i < stockContainers.Count; i++)
        {
            stockContainers[i].transform.localScale = SolitaireStageViewHelperClass.instance.ConvertSizeCard(isPortrait);
        }

    }
    public Vector2 ConvertSizeCard(bool isPortrait)
    {
        float sizeX = 0;
        float sizeY = 0;
        float max = Mathf.Max(Screen.height, Screen.width);
        float min = Mathf.Min(Screen.height, Screen.width);
        float ratioResolution = max / min;
  
        if (isPortrait)
        {
            float minSize = Mathf.Min(sizeCardPortrait.sizeDelta.x, sizeCardPortrait.sizeDelta.y);

           
            sizeX = sizeY = minSize / foundationStacks[0].GetComponent<RectTransform>().sizeDelta.x;

        }
        else
        {
            float minSize = Mathf.Min(sizeCardLandSpace.sizeDelta.x, sizeCardLandSpace.sizeDelta.y);

            sizeX = sizeY = minSize / foundationStacks[0].GetComponent<RectTransform>().sizeDelta.x;

        
        }
    
       
        Vector2 size = new Vector2(sizeX, sizeY);
 
        return size;
     

    }
    public class CardSuitDetail
    {

        public CardSuitDetail(int suit, int count)
        {
            this.suit = suit;
            this.count = count;
        }

        public int suit;
        public int count;
    }

    public void ChangeCardFace()
    {
        CardItem[] cardItems = FindObjectsOfType<CardItem>();
        for (int i = 0; i < cardItems.Length; i++)
        {
            if (cardItems[i].isRoot) continue;
            cardItems[i].GetComponent<CardImage>().SetCard(cardItems[i].Suit, cardItems[i].Rank - 1);
           
        }
    }

   


    public void SetAllDistanceBetweenCard(bool resetLength)
    {

        for (int i = 0; i < tableuStacks.Count; i++)
        {
            BetweenDistanceCard betweenDistanceCard = tableuStacks[i].GetComponent<BetweenDistanceCard>();
            betweenDistanceCard.CheckDistanceCard(resetLength);
        }


    }



    private IEnumerator Movecurve(List<ICommand> commands, float waitTimeMoveCurve)
    {
       
        for (int i = 0; i < commands.Count; i++)
        {
           
            if (waitTimeMoveCurve>0)
            yield return new WaitForSeconds(waitTimeMoveCurve);
            commands[i].execute();
        }
       
    }
    

    public void ActiveMovecurve(List<ICommand> commands, float waitTimeMoveCurve)
    {
        StartCoroutine(Movecurve(commands, waitTimeMoveCurve));
    }
  

    public void HighLightFreeCards()
    {
        for (int i = 0; i < tableuStacks.Count; i++)
        {
            CardItem[] cardItems = tableuStacks[i].GetComponentsInChildren<CardItem>();
            for (int j = 1; j < cardItems.Length; j++)
            {
                cardItems[j].ActiveHighLightCard(GameSettings.Instance.isHighLightMode);
            }
        }
    }

    private bool layerTableu = false;
    public IEnumerator SetLayerTableu(bool isHand)
    {
        layerTableu = true;
        if (isHand)
        {
            for (int i = 0; i < tableuStacks.Count; i++)
            {
                if (i != 1 && DeviceOrientationHandler.instance.isVertical)
                    tableuStacks[i].transform.SetAsLastSibling();
                else
                    tableuStacks[i].transform.SetAsLastSibling();
            }
           if ( DeviceOrientationHandler.instance.isVertical)
           tableuStacks[1].transform.SetAsFirstSibling();
          //  tableuStacks[tableuStacks.Count-2].transform.SetAsLastSibling();
        }
        else
        {
            for (int i = tableuStacks.Count - 1; i >= 0; i--)
            {
                if (i != tableuStacks.Count-2 && DeviceOrientationHandler.instance.isVertical)
                    tableuStacks[i].transform.SetAsLastSibling();
                else
                    tableuStacks[i].transform.SetAsLastSibling();
            }
            if (DeviceOrientationHandler.instance.isVertical)
                tableuStacks[tableuStacks.Count-2].transform.SetAsFirstSibling();
        }
        yield return new WaitForSeconds(1f);
        layerTableu = false;
    }
    public void SaveMedalCurrentMonth()
    {
        int day = GameSettings.Instance.calendarGameDay - 1;
        int month = GameSettings.Instance.calendarGameMonth;
        int year = GameSettings.Instance.calendarGameYear;


        string fileName = string.Format("{0}{1}{2}", day < 9 ? "0" + day : day.ToString(), month < 9 ? "0" + month : month.ToString(), year);
 
        PlayerPrefs.SetInt(fileName, 1);


    }

    public float RatioResolution()
    {
        float max = Mathf.Max(Screen.height, Screen.width);
        float min = Mathf.Min(Screen.height, Screen.width);
        float ratioResolution = max / min;
        return ratioResolution;
    }


    public IEnumerator GetRemainCardInSlots()
    {
        yield return new WaitForSeconds(1f);
        string data = string.Empty;
        List<DataCardResumeGroup> dataCardResumeGroups = new List<DataCardResumeGroup>();
        for (int i = 0; i < tableuStacks.Count; i++)
        {
            CardItem[] cardChildTableu = tableuStacks[i].GetComponentsInChildren<CardItem>();
            dataCardResumeGroups.Add(new DataCardResumeGroup());
            for (int j = 0; j < cardChildTableu.Length; j++)
            {
                DataCardResume dataCard = new DataCardResume();
                dataCard.id = cardChildTableu[j].Id;
                dataCard.isOpen = cardChildTableu[j].isOppened;
                dataCardResumeGroups[dataCardResumeGroups.Count - 1].dataCardResumes.Add(dataCard);
            }


        }

        for (int i = 0; i < stockStacks.Count; i++)
        {




            CardItem[] cardChildStock = stockStacks[i].GetComponentsInChildren<CardItem>();
            dataCardResumeGroups.Add(new DataCardResumeGroup());



            for (int j = 0; j < cardChildStock.Length; j++)
            {
                DataCardResume dataCard = new DataCardResume();
                dataCard.id = cardChildStock[j].Id;
                dataCard.isOpen = cardChildStock[j].isOppened;
                dataCardResumeGroups[dataCardResumeGroups.Count - 1].dataCardResumes.Add(dataCard);
            }






        }

        for (int i = 0; i < foundationStacks.Count; i++)
        {




            CardItem[] cardChildFoundation = foundationStacks[i].GetComponentsInChildren<CardItem>();
            dataCardResumeGroups.Add(new DataCardResumeGroup());



            for (int j = 0; j < cardChildFoundation.Length; j++)
            {
                DataCardResume dataCard = new DataCardResume();
                dataCard.id = cardChildFoundation[j].Id;
                dataCard.isOpen = cardChildFoundation[j].isOppened;
                dataCardResumeGroups[dataCardResumeGroups.Count - 1].dataCardResumes.Add(dataCard);
            }
        }

        data = JsonHelper.ToJson<DataCardResumeGroup>(dataCardResumeGroups.ToArray(), true);
        PlayerPrefAPI.SaveDataRemainCardInSlot(data);

    }

    public List<CardRankDetail> cardRankDetails = new List<CardRankDetail>();
 
    public override void GUIEditor()
    {
#if UNITY_EDITOR



        if (GUILayout.Button("Auto Play Game"))
        {

            cardItems = GetComponent<AutoPlayGame>().AutoPlay();
            if (cardItems.Count == 0)
            {
                DealCardInDeck(currentStockIndex);
            }
            else
            {
                for (int i = 0; i < cardItems.Count; i++)
                {

                    if (cardItems[i] != null)
                        clickByCard(cardItems[i]);
                }

            }

        }

        /*

     if (GUILayout.Button("Four Card Suit WinMode"))
     {

         fourCardWining.Clear();

         for (int h = 0; h < oneCardWining.Count; h++)
         {
             string[] index = oneCardWining[h].Split('*');

             cardRankDetails.Clear();



             for (int i = 0; i < 13; i++)
             {
                 CardRankDetail card = new CardRankDetail();
                 card.rank = i + 1;
                 card.suits = new List<CardSuitDetail>();

                 CardSuitDetail diamonds = new CardSuitDetail(0, 2);
                 CardSuitDetail heart = new CardSuitDetail(1, 2);
                 CardSuitDetail clubs = new CardSuitDetail(2, 2);
                 CardSuitDetail spaders = new CardSuitDetail(3, 2);

                 card.suits.Add(heart);
                 card.suits.Add(diamonds);
                 card.suits.Add(clubs);
                 card.suits.Add(spaders);

                 cardRankDetails.Add(card);
             }

             cards = string.Empty;
             cardTest = string.Empty;
             for (int i = index.Length - 1; i >= 0; i--)
             {
                 string[] str = index[(index.Length - 1) - i].Split('-');
                 int id = int.Parse(str[0]);
                 int rank = int.Parse(str[1]);
                 int suit = cardRankDetails[rank - 1].GetSuit();

                 cards = cards + "*"+id+"-" + rank.ToString() + "-" + suit.ToString();

             }


             for (int i = 1; i < cards.Length; i++)
             {
                 cardTest += cards[i].ToString();
             }
             fourCardWining.Add(cardTest);
         }

     }



     if (GUILayout.Button("Two Card Suit WinMode"))
     {

         twoCardWining.Clear();

         for (int h = 0; h < oneCardWining.Count; h++)
         {
             string[] index = oneCardWining[h].Split('*');

             cardRankDetails.Clear();



             for (int i = 0; i < 13; i++)
             {
                 CardRankDetail card = new CardRankDetail();
                 card.rank = i + 1;
                 card.suits = new List<CardSuitDetail>();

                 CardSuitDetail diamonds = new CardSuitDetail(0, 2);
                 CardSuitDetail heart = new CardSuitDetail(0, 2);
                 CardSuitDetail clubs = new CardSuitDetail(3, 2);
                 CardSuitDetail spaders = new CardSuitDetail(3, 2);

                 card.suits.Add(heart);
                 card.suits.Add(diamonds);
                 card.suits.Add(clubs);
                 card.suits.Add(spaders);

                 cardRankDetails.Add(card);
             }

             cards = string.Empty;
             cardTest = string.Empty;
             for (int i = index.Length - 1; i >= 0; i--)
             {
                 string[] str = index[(index.Length - 1) - i].Split('-');
                 int id = int.Parse(str[0]);
                 int rank = int.Parse(str[1]);
                 int suit = cardRankDetails[rank - 1].GetSuit();

                 cards = cards + "*" + id + "-" + rank.ToString() + "-" + suit.ToString();

             }


             for (int i = 1; i < cards.Length; i++)
             {
                 cardTest += cards[i].ToString();
             }
             twoCardWining.Add(cardTest);
         }

     }

     if (GUILayout.Button("One Card Suit WinMode"))
     {
         cardTest = string.Empty;
         for (int i = 1; i < cards.Length; i++)
         {
             cardTest += cards[i].ToString();
         }
         oneCardWining.Add(cardTest);
     }
     if (GUILayout.Button("Add ID Card"))
     {
         List<int> ids = new List<int>();
         for (int i = 0; i < oneCardWining.Count; i++)
         {
             ids.Clear();
             for (int j = 1; j <= 104; j++)
             {
                 ids.Add(j);
             }
             Shuffle(ids);
             string[] Cards = oneCardWining[i].Split('*');
             string data = string.Empty;

             for (int h = 0; h < Cards.Length; h++)
             {
                 string[] infors = Cards[h].Split('-');
                 data += ids[h] + "-" + infors[0] + "-" + infors[1];

                 data += "*";
             }
             string resultData = string.Empty;
             for (int h = 0; h < data.Length - 1; h++)
             {
                 resultData += data[h].ToString();
             }
             oneCardWining[i] = resultData;
             twoCardWining[i] = resultData;
             fourCardWining[i] = resultData;
         }
     }
      */
        base.GUIEditor();
#endif
 }

 void Shuffle(List<int> a)
 {
     // Loop array
     for (int i = a.Count - 1; i > 0; i--)
     {
         // Randomize a number between 0 and i (so that the range decreases each time)
         int rnd = UnityEngine.Random.Range(0, i);

         // Save the value of the current i, otherwise it'll overwrite when we swap the values
         int temp = a[i];

         // Swap the new and old values
         a[i] = a[rnd];
         a[rnd] = temp;
     }
 }

    }
