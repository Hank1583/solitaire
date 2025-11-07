using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SolitaireEngine;
using SolitaireEngine.Model;
using ManagerUtils;
using Calendar;
using System.Collections;
using UnityEngine.Events;
//help@navyosftgames.com version 1.1 Asset Unity Store

public partial class StageManager : MonoBehaviour, ICardActions, IManagerBaseCommands, IHUDActions
{
	private HUDController hud;

	public static StageManager instance;
	private void Awake() {instance = this;}

	private IManagerBaseCommands manager;
	private IViewBaseCommands viewer;

	private Solitaire solitaire;
	public Solitaire GetSolitaire {get { return solitaire;}} // Debuger Del it in release

	private CommandExecutor executor = new CommandExecutor();
    public CommandExecutor GetExecutor { get { return executor; } }
    [HideInInspector]
	public CommandListUpdater commandUpdater;

	private CommandBuilder commandBuilder;
	private ManagerLogic managerLogic;

    // TODO in future: move out into asset
 
    private List<SolitaireEngine.Model.Card> cardContainer= new List<SolitaireEngine.Model.Card>();
    public List<SolitaireEngine.Model.Card> GetCardContainer
    {
        get
        {
            return cardContainer;
        }
    }
    public IViewBaseCommands GetViewBaseCommands
    {
        get
        {
            return viewer;
        }
    }

    string[,] dealContract = new string[,]
{
        { "0", "0", "0" ,"0", "0", "0", "0", "0", "0", "0" },
        { "0", "0", "0" ,"0", "0", "0", "0", "0", "0", "0" },
        { "0", "0", "0" ,"0", "0", "0", "0", "0", "0", "0" },
        { "0", "0", "0" ,"0", "0", "0", "0", "0", "0", "0" },
        { "0", "0", "0", "0", "1" ,"1", "1", "1", "1", "1" },
        { "1", "1", "1", "1", " " ," ", " ", " ", " ", " " },

};





    #region EnterGame
    public void OnNewGame()
    {
        // if (GameSettings.Instance.isSoundSet) Sound.Instance.Distribute();


        // TODO: re-use old cards

        SolitaireStageViewHelperClass.instance.RemoveCards();

        // wait one update to prevent some bugs




        StartCoroutine(InitGenegal());


    }

