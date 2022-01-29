using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class VillageCharacterController : MonoBehaviour
{
	public enum MovementMode
	{
		Idle,
		Control,
		Path
	}

	public CharacterTemplate Character;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private float animatorRunningVelocity = 3.0f;

	private NavMeshAgent agent;

	private MovementMode movementMode;
	private Vector2 inputMovement;
	private Vector3 inputDestination;

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		switch (movementMode)
		{
			case MovementMode.Control:
			{
				if (inputMovement != Vector2.zero)
				{
					var worldSpaceMovement = new Vector3(inputMovement.x, 0.0f, inputMovement.y);
					var worldSpaceMovementDirection = worldSpaceMovement.normalized;

					agent.isStopped = false;
					agent.SetDestination(transform.position + worldSpaceMovementDirection * 0.65f);
				}
				else
				{
					if (!agent.isStopped)
					{
						agent.isStopped = true;
					}
				}
				movementMode = MovementMode.Idle;
				break;
			}
			case MovementMode.Path:
			{
				if (agent.destination != inputDestination)
				{
					agent.isStopped = false;
					agent.SetDestination(inputDestination);
				}
				break;
			}
			case MovementMode.Idle:
			{
				if (!agent.isStopped)
				{
					agent.isStopped = true;
				}
				break;
			}
		}

		animator.SetFloat("Speed", Mathf.Clamp01(agent.velocity.magnitude / animatorRunningVelocity));
	}

	public void SetControllerInput(Vector2 movement)
	{
		movementMode = MovementMode.Control;
		inputMovement = movement;
	}

	public void SetPathInput(Vector3 destination)
	{
		movementMode = MovementMode.Path;
		inputDestination = destination;
	}

	public void SetIdle()
	{
		movementMode = MovementMode.Idle;
	}
}