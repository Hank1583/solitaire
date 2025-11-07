using System.Collections.Generic;
using SolitaireEngine;
using SolitaireEngine.Model;
using UserWindow;


public partial class StageManager
{

	#region Debug and Test
	public void OnComplete80()
	{
		List<ICommand> solution_commands = commandBuilder.ConvertContractCommandToICommand (solitaire.GetSolution(), GameSettings.Instance.isOneCardSet);
		int count = solution_commands.Count - 5;

		for(int i = 0; i < count; i++){
			executor.Execute (solution_commands [i]);
			 
		}
		// TODO: fix undo after that code
	}
	#endregion

	#region HUD Actions
	// back menu/shop
	void IHUDActions.OnBackToMenuPressed()
	{
		if (GameSettings.Instance.isCalendarGame) LoadCalendarScene ();
		else GeneralBackToMenu ();
	}
	void IHUDActions.OnStorePressed()
	{
		ShowStorePopUp ();
	}
	void IHUDActions.OnShowManualPressed()
	{
		PopUpManager.Instance.ShowManual ();
	}
		

	// bottom buttons
	void IHUDActions.OnOptionsPressed()
	{
		GeneralBackToSettings ();
	}
	void IHUDActions.OnSolutionPressed()
	{
		if (GameSettings.Instance.isSoundSet) Sound.Instance.Up ();
		viewer.UnblinkAll ();
		StartCoroutine(ShowSolution ());
	}

	 void IHUDActions.OnRestartPressed()
	{
		//if (GameSettings.Instance.isSoundSet) Sound.Instance.Distribute ();
		GeneralRestartGame();
	}
	void IHUDActions.OnNewGamePressed()
	{
        if (GameSettings.Instance.isSoundSet) Sound.Instance.Up();
       
		GeneralNewGame ();
	}
	void IHUDActions.OnDailyChallengePressed()
	{
		LoadCalendarScene ();
	}
	void IHUDActions.OnStatisticPressed()
	{
		PopUpManager.Instance.ShowStats ();
	}

	void IHUDActions.OnHintsPressed()
	{
		if (GameSettings.Instance.isSoundSet) Sound.Instance.Creack ();
		 managerLogic.StartCountGame();
		// TODO remove it
	 
		commandUpdater.Reset();
		List<ICommand> listToExecute = new List<ICommand> ();

        foreach (ContractCommand element in GetHints())
        {

            if (element.Action.Equals(ContractCommand.State.ShiftDeckOnece))
            {
            
                viewer.BlinkCard(element.IdFrom);
                managerLogic.isAllowBlinkHint = false;
                return;
            }

            ICommand command = new MoveHintCommand(viewer, element.IdFrom, element.IdTo);
            listToExecute.Add(command);
        }
		SolitaireStageViewHelperClass.instance.movementManager.SetMovingSpeed (SmoothMovementManager.Speed.Hint);


		HUDController.instance.setTrigger(false, () => {
			commandUpdater.Reset();
		 
			HUDController.instance.triggerLess.SetActive(false);
		});

		commandUpdater.ExecuteList (listToExecute, null, () => {

			HUDController.instance.triggerLess.SetActive(false);
		});

		SocialLogic socialLogic = new SocialLogic ();
		socialLogic.Hint ();
	}
	void IHUDActions.OnUndoPressed()
	{
        SolitaireStageViewHelperClass.instance.DisableBlinkEffect();
 
        
        CardStockSlots.instance.undo = true;
        if (GameSettings.Instance.isSoundSet)
        {
            Sound.Instance.Up();
        }
        SolitaireStageViewHelperClass.instance.SetAllDistanceBetweenCard(false);
        if (executor.HasCommands())
		{
			SmoothMovementManager.instance.ClearMoveWhenUndo();
			// TODO: remove it
			SolitaireStageViewHelperClass.instance.movementManager.SetMovingSpeed (SmoothMovementManager.Speed.Undo);
          
            
			 executor.Unexecute();

            if (GameSettings.Instance.isSoundSet)
            {

                Sound.Instance.Shift();
            }
           managerLogic.AddMoves ();
        }

        
	}
	void IHUDActions.OnNoUsefulHintsPressed()
	{
		managerLogic.isGameWin = false;
		managerLogic.SetStopStatsAndSetting ();
	 
		OnGameCompletedAction ();
	}

