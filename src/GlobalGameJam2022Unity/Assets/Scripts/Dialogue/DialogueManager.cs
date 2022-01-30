using GlobalGameJam2022;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
	public static DialogueManager Instance;

	public CharacterSlot[] Slots;

	public int CurrentlyHighlighted = -1;

	[Header("UI")]
	public Text SubtitleText;
	public float TalkerScale;
	public float TalkerScaleSpeed = 2.0f;
	public Color UnfocusedColour;
	public float UnfocusedColourSpeed = 2.0f;

	[Header("Bars")]
	public float BarsInTime = 0.5f;
	public CanvasGroup Darken;
	public RectTransform TopBar;
	public RectTransform BottomBar;

	public bool IsPlaying;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		SubtitleText.text = "";
		SetBars(0.0f);
	}

	public bool IsCharacterTalking(CharacterPersonality character)
	{
		foreach (var slot in Slots)
		{
			if (slot.CurrentTalker == null)
			{
				continue;
			}
			if (slot.CurrentTalker == character && slot.Focused)
			{
				return true;
			}
		}
		return false;
	}

	private void Update()
	{
		foreach (var slot in Slots)
		{
			float desiredScale = slot.Focused ? TalkerScale : 1.0f;
			if (!IsPlaying)
			{
				desiredScale = 0.0f;
			}
			var desiredColor = slot.Focused ? Color.white : UnfocusedColour;

			slot.SlideArea.transform.localScale = Vector3.Lerp(
				slot.SlideArea.transform.localScale,
				Vector3.one * desiredScale,
				Time.unscaledDeltaTime * TalkerScaleSpeed);

			slot.CharacterGraphic.color = Color.Lerp(
				slot.CharacterGraphic.color,
				desiredColor, Time.unscaledDeltaTime * UnfocusedColourSpeed);
		}
	}

	public void SetBars(float value)
	{
		TopBar.pivot = new Vector2(TopBar.pivot.x, value);
		var topSize = TopBar.sizeDelta;
		TopBar.offsetMin = new Vector2(0, 0);
		TopBar.offsetMax = new Vector2(0, 0);
		TopBar.sizeDelta = topSize;

		BottomBar.pivot = new Vector2(BottomBar.pivot.x, 1.0f - value);
		var bottomSize = BottomBar.sizeDelta;
		BottomBar.offsetMin = new Vector2(0, 0);
		BottomBar.offsetMax = new Vector2(0, 0);
		BottomBar.sizeDelta = bottomSize;

		Darken.alpha = value;
	}

	public IEnumerator DialogueRoutine(DialogueTranscript script)
	{
		IsPlaying = true;
		foreach (var slot in Slots)
		{
			slot.Focused = false;
			slot.CharacterGraphic.enabled = false;
			slot.CharacterGraphic.sprite = null;
		}

		foreach (float time in new TimedLoop(BarsInTime))
		{
			SetBars(time);

			yield return null;
		}

		foreach (var message in script.Content)
		{
			CharacterSlot messageSlot = null;
			foreach (var findSlot in Slots)
			{
				if (findSlot.CurrentTalker == message.Character)
				{
					messageSlot = findSlot;
					break;
				}
			}
			if (messageSlot == null)
			{
				foreach (var findSlot in Slots)
				{
					if (!findSlot.Focused)
					{
						messageSlot = findSlot;
						break;
					}
				}
			}

			foreach (var findSlot in Slots)
			{
				findSlot.Focused = false;
			}

			SubtitleText.text = message.Body;
			messageSlot.CharacterGraphic.sprite = message.Character.DialogueGraphic;
			messageSlot.CharacterGraphic.enabled = true;

			messageSlot.Focused = true;
			messageSlot.CurrentTalker = message.Character;
			messageSlot.CharacterGraphic.sprite = message.Graphic;

			yield return new WaitForSeconds(0.2f);

			while (!Input.GetMouseButtonDown(0))
			{
				yield return null;
			}
		}
		IsPlaying = false;

		foreach (var slot in Slots)
		{
			slot.Focused = false;
		}

		foreach (float time in new TimedLoop(BarsInTime))
		{
			SetBars(1.0f - time);
			yield return null;
		}

	}
}
