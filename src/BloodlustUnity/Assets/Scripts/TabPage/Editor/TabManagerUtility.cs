using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
internal class TabManagerUtility
{
	static TabManagerUtility()
	{
		EditorApplication.hierarchyWindowItemOnGUI += DrawHierachy;
	}

	private static void DrawHierachy(int instanceID, Rect selectionRect)
	{
		var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
		if (go == null)
		{
			return;
		}

		var slotCon = go.GetComponent<TabManager>();
		if (slotCon != null)
		{
			var r = new Rect(selectionRect);
			r.xMin = r.xMax - r.height;

			GUI.DrawTexture(r, TabPageResources.Instance.TabManagerIcon);
		}
	}
}
