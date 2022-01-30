using System.Collections;
using UnityEngine;

public class DialogueState : VillageNPCEngineState
{
	public void RunState()
	{
		Engine.ChangeState(this);
	}

	public override IEnumerator DoState()
	{
		Engine.Character.SetPathInput(Quaternion.LookRotation(Vector3.back));

		yield return null;

	}
}
