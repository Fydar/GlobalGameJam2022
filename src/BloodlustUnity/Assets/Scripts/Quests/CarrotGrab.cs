using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotGrab : Pickups
{
    // Start is called before the first frame update
    void Start()
    {
        pickupName = "Carrot";
        happyValue = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void GrabOBJ()
    {
        Destroy(this);
    }
}
