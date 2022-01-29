using GlobalGameJam2022;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour
{
	[Header("Managers")]
	[SerializeField] private DialogueManager Dialogue;

	[Header("Transitions")]
	[SerializeField] private TransitionBase TransitionFromSplashscreen;
	[SerializeField] private float TransitionFromSplashscreenSpeed;


	[Space]
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

	private void Start()
	{
		TransitionFromSplashscreen.SetTime(1.0f);
		StartCoroutine(GameplayLoop());
	}

	private IEnumerator GameplayLoop()
	{
		var loadedDaytime = SceneManager.LoadScene(DaytimeSceneId, new LoadSceneParameters(LoadSceneMode.Additive));
		var loadedNighttime = SceneManager.LoadScene(NighttimeSceneId, new LoadSceneParameters(LoadSceneMode.Additive));

		SetActiveInScene(loadedDaytime, true);
		SetActiveInScene(loadedNighttime, false);

		foreach (float time in new TimedLoop(TransitionFromSplashscreenSpeed))
		{
			TransitionFromSplashscreen.SetTime(1.0f - time);
			yield return null;
		}

		yield return new WaitForSeconds(2.5f);

		if (IntroductionSequence != null)
		{
			yield return StartCoroutine(Dialogue.DialogueRoutine(IntroductionSequence));
		}

		int dayNumber = 0;
		while (true)
		{
			dayNumber++;

			// # Daytime
			SetActiveInScene(loadedDaytime, true);
			SetActiveInScene(loadedNighttime, false);

			// ## Bountyboard Sequence
			while (true)
			{
				yield return null;
			}

			CharacterTemplate killedCharacter = null;

			// ## Daytime Gameplay
			while (true)
			{
				yield return null;
			}

			// # Nighttime
			SetActiveInScene(loadedDaytime, false);
			SetActiveInScene(loadedNighttime, true);

			// ## Nighttime Introduction
			while (true)
			{
				if (killedCharacter != null)
				{

				}

				yield return null;
			}

			// ## Nighttime Gameplay
			while (true)
			{
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
