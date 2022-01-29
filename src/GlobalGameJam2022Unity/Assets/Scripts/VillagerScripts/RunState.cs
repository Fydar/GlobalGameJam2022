using System.Collections;
using UnityEngine;

public class RunState : MonoBehaviour, IState
{
    VillagerAgent villager;



    public Transform runFromObj;

    private void Awake()
    {
        villager = GetComponent<VillagerAgent>();
        

        
    }


    //Uses magnitude to calculate distance between villager and player.
    public IEnumerator DoState()
    {
        Vector3 runAway;

        while(true)
        {
            
            runAway = (transform.position - runFromObj.position).normalized;
            villager.navAI.destination = transform.position + runAway * 5;
            yield return null;
        }
      
        
        //If within range move away quickly.

       // villager.transform.position += new Vector3(2.0f, 0.0f, 0.0f) * Time.deltaTime; //Used for testing purposes.
        
    }

        
 

}
