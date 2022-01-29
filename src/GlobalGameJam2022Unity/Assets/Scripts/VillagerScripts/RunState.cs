using System.Collections;
using UnityEngine;

public class RunState : MonoBehaviour, IState
{
    VillagerAgent villager;

    private void Start()
    {
        villager = GetComponent<VillagerAgent>();
    }


    public IEnumerator DoState()
    {
        

        villager.transform.position += new Vector3(2.0f, 0.0f, 0.0f) * Time.deltaTime; //Used for testing purposes.
        yield return null;
    }

        
 

}
