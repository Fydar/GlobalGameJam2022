using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickups : MonoBehaviour
{
    public string pickupName;
    public GameObject pickupOBJ;
    public int happyValue;


    public abstract void GrabOBJ();

}
