using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class VillageCharacter : MonoBehaviour
{
	[Header("Personality")]
	public CharacterPersonality Personality;

	[Header("Animation")]
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private float standingRotationSpeed = 6.0f;

	[SerializeField]
	private float speedDampening = 0.2f;

	[SerializeField]
	private float animatorRunningVelocity = 3.0f;

	private NavMeshAgent navigation;

	private MovementMode movementMode;
	private Vector2 inputMovement;
	private Vector3? inputDestinationPosition;
	private Quaternion? inputDestinationRotation;

	public MovementMode MovementMode => movementMode;
	public NavMeshAgent Navigation => navigation;

	private void Awake()
	{
		navigation = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		switch (movementMode)
		{
			case MovementMode.Control:
			{
				if (inputMovement == Vector2.zero)
				{
					if (!navigation.isStopped)
					{
						navigation.isStopped = true;
					}
				}
				else
				{
					var worldSpaceMovement = new Vector3(inputMovement.x, 0.0f, inputMovement.y);
					var worldSpaceMovementDirection = worldSpaceMovement.normalized;

					navigation.isStopped = false;

					navigation.SetDestination(navigation.transform.position + worldSpaceMovementDirection * 0.65f);

					inputMovement = Vector2.zero;
				}
				movementMode = MovementMode.Idle;
				break;
			}
			case MovementMode.Path:
			{
				if (inputDestinationPosition != null)
				{
					navigation.isStopped = false;
					navigation.SetDestination(inputDestinationPosition.Value);
					inputDestinationPosition = null;
				}

				if (Vector3.Distance(navigation.transform.position, navigation.destination) < 0.05f)
				{
					navigation.isStopped = true;
				}

				if (navigation.isStopped)
				{
					if (inputDestinationRotation != null)
					{
						navigation.transform.rotation = Quaternion.Slerp(
							navigation.transform.rotation,
							inputDestinationRotation.Value,
							standingRotationSpeed * Time.deltaTime);

						if (Quaternion.Angle(navigation.transform.rotation, inputDestinationRotation.Value) < 1.0f)
						{
							movementMode = MovementMode.Idle;
						}
					}
					else
					{
						movementMode = MovementMode.Idle;
					}
				}
				break;
			}
			case MovementMode.Idle:
			{
				if (!navigation.isStopped)
				{
					navigation.isStopped = true;
				}
				break;
			}
		}

		float currentSpeed = animator.GetFloat("Speed");
		animator.SetFloat("Speed",
			Mathf.Lerp(currentSpeed,
			Mathf.Clamp01(navigation.velocity.magnitude / animatorRunningVelocity),
			Time.deltaTime / speedDampening));
	}

	public void SetControllerInput(Vector2 movement)
	{
		movementMode = MovementMode.Control;
		inputMovement = movement;
		inputDestinationPosition = null;
		inputDestinationRotation = null;
	}

	public void SetPathInput(Vector3 destination)
	{
		movementMode = MovementMode.Path;
		inputMovement = Vector2.zero;
		inputDestinationPosition = destination;
		inputDestinationRotation = null;
	}

	public void SetPathInput(Vector3 destination, Quaternion facingDirection)
	{
		movementMode = MovementMode.Path;
		inputMovement = Vector2.zero;
		inputDestinationPosition = destination;
		inputDestinationRotation = facingDirection;
	}

	public void SetPathInput(Quaternion facingDirection)
	{
		movementMode = MovementMode.Path;
		inputMovement = Vector2.zero;
		inputDestinationPosition = null;
		inputDestinationRotation = facingDirection;
		navigation.isStopped = true;
	}

	public void SetIdle()
	{
		movementMode = MovementMode.Idle;
		inputMovement = Vector2.zero;
		inputDestinationPosition = null;
		inputDestinationRotation = null;
	}
}
