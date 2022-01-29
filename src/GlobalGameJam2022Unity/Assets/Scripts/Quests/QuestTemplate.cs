using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTemplate : ScriptableObject
{
	public string Name;

	public DialogueTranscript InitialInteraction;
	public DialogueTranscript ReminderPrompt;
}

