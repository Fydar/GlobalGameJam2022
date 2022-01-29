using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

//State machine.

//States: Idle, Run.
public interface IState
{
    //Enter, Execute and exit the states.
    public void StateEnter();
    public void StateExecute();
    public void StateExit();

}

public class StateMachine 
{

    IState CurrentState;

    public void ChangeState(IState NewState)
    {
        if (CurrentState != null)
        {
            CurrentState.StateExit();
        }

        CurrentState = NewState;
        CurrentState.StateEnter();
    }

    public void StateUpdate()
    {
        if (CurrentState != null)
        {
            CurrentState.StateExecute();
        }
    }
}