	// solution
	void IHUDActions.OnExitSolution()
	{
		// pause executing solution commands
		StageManager.instance.commandUpdater.Pause();

		PopUpExitSolution ();
	}
	#endregion


	#region Actions received from view
	void ICardActions.OnClickCard (CardItem card)
	{

		managerLogic.StartCountGame ();
		// check from settings
		if (!GameSettings.Instance.isAutoTapMoveSet)
			return;


       

        if (solitaire.HasBetterPlace(card.Id))
		{
			 
			SolitaireStageViewHelperClass.instance.movementManager.SetMovingSpeed(SmoothMovementManager.Speed.Move);

			int best_place_id = solitaire.GetBetterPlace (card.Id);

			ICommand move_command = commandBuilder.CreateMoveCardAndTryOpenParentCommand (card.Id, best_place_id, card.parentCard.Id, true);
            ContinueModeGame.instance.AddDataCard(card.Id, best_place_id, card.parentCard.Id ,SolitaireStageViewHelperClass.instance.FindCardItem(card.parentCard.Id).isOppened);
 
          

            executor.Execute(move_command);


			managerLogic.AddMoves ();

		}
		else
		{
			if (GameSettings.Instance.isSoundSet) Sound.Instance.MissCard ();
			ICommand command = new ShakeCardCommand(viewer, card.Id);
			executor.Execute(command, false);
		}

	}

    ICommand ICardActions.CompleteCardSpider(int id, int parent_id,int best_place_id,bool moveCurve) 
    {

        managerLogic.StartCountGame();
 

        SolitaireStageViewHelperClass.instance.movementManager.SetMovingSpeed(SmoothMovementManager.Speed.Move);
        ICommand move_command = commandBuilder.CreateMoveCardAndTryOpenParentCommand(id, best_place_id, parent_id, true,moveCurve);
        managerLogic.AddMoves();
        return move_command;

    

     

         

    }

    void ICardActions.OnDropCard (int id, int parent_id, List<int> near_ids ,CardItem cardItem)
	{
		 
		if (cardItem.childCard == null || cardItem.GetComponentInParent<CardHasChild>() ==null)
        {
		 
			managerLogic.StartCountGame();
            foreach (int near_id in near_ids)
            {
#if UNITY_EDITOR
                if (near_id == id || near_id == parent_id || near_id.Equals(Data.NULL_CARD))
                {

                    throw new UnityEngine.UnityException("Invalid near card!");
                
                }
#endif

				 
				if (solitaire.CanAttach(id, near_id))
                {
                    
                    ContinueModeGame.instance.AddDataCard(id, near_id, parent_id, SolitaireStageViewHelperClass.instance.FindCardItem(parent_id).isOppened);
					 

					SolitaireStageViewHelperClass.instance.movementManager.SetMovingSpeed(SmoothMovementManager.Speed.Move) ;
                    ICommand move_command = commandBuilder.CreateMoveCardAndTryOpenParentCommand(id, near_id, parent_id, true);
                    
                    executor.Execute(move_command);
                    managerLogic.AddMoves();

                   // SolitaireSpiderCheck.instance.CompleteSpider(cardItem);
                    return;
                }
            }
        }



		SolitaireStageViewHelperClass.instance.AddScoreMove = false;
        solitaireTimer.Schedule(this, 0.1f, () => {

			SolitaireStageViewHelperClass.instance.AddScoreMove = true;
		});
		// move back it no cards found to be attached
		if (GameSettings.Instance.isSoundSet) Sound.Instance.MissCard ();
		 
		SolitaireStageViewHelperClass.instance.prevMoveCardInDeck = false;
		ICommand command = new MoveCardCommand (viewer, id, parent_id, -1);

        
		executor.Execute(command, false);


	}
	void ICardActions.OnClickDeck ()
	{
		if (!solitaire.IsDeckEmpty ())
		{
			managerLogic.StartCountGame ();
			SolitaireStageViewHelperClass.instance.movementManager.SetMovingSpeed(SmoothMovementManager.Speed.Deck);
		
			ICommand shiftDeckCommand;
			if (GameSettings.Instance.isOneCardSet)
			{
				shiftDeckCommand = commandBuilder.CreateShiftDeckCommand();
			}
			else 
			{
				int cardsInDeckCount = solitaire.DeckRestCardsCount();
				int deckTurnCount = (cardsInDeckCount>3) ? 3 : cardsInDeckCount;
				shiftDeckCommand = commandBuilder.CreateShiftDeckCommand (deckTurnCount);
			}
			executor.Execute(shiftDeckCommand);
			managerLogic.AddMoves ();
		}
	}

