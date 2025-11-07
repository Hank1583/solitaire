using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class MoveBackRowCardCommands : ICommand
{
    private List<ICommand> commands = new List<ICommand>();
    private float waitTimeMoveCurve;
    private bool complete = false;
    private bool isExecute = false;
    public MoveBackRowCardCommands(bool  Execute,  bool complete,float waitTimeMoveCurve)
    {
        this.complete = complete;
        this.isExecute = Execute;
        commands.Clear();
        this.waitTimeMoveCurve = waitTimeMoveCurve;
    }

    public void AddCommand(ICommand command)
    {
        commands.Add(command);
    }


    public bool Complete
    {
        get
        {
            return complete;
        }
    }

    public int Length()
    {
        return commands.Count;
    }

   public void execute()
    {
       
          
        if (waitTimeMoveCurve <= 0)
        {
            for (int i = commands.Count - 1; i >= 0; i--)
            {

                commands[i].execute();
            }
        }
        else
        {
            SolitaireStageViewHelperClass.instance.ActiveMovecurve(commands, waitTimeMoveCurve);
        }

    }

  

    public void unexecute()
    {
       
        
        SolitaireSpiderCheck.instance.completeRowCard = !complete;
     
        for (int i = commands.Count-1; i >=0; i--)
        {
           
            commands[i].unexecute();
        }
 


        if (complete)
        {
            SolitaireSpiderCheck.instance.Undo();
        }
        else
        {
            SolitaireStageViewHelperClass.instance.UpStockDealCard();
        }
     
        SolitaireStageViewHelperClass.instance.OffAllCardInContainerCard();
 
    }


     
}
