namespace ManagerUtils
{
	using System.Collections.Generic;
	using SolitaireEngine;
	using SolitaireEngine.Model;
	public class CommandBuilder
	{
		private Solitaire solitaire;
		private IManagerBaseCommands manager;
		private IViewBaseCommands viewer;
		public CommandBuilder(Solitaire solitaire, IManagerBaseCommands manager, IViewBaseCommands viewer)
		{
			this.solitaire = solitaire;
			this.manager = manager;
			this.viewer = viewer;
		}
		private const int LOW_PAYMENT = 5;
		private const int HI_PAYMENT = 10;
        #region Create ICommand
        // **************** COMMANDS BUILDER ***************
        public ICommand CreateMoveCardAndTryOpenParentCommand(int id_who, int id_where, int id_view_parent, bool anim = false, bool moveCurve = false,bool openCardParent =true)
		{
			int id_solitaire_parent = solitaire.GetParentID(id_who);
           
			ICommand command_engine_move = new MoveCardSolitCommand (manager, id_who, id_where, id_solitaire_parent);
          
			ICommand command_view_move = new MoveCardCommand (viewer, id_who, id_where, id_view_parent,moveCurve, anim);
			ICommand command_engine;
			ICommand command_view;
			
			int shouldOpenDownCardId = solitaire.ShouldOpenDownCard(id_who);
            CardItem[] cardItems = MonoHandler.FindObjectsOfType<CardItem>();
            bool visible = false;
            for (int i = 0; i < cardItems.Length; i++)
            {
                if (cardItems[i].Id == id_view_parent)
                {
                    visible = cardItems[i].isOppened;
                    break;
                }

            }

            if (shouldOpenDownCardId.Equals(-1))
            {
                ICommand command_engine_open = new TurnCardSolitCommand(manager, shouldOpenDownCardId);
                command_engine = new CommandsPair(command_engine_move, command_engine_open);

              

                TurnCardCommand command_view_open = new TurnCardCommand(viewer, id_view_parent, visible);
                if (!openCardParent)
                {
                    command_view_open.DisableOpen();
                }



                command_view = new CommandsPair(command_view_move, command_view_open);


            }

            else
            {
                command_engine = command_engine_move;
                command_view = command_view_move;

            }
        
			
			ICommand command_move = new CommandsPair (command_engine, command_view);
			
			ICommand scoring_command = null;
		 
			 
			
			ICommand main_command;
			if (scoring_command == null) main_command = command_move;
			else main_command = new CommandsPair (command_move, scoring_command);




			return main_command;
		}
		public ICommand CreateShiftDeckCommand()
		{
			ICommand command_engine = new ShiftDeckSolitCommand (manager);
			ICommand command_view = new ShiftDeckCommand(viewer);
			ICommand command_shift_deck = new CommandsPair (command_engine, command_view);
			return command_shift_deck;
		}

		public ICommand CreateShiftDeckCommand(int times)
		{
			ICommand shiftDeckOnce = CreateShiftDeckCommand ();
			ICommand shiftDeckTwice = new CommandsPair (CreateShiftDeckCommand (),CreateShiftDeckCommand ());
			ICommand shiftDeckThreeTimes = new CommandsPair (shiftDeckOnce, shiftDeckTwice);

			ICommand command_shift_deck = shiftDeckOnce;
			switch (times) 
			{
			case 1:
				command_shift_deck = shiftDeckOnce;
				break;
			case 2:
				command_shift_deck = shiftDeckTwice;
				break;
			case 3:
				command_shift_deck = shiftDeckThreeTimes;
				break;
			default:
				break;
			}
			return command_shift_deck;
		}
			
