using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TabManager))]
internal class TabMManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		var manager = (TabManager)target;
		var origionalColor = GUI.color;

		foreach (var tab in manager.Pages)
		{
			GUI.color = tab.isActiveAndEnabled ? new Color(0.925f, 0.925f, 0.925f, 1.0f) : new Color(1.0f, 1.0f, 1.0f, 0.4f);

			GUIContent tabContent;

			if (tab.Default)
			{
				tabContent = new GUIContent(tab.name, EditorGUIUtility.IconContent("Favorite").image);
			}
			else
			{
				tabContent = new GUIContent(tab.name);
			}

			if (GUILayout.Button(tabContent, EditorStyles.toolbarButton))
			{
				EditorGUIUtility.PingObject(tab);
				manager.Goto(tab);
			}
		}
		GUI.color = origionalColor;
	}
}
