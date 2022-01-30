using UnityEngine;

public class VillageNPCEngine : MonoBehaviour
{
	[SerializeField]
	private VillageCharacter character;

	[SerializeField]
	private VillageNPCEngineState defaultState;

	[SerializeField]
	private VillagerEffects villagerEffectsPrefab;

	[SerializeField] public float moveSpeed = 2.0f;
	[SerializeField] public int happiness;
	[SerializeField] public bool isTalking;
	[SerializeField] public bool hasBeenTalkedTo;

	private VillageNPCEngineState currentState;
	private Coroutine currentStateCoroutine;

	public VillagerEffects Effects { get; private set; }

	public VillageCharacter Character => character;
	public VillageNPCEngineState DefaultState => defaultState;

	private void Awake()
	{
		var states = GetComponents<VillageNPCEngineState>();
		foreach (var state in states)
		{
			state.Engine = this;
			state.IsActiveState = false;
		}

		Effects = Instantiate(villagerEffectsPrefab, character.transform, false);

		var newComponent = character.gameObject.AddComponent<VillageNPCInteractable>();
		newComponent.Engine = this;
	}

	private void Start()
	{
		DayReset();
	}

	public void ChangeState(VillageNPCEngineState newState)
	{
		if (currentState != null)
		{
			currentState.IsActiveState = false;
			currentState.OnExit();

			if (currentStateCoroutine != null)
			{
				StopCoroutine(currentStateCoroutine);
				currentStateCoroutine = null;
			}
		}

		currentState = newState;
		currentState.IsActiveState = true;
		currentState.OnEnter();
		currentStateCoroutine = StartCoroutine(newState.DoState());
	}

	public void DayReset()
	{
		character.transform.SetPositionAndRotation(transform.position, transform.rotation);
		ChangeState(defaultState);
	}
}
