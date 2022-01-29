using System.Collections;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
	[SerializeField] private TransitionBase TransitionFromSplashscreen;
	[SerializeField] private TransitionBase TransitionAfterKilling;
	[SerializeField] private TransitionBase TransitionFromDaytime;
	[SerializeField] private TransitionBase TransitionFromNighttime;

	private void Start()
	{
		StartCoroutine(GameplayLoop());
	}

	private IEnumerator GameplayLoop()
	{
		while (true)
		{


			yield return null;
		}
	}
}
