using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//State machine.

//States: Idle, Run.
public interface IState
{
    public void StateEnter();
    public void StateExecute();
    public void StateExit();

}

public class StateMachine : MonoBehaviour
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

public class IdleState : IState
{
    VillagerAgent villager;

    public IdleState(VillagerAgent villager)
    {
        this.villager = villager;
    }

    public void StateEnter()
    {
        Debug.Log("Entering...");
    }
    public void StateExecute()
    {
        Debug.Log("Updating...");
        

    }
    public void StateExit()
    {
        Debug.Log("Exiting...");
    }
}

public class RunState : IState
{
    VillagerAgent villager;

    public RunState(VillagerAgent villager)
    {
        this.villager = villager;
    }

    public void StateEnter()
    {
        Debug.Log("Entering...");
    }
    public void StateExecute()
    {
        Debug.Log("Updating...");
        
    }
    public void StateExit()
    {
        Debug.Log("Exiting...");
    }
}
