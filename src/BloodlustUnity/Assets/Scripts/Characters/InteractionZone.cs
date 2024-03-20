using System.Collections.Generic;
using UnityEngine;

public class InteractionZone : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> inArea = new();

	private void OnTriggerEnter(Collider other)
	{
		var otherInteractable = other.gameObject;
		if (otherInteractable != null)
		{
			inArea.Add(otherInteractable);

			Sort();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		var otherInteractable = other.gameObject;
		if (otherInteractable != null)
		{
			inArea.Remove(otherInteractable);
		}
	}

	public bool CanInteract()
	{
		Prune();

		return inArea.Count > 0;
	}

	public TComponent GetComponentInInteractivity<TComponent>()
		where TComponent : class
	{
		Prune();

		if (inArea.Count == 0)
		{
			return null;
		}

		Sort();

		foreach (var inArea in inArea)
		{
			var component = inArea.GetComponent<TComponent>();

			if (component != null)
			{
				return component;
			}
		}

		return null;
	}

	private void Prune()
	{
		for (int i = inArea.Count - 1; i >= 0; i--)
		{
			if (inArea[i] == null)
			{
				inArea.RemoveAt(i);
			}
		}
	}

	private void Sort()
	{
		inArea.Sort((a, b) => Vector3.Distance(transform.position, a.transform.position)
				.CompareTo(Vector3.Distance(transform.position, b.transform.position)));
	}
}
