using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWander : MonoBehaviour
{

    public float wanderRadius = 10.0f;
    public float wanderDistance = 10.0f;
    public float wanderJitter = 1.0f;
    Vector3 wanderTarget = Vector3.zero;
    float wanderAngle = 0.0f;

    public bool wanderEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        wanderAngle = Random.Range(0.0f, Mathf.PI * 2);
        wanderTarget = new Vector3(Mathf.Cos(wanderAngle), 0, Mathf.Sin(wanderAngle)) * wanderRadius;
    }

    public Vector3 steerCalc()
    {

        wanderAngle += Random.Range(-wanderJitter, wanderJitter);
        Vector3 targetLocal = wanderTarget;
        Vector3 targetWorld = transform.position + wanderTarget;

        targetWorld += transform.forward * wanderDistance;

        return targetWorld - transform.position;

    }
}