	void ICardActions.OnTurnDeck ()
	{
		if (!solitaire.IsDeckEmpty ())
		{
			managerLogic.StartCountGame ();
			if (!GameSettings.Instance.isStandardSet && !managerLogic.HasDeckTurn)  //countDeckTurn >= DECK_TURN_LIMIT)
			{
				Sound.Instance.MissCard ();
				return;
			} 
			ICommand command_turn_deck = commandBuilder.CreateTurnDeckCommand ();
           
			executor.Execute(command_turn_deck);
			managerLogic.AddMoves ();
		}
	}
	void ICardActions.OnClickAnywhere ()
	{
		viewer.UnblinkAll ();
		managerLogic.StartCountGame ();		
	}
	#endregion


	#region Actions received from command
	public void Move (int idFrom, int idTo)
	{

       
		solitaire.Move (idFrom, idTo);
	}
	public void TurnCard (int id)
	{
		solitaire.TurnCard (id);
	}
	public void ShiftDeck ()
	{
		if (GameSettings.Instance.isSoundSet) Sound.Instance.Foley ();
		solitaire.ShiftDeck ();
	}
	public void UndoShiftDeck ()
	{
		if (GameSettings.Instance.isSoundSet) Sound.Instance.Foley ();
		solitaire.UndoShiftDeck ();
	}
	public void ReverseDeck (bool isOpenCard)
	{
		if (isOpenCard) {
			if (GameSettings.Instance.isSoundSet) Sound.Instance.Up ();
			managerLogic.countDeckTurn--;
		}
		else
		{
			if (GameSettings.Instance.isSoundSet) Sound.Instance.Down ();
			managerLogic.countDeckTurn++;
		}

		bool isTurnImage = (GameSettings.Instance.isStandardSet || (!GameSettings.Instance.isStandardSet  && managerLogic.HasDeckTurn));
		 

		solitaire.ReverseDeck (isOpenCard);
	}
	public void Score (int score)
	{
		managerLogic.AddScore (score);
	}
	public void SetThronCardMove (int id, bool isMoved)
	{
		solitaire.SetThronCardMove (id, isMoved);
	}
	public void SetCommunityCardMove (int id, bool isMoved)
	{
		solitaire.SetCommunityCardMove (id, isMoved);
	}
	#endregion

	void AutoCompleteGame()
	{
		managerLogic.isAllowBlinkHint = false;
		viewer.UnblinkAll ();

		SolitaireStageViewHelperClass.instance.movementManager.SetMovingSpeed (SmoothMovementManager.Speed.AutoComplete);

		var auto_complete_commands = commandBuilder.ConvertAutoCompleteContractCommandToICommand (
			solitaire.GetCurrentSolution (GameSettings.Instance.isOneCardSet), 
			GameSettings.Instance.isStandardSet);

		// block touches
		HUDController.instance.setTrigger (false, null);

		commandUpdater.ExecuteList (auto_complete_commands, managerLogic.AddMoves, null);

      
    }



   

    public ICommand  AddMoveCard(int id, int best_place_id, int parent_id )
    {
        ICommand move_command = commandBuilder.CreateMoveCardAndTryOpenParentCommand(id, best_place_id, parent_id, true,false);
        return move_command;
    }





	// TODO: move it to shop builder

	#region PopUpWindow
	private void ShowStorePopUp()
	{
		string titleLineData = "STORE";
		string listLinesData = "Here you can remove ads"+"\n"+"and restore purchase.";

		// Build store pop up
		List<ResultButtonData> buttonData = new List<ResultButtonData> ();
		buttonData.Add (new ResultButtonData ("Remove ads", 1, OnShopRemove));
		buttonData.Add (new ResultButtonData ("Restore purchase", 0, OnShopRestore));
		buttonData.Add (new ResultButtonData ("Cancel", 2, OnShopCancel));

		// show store pop up
		PopUpManager.Instance.ShowDialog (titleLineData, listLinesData, buttonData);
	}
	private void OnShopRemove()
	{
		// TODO: Remove ads
		PopUpManager.Instance.Close ();
	}
	private void OnShopRestore()
	{
		// TODO: Restore purchase
		PopUpManager.Instance.Close ();
	}
	private void OnShopCancel()
	{
		PopUpManager.Instance.Close ();
	}
	#endregion
}