using UnityEngine;

public class RunState : MonoBehaviour, IState
{
    VillagerAgent villager;

    private void Start()
    {
        villager = GetComponent<VillagerAgent>();
    }

    public void StateEnter()
    {
        
    }
    public void StateExecute()
    {
        
        villager.transform.position += new Vector3(2.0f, 0.0f, 0.0f) * Time.deltaTime; //Used for testing purposes.
        
    }
    public void StateExit()
    {
        
    }

}
