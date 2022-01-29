using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VillagerAgent : MonoBehaviour
{

    StateMachine states = new StateMachine();
    public float moveSpeed = 2.0f;
    public int happiness;
    public bool isTalking;

    public IdleState idle;
    public RunState run;
    public DialogueState dialogue;

    public NavMeshAgent navAI;

    private void Awake()
    {
        idle = gameObject.GetComponent<IdleState>();
        run = gameObject.GetComponent<RunState>();
        dialogue = gameObject.GetComponent<DialogueState>();
    }

    // Start is called before the first frame update
    void Start()
    {
        states.ChangeState(GetComponent<DialogueState>());
        isTalking = true;
        navAI = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        states.StateUpdate();

        //This works once, but nonetheless does.
        if(isTalking != true)
        {
            states.ChangeState(GetComponent<IdleState>());
        }

        
    }
}


