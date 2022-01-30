using UnityEngine;

[CreateAssetMenu]
public class CharacterPersonality : ScriptableObject
{
	public string Name;
	public Sprite DialogueGraphic;

	public QuestTemplate[] Quests;

	[Tooltip("Dialogue used when the character has no quests.")]
	public DialogueTranscript[] DefaultDialogue;

	[Tooltip("Dialogue used when the player kills them and ponders on their actions.")]
	public DialogueTranscript[] RegretSpeech;
}
