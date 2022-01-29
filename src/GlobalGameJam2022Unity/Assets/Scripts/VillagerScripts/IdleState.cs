using UnityEngine;

public class IdleState : MonoBehaviour, IState
{
    VillagerAgent villager;
    
    //For random nav mesh position?
    public float navRange = 10.0f;

    [SerializeField]
    private Transform movPosTrans;

    private void Start()
    {
        villager = GetComponent<VillagerAgent>();
    }

    public void StateEnter()
    {
        
    }
    public void StateExecute()
    {

        

    }
    public void StateExit()
    {
        
    }

   
}
