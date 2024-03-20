using Cinemachine;
using FMODUnity;
using GlobalGameJam2022;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour
{
	[Header("Managers")]
	[SerializeField] private DialogueManager Dialogue;
	[SerializeField] private VillagePlayerEngine Player;

	[Header("Transitions")]
	[SerializeField] private TransitionBase TransitionAfterKilling;
	[SerializeField] private float TransitionAfterKillingSpeed;

	[Space]
	[SerializeField] private TransitionBase TransitionFromDaytime;
	[SerializeField] private float TransitionFromDaytimeSpeed;

	[Space]
	[SerializeField] private TransitionBase TransitionFromNighttime;
	[SerializeField] private float TransitionFromNighttimeSpeed;

	[Header("Scenes")]
	public string VillageEnvironmentDaySceneId = "Village_Environment_Day";
	public string VillageEnvironmentNightSceneId = "Village_Environment_Night";
	public string VillageVillagersSceneId = "Village_Villagers";

	[Header("Dialogue")]
	public CinemachineVirtualCamera DialogueCamera;
	public DialogueTarget DialogueTarget;

	[Space]
	public DialogueTranscript IntroductionSequence;

	[Space]
	public DialogueTranscript ClosingMonologue;
	public Transform WalkToAtEnd;

	[Space]
	public DialogueTranscript FirstBloodlustSequence;
	public DialogueTranscript[] AlternateBloodlustSequences;

	[Space]
	public KillSequenceController KillSequence;

	[Header("Gameplay")]
	public float TimeUntilBloodlustMinimum = 35.0f;
	public float TimeUntilBloodlustMaximum = 100.0f;

	[Space]
	public float TimeUntilSunriseMinimum = 120.0f;
	public float TimeUntilSunriseMaximum = 120.0f;

	private void Awake()
	{
		Dialogue.gameObject.SetActive(true);
		Player.gameObject.SetActive(false);
		KillSequence.gameObject.SetActive(false);
	}

	private void Start()
	{
		StartCoroutine(GameplayLoop());
	}

	private IEnumerator GameplayLoop()
	{
		TransitionFromNighttime.SetTime(1.0f);

		var loadedEnvironmentDay = SceneManager.LoadScene(VillageEnvironmentDaySceneId, new LoadSceneParameters(LoadSceneMode.Additive));
		var loadedEnvironmentNight = SceneManager.LoadScene(VillageEnvironmentNightSceneId, new LoadSceneParameters(LoadSceneMode.Additive));
		var loadedVillagers = SceneManager.LoadScene(VillageVillagersSceneId, new LoadSceneParameters(LoadSceneMode.Additive));

		yield return null;

		// Move the villagers to the day scene.
		foreach (var villager in loadedVillagers.GetRootGameObjects())
		{
			SceneManager.MoveGameObjectToScene(villager, loadedEnvironmentDay);
		}

		SetActiveInScene(loadedEnvironmentDay, true);
		SetActiveInScene(loadedEnvironmentNight, false);

		var aliveCharacters = new List<VillageNPCEngine>();
		aliveCharacters.AddRange(FindObjectsOfType<VillageNPCEngine>(true));

		int dayNumber = 0;
		while (true)
		{
			dayNumber++;

			// # Daytime

			// ## Setup new day
			Player.transform.SetParent(null);
			SceneManager.MoveGameObjectToScene(Player.gameObject, loadedEnvironmentDay);
			SceneManager.MoveGameObjectToScene(gameObject, loadedEnvironmentDay);
			Player.gameObject.SetActive(true);

			SetActiveInScene(loadedEnvironmentDay, true);
			SetActiveInScene(loadedEnvironmentNight, false);

			RuntimeManager.StudioSystem.setParameterByName("Bloodlust", -1);
			RuntimeManager.StudioSystem.setParameterByName("TimeOfDay", 0);

			foreach (float time in new TimedLoop(TransitionFromNighttimeSpeed))
			{
				TransitionFromNighttime.SetTime(1.0f - time);
				yield return null;
			}

			if (dayNumber == 1)
			{
				Player.enabled = false;
				// yield return new WaitForSeconds(2.5f);

				if (IntroductionSequence != null)
				{
					Dialogue.gameObject.SetActive(true);
					yield return StartCoroutine(Dialogue.DialogueRoutine(IntroductionSequence));
				}
			}

			// ## Bountyboard Sequence
			// while (true)
			// {
			// 	yield return null;
			// 
			// 	if (Input.GetMouseButtonDown(0))
			// 	{
			// 		break;
			// 	}
			// }

			Player.enabled = true;


			foreach (var npc in aliveCharacters)
			{
				npc.hasBeenTalkedTo = false;
			}

			// ## Daytime Talking Gameplay
			while (true)
			{
				Player.activatedInteractable = null;
				yield return null;

				if (Player.activatedInteractable is VillageNPCInteractable npcToTalkTo)
				{
					// Talk to a character
					Player.enabled = false;
					DialogueCamera.gameObject.SetActive(true);
					npcToTalkTo.Engine.ChangeState(npcToTalkTo.Engine.GetComponent<DialogueState>());

					DialogueTarget.SetTargets(new Transform[] { npcToTalkTo.Engine.Character.transform },
						npcToTalkTo.transform.position.x < Player.Character.transform.position.x ? DialogueSide.Right : DialogueSide.Left);

					DialogueCamera.LookAt = npcToTalkTo.Engine.Character.transform;

					if (npcToTalkTo.Engine.hasBeenTalkedTo)
					{
						yield return StartCoroutine(
							Dialogue.DialogueRoutine(npcToTalkTo.Engine.Character.Personality.DayDefaultDialogue[
								Random.Range(0, npcToTalkTo.Engine.Character.Personality.DayDefaultDialogue.Length)]));
					}
					else
					{
						npcToTalkTo.Engine.hasBeenTalkedTo = true;
						yield return StartCoroutine(
							Dialogue.DialogueRoutine(npcToTalkTo.Engine.Character.Personality.DayFirstDialogue[
								Random.Range(0, npcToTalkTo.Engine.Character.Personality.DayFirstDialogue.Length)]));
					}

					npcToTalkTo.Engine.ChangeState(npcToTalkTo.Engine.DefaultState);
					DialogueCamera.gameObject.SetActive(false);
					Player.enabled = true;
				}
				Player.activatedInteractable = null;

				bool isAll = true;
				foreach (var npc in aliveCharacters)
				{
					if (!npc.hasBeenTalkedTo)
					{
						isAll = false;
						break;
					}
				}
				if (isAll)
				{
					break;
				}
			}

			// ## Bloodlust Sequence
			float timeUntilBloodlust = Random.Range(TimeUntilBloodlustMinimum, TimeUntilBloodlustMaximum);
			yield return new WaitForSeconds(timeUntilBloodlust);

			RuntimeManager.StudioSystem.setParameterByName("Bloodlust", 0);

			Player.enabled = false;

			foreach (var npc in aliveCharacters)
			{
				npc.Effects.PlayExclamation();
			}

			yield return new WaitForSeconds(0.1f);

			if (dayNumber == 1)
			{
				yield return StartCoroutine(
					Dialogue.DialogueRoutine(FirstBloodlustSequence));
			}
			else
			{
				yield return StartCoroutine(
					Dialogue.DialogueRoutine(
						AlternateBloodlustSequences[Random.Range(0, AlternateBloodlustSequences.Length)]));
			}

			Player.enabled = true;
			RuntimeManager.StudioSystem.setParameterByName("Bloodlust", 1);

			foreach (var npc in aliveCharacters)
			{
				var fleeState = npc.GetComponent<FleeState>();
				if (fleeState != null)
				{
					fleeState.RunState(Player.Character.transform);
				}
			}

			Player.activatedInteractable = null;
			Player.IsBloodlusted = true;
			Player.enabled = true;


			VillageNPCEngine killedCharacter = null;
			// wait until a player has killed a character
			while (true)
			{
				if (Player.activatedInteractable is VillageNPCInteractable interactableNpc)
				{
					killedCharacter = interactableNpc.Engine;
					break;
				}
				yield return null;
			}

			Player.enabled = false;
			Player.IsBloodlusted = false;
			KillSequence.CharacterGraphic.sprite = killedCharacter.Character.Personality.DialogueGraphic;
			RuntimeManager.StudioSystem.setParameterByName("Bloodlust", 2);
			KillSequence.gameObject.SetActive(true);
			yield return new WaitForSeconds(5.0f);
			KillSequence.gameObject.SetActive(false);

			TransitionFromDaytime.SetTime(1.0f);

			aliveCharacters.Remove(killedCharacter);
			killedCharacter.gameObject.SetActive(false);

			RuntimeManager.StudioSystem.setParameterByName("Bloodlust", -1);

			// # Nighttime
			foreach (var npc in aliveCharacters)
			{
				npc.hasBeenTalkedTo = false;
				SceneManager.MoveGameObjectToScene(npc.gameObject, loadedEnvironmentNight);
				npc.DayReset();
			}

			Player.transform.SetParent(null);
			SceneManager.MoveGameObjectToScene(Player.gameObject, loadedEnvironmentNight);
			SceneManager.MoveGameObjectToScene(gameObject, loadedEnvironmentNight);
			Player.gameObject.SetActive(true);

			SetActiveInScene(loadedEnvironmentDay, false);
			SetActiveInScene(loadedEnvironmentNight, true);

			RuntimeManager.StudioSystem.setParameterByName("TimeOfDay", 1);

			yield return new WaitForSeconds(2.0f);

			foreach (float time in new TimedLoop(TransitionFromDaytimeSpeed))
			{
				TransitionFromDaytime.SetTime(1.0f - time);
				yield return null;
			}
			Player.enabled = true;

			// ## Nighttime Talking Gameplay
			while (true)
			{
				Player.activatedInteractable = null;
				yield return null;

				if (Player.activatedInteractable is VillageNPCInteractable npcToTalkTo)
				{
					// Talk to a character
					Player.enabled = false;
					npcToTalkTo.Engine.ChangeState(npcToTalkTo.Engine.GetComponent<DialogueState>());

					DialogueTarget.SetTargets(new Transform[] { npcToTalkTo.Engine.Character.transform },
						npcToTalkTo.transform.position.x < Player.Character.transform.position.x ? DialogueSide.Right : DialogueSide.Left);

					DialogueCamera.gameObject.SetActive(true);
					DialogueCamera.LookAt = npcToTalkTo.Engine.Character.transform;

					if (npcToTalkTo.Engine.hasBeenTalkedTo)
					{
						yield return StartCoroutine(
							Dialogue.DialogueRoutine(npcToTalkTo.Engine.Character.Personality.NightDefaultDialogue[
								Random.Range(0, npcToTalkTo.Engine.Character.Personality.NightDefaultDialogue.Length)]));
					}
					else
					{
						npcToTalkTo.Engine.hasBeenTalkedTo = true;
						yield return StartCoroutine(
							Dialogue.DialogueRoutine(npcToTalkTo.Engine.Character.Personality.NightDefaultDialogue[
								Random.Range(0, npcToTalkTo.Engine.Character.Personality.NightDefaultDialogue.Length)]));
					}

					npcToTalkTo.Engine.ChangeState(npcToTalkTo.Engine.DefaultState);
					DialogueCamera.gameObject.SetActive(false);
					Player.enabled = true;
				}
				Player.activatedInteractable = null;

				bool isAll = true;
				foreach (var npc in aliveCharacters)
				{
					if (!npc.hasBeenTalkedTo)
					{
						isAll = false;
						break;
					}
				}
				if (isAll)
				{
					break;
				}
			}

			// ## Nighttime Monologue

			Player.enabled = false;
			if (WalkToAtEnd != null)
			{
				Player.Character.SetPathInput(WalkToAtEnd.position, WalkToAtEnd.rotation);
			}
			DialogueCamera.gameObject.SetActive(true);
			DialogueCamera.LookAt = Player.Character.transform;
			DialogueTarget.SetTargets(new Transform[] { Player.Character.transform }, DialogueSide.Right);

			yield return new WaitForSeconds(Random.Range(TimeUntilSunriseMinimum, TimeUntilSunriseMaximum));
			if (ClosingMonologue != null)
			{
				Dialogue.gameObject.SetActive(true);
				yield return StartCoroutine(Dialogue.DialogueRoutine(ClosingMonologue));
			}

			yield return new WaitForSeconds(0.5f);

			foreach (float time in new TimedLoop(TransitionFromNighttimeSpeed))
			{
				TransitionFromNighttime.SetTime(time);
				yield return null;
			}

			SceneManager.LoadScene(0, LoadSceneMode.Single);
		}
	}

	private void SetActiveInScene(Scene scene, bool state)
	{
		var rootObjects = scene.GetRootGameObjects();
		for (int i = 0; i < rootObjects.Length; i++)
		{
			var rootObject = rootObjects[i];

			rootObject.SetActive(state);
		}
	}
}
