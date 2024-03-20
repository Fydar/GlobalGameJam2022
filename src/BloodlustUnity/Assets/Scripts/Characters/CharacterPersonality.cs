using UnityEngine;

[CreateAssetMenu]
public class CharacterPersonality : ScriptableObject
{
	public string Name;
	public Sprite DialogueGraphic;

	public DialogueTranscript[] DayFirstDialogue;
	public DialogueTranscript[] DayDefaultDialogue;
	public DialogueTranscript[] NightDefaultDialogue;

	[Tooltip("Dialogue used when the player kills them and ponders on their actions.")]
	public DialogueTranscript[] RegretSpeech;
}
