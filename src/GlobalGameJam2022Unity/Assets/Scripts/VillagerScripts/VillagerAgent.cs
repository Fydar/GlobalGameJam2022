using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerAgent : MonoBehaviour
{

    public Vector3 velocity;
    public float mass;
    public float maxSpeed;
    public float maxForce;

    AIWander wander;


    private void Awake()
    {
        wander = gameObject.GetComponent<AIWander>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        wander.steerCalc();
    }
}
