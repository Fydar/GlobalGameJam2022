using System;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

[Serializable]
public class CharacterSlot
{
	public RectTransform SlideArea;
	public Image CharacterGraphic;

	[Space]
	[NonSerialized]
	public CharacterTemplate CurrentTalker;
	public bool Focused;
}
