using UnityEngine;

public class VillagePlayerEngine : MonoBehaviour
{
	[SerializeField]
	private VillageCharacter character;

	[SerializeField]
	private InteractionZone interactionZone;

	[Header("State")]
	public IInteractable activatedInteractable;
	public bool IsBloodlusted = false;

	private IInteractable currentInteractable;

	public VillageCharacter Character => character;

	private void Update()
	{
		var inputMovement = new Vector2(
			Input.GetAxisRaw("Horizontal"),
			Input.GetAxisRaw("Vertical"));

		character.SetControllerInput(inputMovement);

		if (!IsBloodlusted)
		{
			var nextInteractable = interactionZone.GetComponentInInteractivity<IInteractable>();
			if (currentInteractable != nextInteractable)
			{
				SetSelected(nextInteractable);
			}

			if (Input.GetMouseButtonDown(0))
			{
				if (currentInteractable is VillageNPCInteractable currentNpc)
				{
					currentNpc.Engine.Effects.TriggerTalkIndicator();
					activatedInteractable = currentInteractable;
				}
			}
		}
		else
		{
			// Only select villagers if you are bloodlusted
			var nextInteractable = interactionZone.GetComponentInInteractivity<VillageNPCInteractable>();
			if (currentInteractable != nextInteractable as IInteractable)
			{
				SetSelected(nextInteractable);
			}

			if (Input.GetMouseButtonDown(0))
			{
				if (currentInteractable is VillageNPCInteractable currentNpc)
				{
					currentNpc.Engine.Effects.TriggerKillIndicator();
					activatedInteractable = currentInteractable;
				}
			}
		}
	}

	public void SetSelected(IInteractable nextInteractable)
	{
		if (currentInteractable is VillageNPCInteractable oldNpc)
		{
			oldNpc.Engine.Effects.SetKillIndicator(false);
			oldNpc.Engine.Effects.SetTalkIndicator(false);
		}

		currentInteractable = nextInteractable;

		if (currentInteractable is VillageNPCInteractable newNpc)
		{
			newNpc.Engine.Effects.SetKillIndicator(IsBloodlusted);
			newNpc.Engine.Effects.SetTalkIndicator(!IsBloodlusted);
		}
	}

	public void DayReset()
	{
		character.transform.SetPositionAndRotation(transform.position, transform.rotation);
	}
}
