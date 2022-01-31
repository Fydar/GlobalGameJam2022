using System;
using UnityEngine;

[ExecuteInEditMode]
public class DialogueTarget : MonoBehaviour
{
	[Header("Configuration")]
	[SerializeField]
	private float pullForwardOffset;

	[SerializeField]
	private float pullSidewaysOffset;

	[Header("State")]
	[SerializeField]
	private Transform[] targets = Array.Empty<Transform>();

	[SerializeField]
	private DialogueSide dialogueSide = DialogueSide.Left;


	private void Update()
	{
		if (targets.Length != 0)
		{
			var averagePosition = Vector3.zero;
			foreach (var target in targets)
			{
				averagePosition += target.position;
			}
			averagePosition /= targets.Length;

			transform.position = new Vector3(
				averagePosition.x - (dialogueSide == DialogueSide.Left ? pullSidewaysOffset : -pullSidewaysOffset),
				averagePosition.y,
				averagePosition.z - pullForwardOffset);
		}
	}

	public void SetTargets(Transform[] targets, DialogueSide dialogueSide)
	{
		this.targets = targets;
		this.dialogueSide = dialogueSide;
	}
}
