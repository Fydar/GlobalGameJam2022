using System;
using UnityEngine;

[Serializable]
public struct DialogueMessage
{
	[TextArea(1, 3)]
	public string Body;

	[Space]
	public CharacterPersonality Character;
	public Sprite Graphic;
}
