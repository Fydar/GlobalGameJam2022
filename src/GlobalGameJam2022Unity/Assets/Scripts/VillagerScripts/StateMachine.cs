using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//State machine.

//States: Idle, Run.
public interface IState
{
    //Enter, Execute and exit the states.
    public IEnumerator DoState();

}

public class StateMachine 
{

    IState CurrentState;
    VillagerAgent villager;

    Coroutine coroutine;

    public StateMachine(IState currentState, VillagerAgent villager)
    {
        CurrentState = currentState;
        this.villager = villager;

        ChangeState(currentState);
    }

    public void ChangeState(IState NewState)
    {
        if (coroutine != null)
        {
            villager.StopCoroutine(coroutine);
            coroutine = null;
        }
        coroutine = villager.StartCoroutine(CurrentState.DoState());

        
    }

}
