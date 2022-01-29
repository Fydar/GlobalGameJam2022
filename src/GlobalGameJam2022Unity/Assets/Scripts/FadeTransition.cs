using UnityEngine;

public class FadeTransition : TransitionBase
{
	[SerializeField]
	private CanvasGroup canvasGroup;

	public override void SetTime(float time)
	{
		canvasGroup.alpha = time;
	}
}