		public ICommand CreateTurnDeckCommand()
		{
			ICommand command_engine = new TurnDeckSolitCommand (manager);
			ICommand command_view = new TurnDeckCommand(viewer);
			ICommand command_turn_deck = new CommandsPair (command_engine, command_view);
			return command_turn_deck;
		}
		public ICommand CreateUnitarMoveCommand (int id_who, int id_where, int id_view_parent, bool anim = false)
		{
			int id_solitaire_parent = solitaire.GetParentID(id_who);
             
            ICommand command_engine_move = new MoveCardSolitCommand (manager, id_who, id_where, id_solitaire_parent);
			ICommand command_view_move = new MoveCardCommand (viewer, id_who, id_where, id_view_parent, anim);
			ICommand command_move = new CommandsPair (command_engine_move, command_view_move);
			return command_move;
		}
		public ICommand CreateUnitarTurnCardCommand (int id_who)
		{
			ICommand command_engine_turn = new TurnCardSolitCommand (manager, id_who);
			ICommand command_view_turn = new TurnCardCommand (viewer, id_who,true);
			ICommand command_turn = new CommandsPair (command_engine_turn, command_view_turn);
			return command_turn;
		}
		#endregion
		private bool IsIdInThronOrThronBase(int id)
		{
			return solitaire.IsIdInThronOrThronBase (id);
		}
		private bool IsIdInCommunityOrCommunityBase(int id)
		{
			return solitaire.IsIdInCommunityOrCommunityBase (id);
		}
		private bool GetThronCardMove (int id)
		{
			return solitaire.GetThronCardMove (id);
		}
		private bool GetCommunityCardMove (int id)
		{
			return solitaire.GetCommunityCardMove (id);
		}
		#region Convert
		public List<ICommand> ConvertContractCommandToICommand (List<ContractCommand> contractComands, bool isOneCard)
		{
			List<ICommand> convertedCommands = new List<ICommand>();
			foreach (ContractCommand element in contractComands)
			{
				ICommand command = null;
				switch (element.Action)
				{
				case ContractCommand.State.Move:
					// TODO remove it
					int parentForUndo = SolitaireStageViewHelperClass.instance.getCardById (element.IdFrom).parentCard.Id;
					command = CreateUnitarMoveCommand (element.IdFrom, element.IdTo, parentForUndo, true);
					break;

				case ContractCommand.State.TurnCard:
					command = CreateUnitarTurnCardCommand(element.IdFrom);
					break;		

				case ContractCommand.State.ShiftDeckOnece:
					command = CreateShiftDeckCommand ();
					break;	
				
				case ContractCommand.State.ShiftDeckTwice:
					if (isOneCard)
					{
						command = CreateShiftDeckCommand ();
						convertedCommands.Add(command);
						command = CreateShiftDeckCommand ();
						convertedCommands.Add(command);
						command = null;
					}
					else
						command = CreateShiftDeckCommand (2);
					break;	
				case ContractCommand.State.ShiftDeckThird:
					if (isOneCard)
					{
						command = CreateShiftDeckCommand ();
						convertedCommands.Add(command);
						command = CreateShiftDeckCommand ();
						convertedCommands.Add(command);
						command = CreateShiftDeckCommand ();
						convertedCommands.Add(command);
						command = null;
					}
					else
						command = CreateShiftDeckCommand (3);
					break;	

				case ContractCommand.State.ReverseDeck:
					command = CreateTurnDeckCommand ();
					break;	

				default: 
					break;
				}
				if(command != null)
					convertedCommands.Add(command);
			}
			return convertedCommands;
		}

		public List<ICommand> ConvertAutoCompleteContractCommandToICommand (List<ContractCommand> contractComands, bool isStandardGame)
		{
			List<ICommand> convertedCommands = new List<ICommand>();
			foreach (ContractCommand element in contractComands)
			{
				ICommand card_command = null;
				ICommand payout_command = null;
//				UnityEngine.Debug.Log (element.Action + "/" + element.IdFrom + "->" + element.IdTo );

				switch (element.Action)
				{
				case ContractCommand.State.Move:
					// TODO remove it
					int parentForUndo = SolitaireStageViewHelperClass.instance.getCardById (element.IdFrom).parentCard.Id;
					card_command = CreateUnitarMoveCommand (element.IdFrom, element.IdTo, parentForUndo, true);
					int payment = (isStandardGame) ? HI_PAYMENT : LOW_PAYMENT;
					//payout_command = new ScoringCommand (manager, payment);
					break;

				case ContractCommand.State.TurnCard:
					card_command = CreateUnitarTurnCardCommand(element.IdFrom);
					break;		

				case ContractCommand.State.ShiftDeckOnece:

					card_command = CreateShiftDeckCommand ();
					break;	

				case ContractCommand.State.ShiftDeckTwice:
					card_command = CreateShiftDeckCommand (2);
					break;	
				case ContractCommand.State.ShiftDeckThird:
					card_command = CreateShiftDeckCommand (3);
					break;	


				case ContractCommand.State.ReverseDeck:
					card_command = CreateTurnDeckCommand ();
					break;	

				default: 
					break;
				}
				if (card_command != null)
				{
					ICommand main_command =  new CommandsPair (card_command, payout_command);
					convertedCommands.Add(main_command);
				}
			}
			return convertedCommands;
		}
		#endregion
	}
}