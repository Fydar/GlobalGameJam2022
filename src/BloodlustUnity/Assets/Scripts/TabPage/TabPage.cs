using System.Collections.Generic;
using UnityEngine;

public class TabPage : MonoBehaviour
{
	public bool IsOpen;
	public bool Default;

	public int TransitionMode;
	public float TransitionDuration = 0.25f;

	private float CurrentTime;
	private bool IsAnimating;
	private TabManager ParentManager;

	public TabManager Manager
	{
		get
		{
			if (ParentManager == null)
			{
				var current = transform.parent;

				if (current == null)
				{
					Debug.LogError("<b>TabPage:</b> Tab Pages can't be root elements in the scene.");
					return null;
				}

				while (ParentManager == null)
				{
					if (current.GetComponent<TabPage>() != null)
					{
						break;
					}

					ParentManager = current.GetComponent<TabManager>();

					current = current.parent;

					if (current == null)
					{
						break;
					}
				}

				if (ParentManager == null)
				{
					Debug.LogError("<b>TabPage:</b> There is a TabPage without a TabManager in it's parent objects.");
					return null;
				}
			}
			return ParentManager;
		}
		set => ParentManager = value;
	}

	public void TransitionIn()
	{
		if (IsOpen)
		{
			return;
		}

		IsOpen = true;

		if (TransitionMode == 0 || !Application.isPlaying)
		{
			gameObject.SetActive(true);
		}
		else
		{
			if (!IsAnimating)
			{
				gameObject.SetActive(true);
				transform.SetAsLastSibling();
				StartCoroutine(TransitionRoutine());
			}
		}
	}

	public void TransitionOut()
	{
		if (!IsOpen)
		{
			return;
		}

		IsOpen = false;

		if (TransitionMode == 0 || !Application.isPlaying)
		{
			gameObject.SetActive(false);
		}
		else
		{
			if (!IsAnimating)
			{
				gameObject.SetActive(true);
				StartCoroutine(TransitionRoutine());
			}
		}
	}

	private IEnumerator<YieldInstruction> TransitionRoutine()
	{
		var fader = GetComponent<CanvasGroup>();
		if (fader != null)
		{
			IsAnimating = true;
			while (true)
			{
				fader.alpha = CurrentTime;

				CurrentTime += ((IsOpen ? 1.0f : 0.0f).CompareTo(CurrentTime) * Time.deltaTime) / TransitionDuration;
				CurrentTime = Mathf.Clamp01(CurrentTime);

				fader.alpha = CurrentTime;

				if (TransitionMode == 2)
				{
					transform.localScale = Vector3.Lerp(Vector3.one * 1.15f, Vector3.one, CurrentTime);
				}
				else if (TransitionMode == 3)
				{
					transform.localScale = Vector3.Lerp(Vector3.one * 0.85f, Vector3.one, CurrentTime);
				}

				if (Mathf.Abs(CurrentTime - (IsOpen ? 1.0f : 0.0f)) < 0.01f)
				{
					break;
				}

				yield return null;
			}
			fader.alpha = IsOpen ? 1.0f : 0.0f;
			IsAnimating = false;
		}

		if (!IsOpen)
		{
			gameObject.SetActive(false);
		}
	}
}
