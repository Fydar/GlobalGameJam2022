using UnityEngine;
//Needs nothing, used for when player is talking to villager.
public class DialogueState : MonoBehaviour, IState
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
        
    }
    public void StateExit()
    {
        
    }
}
