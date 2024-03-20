using System.Collections;
using UnityEngine;

public abstract class VillageNPCEngineState : MonoBehaviour
{
	public VillageNPCEngine Engine { get; internal set; }
	public bool IsActiveState { get; internal set; }

	public abstract IEnumerator DoState();
	public virtual void OnEnter() { }
	public virtual void OnExit() { }
}
