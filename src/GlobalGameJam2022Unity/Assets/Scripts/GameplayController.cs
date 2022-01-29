using FMODUnity;
using GlobalGameJam2022;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class GameplayController : MonoBehaviour
{
	[Header("Managers")]
	[SerializeField] private DialogueManager Dialogue;
	[SerializeField] private VillagePlayerController Player;

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
	public string DaytimeSceneId = "DaytimeVillage";
	public string NighttimeSceneId = "NighttimeVillage";

	[Header("Dialogue")]
	public DialogueTranscript IntroductionSequence;

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

		var loadedDaytime = SceneManager.LoadScene(DaytimeSceneId, new LoadSceneParameters(LoadSceneMode.Additive));
		var loadedNighttime = SceneManager.LoadScene(NighttimeSceneId, new LoadSceneParameters(LoadSceneMode.Additive));

		var aliveCharacters = new List<VillageNPC>();
		aliveCharacters.AddRange(FindObjectsOfType<VillageNPC>(true));

		SetActiveInScene(loadedDaytime, true);
		SetActiveInScene(loadedNighttime, false);


		int dayNumber = 0;
		while (true)
		{
			dayNumber++;

			// # Daytime

			// ## Setup new day
			yield return null;

			Player.transform.SetParent(null);
			SceneManager.MoveGameObjectToScene(Player.gameObject, loadedDaytime);
			SceneManager.MoveGameObjectToScene(gameObject, loadedDaytime);
			Player.gameObject.SetActive(true);

			SetActiveInScene(loadedDaytime, true);
			SetActiveInScene(loadedNighttime, false);

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
				yield return new WaitForSeconds(2.5f);

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


			float timeUntilBloodlust = Random.Range(TimeUntilBloodlustMinimum, TimeUntilBloodlustMaximum);

			// ## Daytime Gameplay
			while (true)
			{
				yield return null;

				timeUntilBloodlust -= Time.deltaTime;

				if (timeUntilBloodlust <= 0.0f)
				{
					RuntimeManager.StudioSystem.setParameterByName("Bloodlust", 0);

					Player.enabled = false;

					yield return new WaitForSeconds(0.1f);

					if (dayNumber == 1)
					{
						yield return StartCoroutine(Dialogue.DialogueRoutine(IntroductionSequence));
					}
					else
					{
						yield return StartCoroutine(
							Dialogue.DialogueRoutine(
								AlternateBloodlustSequences[Random.Range(0, AlternateBloodlustSequences.Length)]));
					}

					RuntimeManager.StudioSystem.setParameterByName("Bloodlust", 1);

					foreach (var npc in aliveCharacters)
					{
						// npc.RunFrom(Player);
					}

					Player.killedCharacter = null;
					Player.IsBloodlusted = true;

					while (Player.killedCharacter == null)
					{
						yield return null;
					}
					break;
				}
			}

			Player.IsBloodlusted = false;
			KillSequence.CharacterGraphic = Player.killedCharacter.GetComponent<VillageCharacterController>().Character.DialogueGraphic;
			KillSequence.gameObject.SetActive(true);
			yield return new WaitForSeconds(5.0f);

			RuntimeManager.StudioSystem.setParameterByName("Bloodlust", 2);

			foreach (float time in new TimedLoop(TransitionAfterKillingSpeed))
			{
				TransitionAfterKilling.SetTime(time);
				yield return null;
			}

			RuntimeManager.StudioSystem.setParameterByName("Bloodlust", -1);

			// # Nighttime
			Player.transform.SetParent(null);
			SceneManager.MoveGameObjectToScene(Player.gameObject, loadedNighttime);
			SceneManager.MoveGameObjectToScene(gameObject, loadedNighttime);
			Player.gameObject.SetActive(true);

			SetActiveInScene(loadedDaytime, false);
			SetActiveInScene(loadedNighttime, true);

			TransitionAfterKilling.SetTime(0.0f);
			TransitionFromDaytime.SetTime(1.0f);

			RuntimeManager.StudioSystem.setParameterByName("TimeOfDay", 1);

			foreach (float time in new TimedLoop(TransitionFromDaytimeSpeed))
			{
				TransitionFromDaytime.SetTime(1.0f - time);
				yield return null;
			}

			// ## Nighttime Introduction
			if (Player.killedCharacter != null)
			{
				while (true)
				{

					yield return null;
				}
			}

			Player.enabled = true;

			// ## Nighttime Gameplay
			while (true)
			{
				yield return null;
			}

			foreach (float time in new TimedLoop(TransitionFromNighttimeSpeed))
			{
				TransitionFromNighttime.SetTime(time);
				yield return null;
			}
		}
	}

	void SetActiveInScene(Scene scene, bool state)
	{
		var rootObjects = scene.GetRootGameObjects();
		for (int i = 0; i < rootObjects.Length; i++)
		{
			var rootObject = rootObjects[i];

			rootObject.SetActive(state);
		}
	}
}
