using System.Collections;
using UnityEngine;

public class FleeState : VillageNPCEngineState
{
	private Transform fleeingFrom;
	private float previousSpeed = 0;

	public float SpeedMultipler = 2.0f;

	public void RunState(Transform fleeingFrom)
	{
		this.fleeingFrom = fleeingFrom;

		Engine.ChangeState(this);
	}

	public override IEnumerator DoState()
	{
		previousSpeed = Engine.Character.Navigation.speed;
		Engine.Character.Navigation.speed *= SpeedMultipler;

		while (true)
		{
			if (fleeingFrom != null)
			{
				Vector3 runDirection = (transform.position - fleeingFrom.position).normalized;

				Engine.Character.SetPathInput(transform.position + (runDirection * 5));

				// Don't update pathfinding too often for optimization
				yield return new WaitForSeconds(0.25f);
			}
		}
	}

	public override void OnExit()
	{
		Engine.Character.Navigation.speed = previousSpeed;
	}
}