    public void OnRestartGame()
    {

        managerLogic.InitScoring();

      InitHUD();
      solitaire.Restart();

        SolitaireStageViewHelperClass.instance.ResetView();

        viewer.Restart(solitaire.GetStartingCardsId(), OnGameRestarted);
       
        CardItem[] cards = FindObjectsOfType<CardItem>();
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].Hide = false;
        }
       
    }
    void OnGameRestarted(){
		print ("cards are dealt");
	}

	public void OnContinueGame()
	{

	}

	public void OnPause()
	{
	}
	#endregion;	

	private void Start()
	{
		GameSettings.Instance.isGameStarted = false;
		viewer = StageView.instance;
		manager = this;
		commandUpdater = CommandListUpdater.instanse;
		GameFlowDispatcher.Instance.FromManagerToMenu ();
		managerLogic = new ManagerLogic ();
	}


    IEnumerator InitGenegal()
    {
        yield return new WaitForEndOfFrame();
        InitSolitaire();

        InitHUD();

        managerLogic.InitScoring();

        InitViewer();

        commandBuilder = new CommandBuilder(solitaire, manager, viewer);
    }
   
    private int currentNumberOfSuit = -1;
    private void InitSolitaire()
	{
      

      
      
        int count = 0;
       
        if (currentNumberOfSuit!= GameSettings.Instance.numberOfSuit)
        {
            
            currentNumberOfSuit = GameSettings.Instance.numberOfSuit;
            cardContainer.Clear();
            switch (GameSettings.Instance.numberOfSuit)
            {
                case 1:


                    int value = 8;

                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 13; j++)
                        {
 
                            count++;
                            cardContainer.Add(new SolitaireEngine.Model.Card(count, (j + 1), 3));
                        }

                    }

                    break;

                case 2:
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 13; j++)
                        {
                            count++;

                            cardContainer.Add(new SolitaireEngine.Model.Card(count, (j + 1), 0));

                        }

                    }


                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 13; j++)
                        {
                            count++;

                            cardContainer.Add(new SolitaireEngine.Model.Card(count, (j + 1), 3));

                        }

                    }

                    break;



                case 4:
                    for (int i = 0; i < 8; i++)
                    {
                        int rank = i;
                        for (int j = 0; j < 13; j++)
                        {
                            count++;
                            if (i >= 4) rank = i - 4;
                            cardContainer.Add(new SolitaireEngine.Model.Card(count, (j + 1), rank));

                        }

                    }

                    break;
            }
        }
       
       


       Shuffle(cardContainer);


        List<SolitaireEngine.Model.Card> deck = new List<SolitaireEngine.Model.Card>();
        deck.AddRange(cardContainer);


        
		solitaire = new Solitaire (deck, null);
      
    }

    void Shuffle(List<SolitaireEngine.Model.Card> a)
    {
        // Loop array
        for (int i = a.Count - 1; i > 0; i--)
        {
            // Randomize a number between 0 and i (so that the range decreases each time)
            int rnd = UnityEngine.Random.Range(0, i);

            // Save the value of the current i, otherwise it'll overwrite when we swap the values
            SolitaireEngine.Model.Card temp = a[i];

            // Swap the new and old values
            a[i] = a[rnd];
            a[rnd] = temp;
        }

       
        
    }



    private void OnGUI()
    {
#if UNITY_EDITOR


        if (GUILayout.Button("Win Game"))
        {
            managerLogic.isAllowFinish = false;
            // wait when the last card arrived 
            StartCoroutine(runAter(() => OnGameCompleted(), .5f));
        }
#endif
    }
    private void InitViewer()
	{	
		ConstantContainer c = solitaire.GetStackHolderId();
        viewer.Init (c.NULL_CARD, c.DECK_STACK_HOLDER, c.FOUNDATION_STACK_HOLDER, c.TABLEAU_STACK_HOLDER, solitaire.GetStartingCards (), dealContract);
	}
	private void InitHUD ()
	{
		hud = HUDController.instance;
		hud.ResetStatusBars ();
	}
	#region Game Complete Logic
	private void OnGameCompleted ()
	{
		managerLogic.isGameWin = true;
		AddScoreTimeBonus ();
        isBestTime = ((int)managerLogic.timer < StatsSettings.Instance.shortestTime[0] || StatsSettings.Instance.shortestTime[0] == 0);
        managerLogic.SetStopStatsAndSetting ();
        solitaireTimer.Schedule(this, 0.3f, () => { managerLogic.SetStatsScoreAndTime(managerLogic.score, (int)managerLogic.timer, managerLogic.moves); });

        HUDController.instance.VisibleLayout(false);
        if (GameSettings.Instance.isSoundSet) Sound.Instance.Win ();

	 
		PopUpManager.Instance.ShowWin (OnCardDance);

		SocialLogic socialLogic = new SocialLogic ();
		socialLogic.Set ();
	}



	private void AddScoreTimeBonus()
	{
		const float scaler = 10000;
		int bonus = (int) (1f / managerLogic.timer * scaler);
        AddScore(bonus);
		HUDController.instance.SetScore(managerLogic.score); // dependency
	}
	private void OnCardDance ()
	{
		// check setting before animate card
		if (GameSettings.Instance.isEffectSet) // show card dancing effect
		{ 	
			if (GameSettings.Instance.isSoundSet) Sound.Instance.Claps ();
			SolitaireStageViewHelperClass.instance.ShowDance (OnGameCompletedAction);
		}
		else // show win window without card dancing
			OnGameCompletedAction();
	}
	private void OnGameCompletedAction()
	{
        if (GameSettings.Instance.isSoundSet)
        {
        
            Sound.Instance.Shift();
        }

		if (GameSettings.Instance.isCalendarGame)
			PopUpReturnToCalendar ();
		else
			PopUpReturnToMenu();
	}
	#endregion
	// helper
	public bool HasHint
	{
		get
		{
//            Debug.Log("GetHints ().Count " + GetHints().Count);
			return (GetHints ().Count > 0);
		}
	}

	int GetRandomHint()
	{

        List<CardItem> cardItems = AutoPlayGame.instance.AutoPlay();

        if (cardItems.Count == 0)
        {
            CardItem stock = SolitaireStageViewHelperClass.instance.GetCurrentStock;
            if (stock.childCard != null)
            {
                cardItems.Add(stock);
            }
              
        }

        return cardItems[Random.Range(0, cardItems.Count-1)].Id;		
	}
	private List<ContractCommand> GetHints ()
	{
		if (solitaire.GetHints().Count > 0)
			return solitaire.GetHints ();
		
	 
		List<ContractCommand> list_c = new List<ContractCommand> ();
		return list_c;
	}
		
	private IEnumerator ShowSolution ()
	{
		GeneralRestartGame ();
	 
		// TODO remove it and use callback
		// wait card dealt
		yield return new WaitForSeconds (1.6f);

		SolitaireStageViewHelperClass.instance.movementManager.SetMovingSpeed(SmoothMovementManager.Speed.Solution1x);

		List<ICommand> solution_commands = commandBuilder.ConvertContractCommandToICommand (solitaire.GetSolution(), GameSettings.Instance.isOneCardSet);
		// block touches
		HUDController.instance.setTrigger (false, null);

		commandUpdater.ExecuteList (solution_commands, null, PopUpExitSolution);

		managerLogic.isAllowAutoComplete = false; // do not allow autocomplete game in Show Solution Mode.
		managerLogic.isAllowFinish = false; // do not allow finish game in Show Solution Mode.
	}
	#region Update
	private void Update()
	{
		if (GameSettings.Instance.isGameStarted && !managerLogic.isGameWin) // beter bool like isFinished
		{
			IncreaseTimers ();
			CheckFinihUpdate ();
		} 
	}
	private void IncreaseTimers()
	{
		float time = Time.deltaTime;
		managerLogic.timer += time;
		hud.SetTime((int)managerLogic.timer);
 
		
		if (managerLogic.hintTimer.Tick(time))
		{
			// check setting before blink card
			if (!GameSettings.Instance.isAutoHintSet)
				return;

            
            if (managerLogic.isAllowBlinkHint && HasHint)
			{

                SolitaireStageViewHelperClass.instance.DisableBlinkEffect();

               
                int hint = GetRandomHint();

                if (hint != -1) ;
               SolitaireStageViewHelperClass.instance.ShakeCard (SolitaireStageViewHelperClass.instance.FindCardItem( hint));
                
                managerLogic.isAllowBlinkHint = false;
			}
		}
	}

    public void ResetBlinkCard()
    {
        managerLogic.isAllowBlinkHint = true;
    }
    private bool isBestTime = false;
    public bool GetBestTime()
    {
        return isBestTime;
    }
    private void CheckFinihUpdate ()
	{


		if (managerLogic.isAllowAutoComplete &&  IsRuleAprooveAutoComplete())
		{
			managerLogic.isAllowAutoComplete = false;
			// wait when the card finished moving
			StartCoroutine(runAter( ()=>PopUpAutoComplete(), .5f));
		}
		if (managerLogic.isAllowFinish && solitaire.IsComplete ())
		{
			managerLogic.isAllowFinish = false;
			// wait when the last card arrived 
			StartCoroutine(runAter( ()=>OnGameCompleted(), .5f));
		}
	}
	private IEnumerator runAter(UnityAction runnable, float seconds){
		yield return new WaitForSeconds (seconds);
		if(runnable != null)
			runnable ();
	}
	private bool IsRuleAprooveAutoComplete()
	{
		bool isCommunityHasAllOpenCard = solitaire.IsCommunityHasAllOpenCard ();
		bool isDeckHasOpenedCard = (solitaire.IsDeckOpenedAllCards() && solitaire.DeckTotalCardsCount() <= 1) ? true : false;

		return (isCommunityHasAllOpenCard && isDeckHasOpenedCard);
	}
	#endregion

	#region Exit Game
	public void GeneralRestartGame()
	{
		ResetActivity ();
		GameFlowDispatcher.Instance.FromManagerToRestartGame ();
	}
	private void GeneralNewGame()
	{
	 
		ResetActivity ();
		GameFlowDispatcher.Instance.FromManagerToNewGame ();
	}
	private void GeneralBackToMenu()
	{
		ResetActivity ();
		GameFlowDispatcher.Instance.FromManagerToMenu ();
	}
	private void GeneralBackToCalendar()
	{
		ResetActivity ();
		GameSettings.Instance.isGameStarted = false;
		GameFlowDispatcher.Instance.FromManagerToCalendar ();
	}
	private void GeneralBackToSettings()
	{
//		ResetActivity ();
		GameFlowDispatcher.Instance.FromManagerToSettings ();
	}
	private void ResetActivity()
	{
		executor.Reset ();
		commandUpdater.Reset ();
	 
		SolitaireStageViewHelperClass.instance.ResetView ();
	}


    public void AddStatusGame(int score,int timer,int move)
    {
        managerLogic.SetBeginMove(move);
        managerLogic.SetBeginTimer(timer);
        managerLogic.SetScore(score);
    }

    public void AddScore(int score)
    {
        managerLogic.AddScore(score);
    }

    public void ContinueGame(List< int> id, List<int> best_place_id, List<int> parent_id, TurnCardCommand turnCommand, bool addMultiMove ,bool completeRowCard =false)
    {
        MoveBackRowCardCommands moveBackCard = new MoveBackRowCardCommands(true,completeRowCard, 0);
        if (addMultiMove)
        {
          
            for (int i = 0; i < id.Count; i++)
            {
                ICommand move_command = new MoveCardCommand(viewer, id[i], best_place_id[i], parent_id[i], false, false);
              
              
                moveBackCard.AddCommand(move_command);
            }
            if (completeRowCard && turnCommand!=null)
            {
                moveBackCard.AddCommand(turnCommand);
                
             
            }
            executor.Execute(moveBackCard);
        }
        else
        {

            for (int i = 0; i < id.Count; i++)
            {
                ICommand  move_command = new MoveCardCommand(viewer, id[i], best_place_id[i], parent_id[i], false, false);

                CommandsPair commandsPair = new CommandsPair(move_command, turnCommand);
              
             
                executor.Execute(commandsPair);
            }
          
        }

    }
    #endregion
}