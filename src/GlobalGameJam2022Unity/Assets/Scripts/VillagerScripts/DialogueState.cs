using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Needs nothing, used for when player is talking to villager.
public class DialogueState : MonoBehaviour, IState
{

    VillagerAgent villager;
    private void Start()
    {
        villager = GetComponent<VillagerAgent>();
    }



    public IEnumerator DoState()
    {
        
        yield return null;

    }
}
