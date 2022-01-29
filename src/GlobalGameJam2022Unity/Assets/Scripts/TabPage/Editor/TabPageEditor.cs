using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TabPage))]
[CanEditMultipleObjects]
internal class TabPageEditor : Editor
{
	public static GUIContent[] TransitionOptions = null;

	public override void OnInspectorGUI()
	{
		if (TransitionOptions == null)
		{
			TransitionOptions = new GUIContent[]
			{
				new GUIContent("Appear", TabPageResources.Instance.AppearIcon),
				new GUIContent("Fade", TabPageResources.Instance.FadeIcon),
				new GUIContent("Shrink", TabPageResources.Instance.ShinkIcon),
				new GUIContent("Grow", TabPageResources.Instance.GrowIcon)
			};
		}

		using (new EditorGUI.DisabledScope(true))
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
		}

		if (targets.Length == 1)
		{
			var page = (TabPage)target;
			EditorGUILayout.LabelField("Page Name", page.name);
			EditorGUILayout.LabelField("Page Index", Array.FindIndex(page.Manager.Pages, (findPage) => findPage == page).ToString());

			EditorGUI.BeginChangeCheck();
			page.Default = EditorGUILayout.Toggle("Default", page.Default);
			if (EditorGUI.EndChangeCheck())
			{
				if (page.Default == true)
				{
					foreach (var sibling in page.Manager.Pages)
					{
						sibling.Default = false;
					}
					page.Default = true;
					EditorApplication.RepaintHierarchyWindow();
				}
			}

			EditorGUI.BeginChangeCheck();
			page.TransitionMode = EditorGUILayout.Popup(new GUIContent("Transition"), page.TransitionMode, TransitionOptions);
			if (page.TransitionMode != 0)
			{
				using (new EditorGUI.IndentLevelScope())
				{
					page.TransitionDuration = EditorGUILayout.FloatField(new GUIContent("Duration"), page.TransitionDuration);
				}
			}
			if (EditorGUI.EndChangeCheck())
			{
				EditorUtility.SetDirty(page);
			}
		}
		else
		{
			EditorGUILayout.LabelField("Page Name", "--");
			EditorGUILayout.LabelField("Page Index", "--");

			bool agreeTransition = true;
			bool agreeDuration = true;
			var firstPage = (TabPage)targets[0];
			bool anyAppear = firstPage.TransitionMode == 0;
			int lastTransition = firstPage.TransitionMode;
			float lastDuration = firstPage.TransitionDuration;
			for (int i = 1; i < targets.Length; i++)
			{
				var page = (TabPage)targets[i];

				if (page.TransitionMode != lastTransition)
				{
					agreeTransition = false;
				}

				if (page.TransitionDuration != lastDuration)
				{
					agreeDuration = false;
				}

				if (page.TransitionMode == 0)
				{
					anyAppear = true;
				}

				if (anyAppear && agreeDuration && !agreeTransition)
				{
					break;
				}
			}

			bool showMixed = EditorGUI.showMixedValue;

			EditorGUI.showMixedValue = !agreeTransition;
			EditorGUI.BeginChangeCheck();
			int newTransition = EditorGUILayout.Popup(new GUIContent("Transition"), firstPage.TransitionMode, TransitionOptions);
			if (EditorGUI.EndChangeCheck())
			{
				for (int i = 0; i < targets.Length; i++)
				{
					var page = (TabPage)targets[i];
					page.TransitionMode = newTransition;
					EditorUtility.SetDirty(page);
				}
			}

			if (!anyAppear)
			{
				EditorGUI.showMixedValue = !agreeDuration;

				using (new EditorGUI.IndentLevelScope())
				{
					EditorGUI.BeginChangeCheck();
					float newDuration = EditorGUILayout.FloatField(new GUIContent("Duration"), lastDuration);
					if (EditorGUI.EndChangeCheck())
					{
						for (int i = 0; i < targets.Length; i++)
						{
							var page = (TabPage)targets[i];
							page.TransitionDuration = newDuration;
							EditorUtility.SetDirty(page);
						}
					}
				}
			}

			EditorGUI.showMixedValue = showMixed;
		}
	}
}
