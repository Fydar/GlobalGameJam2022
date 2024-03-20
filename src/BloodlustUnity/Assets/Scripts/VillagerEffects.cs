using UnityEngine;

public class VillagerEffects : MonoBehaviour
{
	[SerializeField] private Animator killIndicator;
	[SerializeField] private Animator talkIndicator;
	[SerializeField] private ParticleSystem exclamationParticles;

	public void SetKillIndicator(bool activity)
	{
		killIndicator.gameObject.SetActive(activity);
	}

	public void TriggerKillIndicator()
	{
		killIndicator.SetTrigger("Use");
	}

	public void SetTalkIndicator(bool activity)
	{
		talkIndicator.gameObject.SetActive(activity);
	}

	public void TriggerTalkIndicator()
	{
		talkIndicator.SetTrigger("Use");
	}

	public void PlayExclamation()
	{
		exclamationParticles.Play();
	}
}
