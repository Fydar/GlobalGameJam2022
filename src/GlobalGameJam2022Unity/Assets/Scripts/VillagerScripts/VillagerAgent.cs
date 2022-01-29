using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VillagerAgent : MonoBehaviour
{

    StateMachine states;
    public float moveSpeed = 2.0f;
    public int happiness;
    public bool isTalking;

    private IdleState idle;
    private RunState run;
    private DialogueState dialogue;

    public NavMeshAgent navAI;

    Vector3 storedPos;

    private void Awake()
    {
        idle = gameObject.GetComponent<IdleState>();
        run = gameObject.GetComponent<RunState>();
        dialogue = gameObject.GetComponent<DialogueState>();
        navAI = GetComponent<NavMeshAgent>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        states = new StateMachine(idle, this);
        //states.ChangeState(GetComponent<DialogueState>());
        isTalking = true;

        storedPos = transform.position;
       
    }

    // Update is called once per frame
    void Update()
    {
        
        /*
        //This works once, but nonetheless does.
        if(isTalking != true)
        {
            states.ChangeState(GetComponent<IdleState>());
        }
        */
        
    }

    public void dayReset()
    {
        transform.position = storedPos;
        navAI.destination = transform.position;
        states.ChangeState(idle);
    }
}


