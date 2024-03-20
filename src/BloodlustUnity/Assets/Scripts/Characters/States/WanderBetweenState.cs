using System.Collections;
using System.Linq;
using UnityEngine;

public class WanderBetweenState : VillageNPCEngineState
{
	[SerializeField] private Transform[] idleLocations;

	[SerializeField] private float minimumWaitTime = 3.0f;
	[SerializeField] private float maximumWaitTime = 5.0f;

	private Transform currentTarget;

	public void RunState()
	{
		Engine.ChangeState(this);
	}

	public override IEnumerator DoState()
	{
		while (true)
		{
			// Don't go to the same location twice and don't go to a "null" waypoint.
			var selection = idleLocations
				.Where(location => location != null && location != currentTarget)
				.ToArray();

			var newIdleLocation = selection[Random.Range(0, selection.Length)];

			currentTarget = newIdleLocation;

			Engine.Character.SetPathInput(newIdleLocation.position, newIdleLocation.rotation);

			while (Engine.Character.MovementMode == MovementMode.Path)
			{
				yield return null;
			}

			yield return new WaitForSeconds(Random.Range(minimumWaitTime, maximumWaitTime));
		}
	}
}
