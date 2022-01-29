using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MonoBehaviour, IState
{
    VillagerAgent villager;
    
    //For random nav mesh position?
    public float navRange = 10.0f;

    [SerializeField]
    private List<Transform> movPosTrans = new List<Transform>();

    private void Awake()
    {
        villager = GetComponent<VillagerAgent>();
    }


    public IEnumerator DoState()
    {
        while(true)
        {


            var newPosition = movPosTrans[Random.Range(0, movPosTrans.Count)];
            villager.navAI.destination = newPosition.position;


            while (Vector3.Distance(transform.position, villager.navAI.destination) > 0.8f)
            {
                yield return null;
            }
            
            yield return new WaitForSeconds(3.0f);
        }

        


    }

}
