using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
	[BankRef]
	public List<string> Banks = new List<string>();

	public string Scene = "MainScene";

	public void Start()
	{
		StartCoroutine(LoadGameAsync());
	}

	private IEnumerator LoadGameAsync()
	{
		var async = SceneManager.LoadSceneAsync(Scene);

		async.allowSceneActivation = false;

		foreach (string bank in Banks)
		{
			RuntimeManager.LoadBank(bank, true);
		}

		while (!RuntimeManager.HaveAllBanksLoaded)
		{
			yield return null;
		}

		while (RuntimeManager.AnySampleDataLoading())
		{
			yield return null;
		}

		async.allowSceneActivation = true;

		while (!async.isDone)
		{
			yield return null;
		}
	}
}
