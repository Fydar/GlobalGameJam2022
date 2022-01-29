using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerAgent : MonoBehaviour
{

    StateMachine states = new StateMachine();
    public float moveSpeed = 2.0f;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        states.ChangeState(new IdleState(this));
    }

    // Update is called once per frame
    void Update()
    {
        states.StateUpdate();
    }
}


