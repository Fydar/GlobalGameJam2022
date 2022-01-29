using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
internal class TabPageUtility
{
	static TabPageUtility()
	{
		Selection.selectionChanged += () =>
		{
			if (Selection.activeGameObject == null)
			{
				return;
			}

			if (Application.isPlaying)
			{
				return;
			}

			TabPage page = null;
			var current = Selection.activeGameObject.transform;

			while (page == null)
			{
				page = current.GetComponent<TabPage>();

				current = current.parent;

				if (current == null)
				{
					break;
				}
			}

			if (page == null)
			{
				return;
			}

			var manager = page.Manager;
			if (manager == null)
			{
				return;
			}

			manager.Goto(page);
		};

		EditorApplication.hierarchyWindowItemOnGUI += DrawHierachy;
	}

	private static void DrawHierachy(int instanceID, Rect selectionRect)
	{
		var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
		if (go == null)
		{
			return;
		}

		var slotCon = go.GetComponent<TabPage>();
		if (slotCon != null)
		{
			var r = new Rect(selectionRect);
			r.xMin = r.xMax - r.height;

			if (slotCon.Default)
			{
				GUI.DrawTexture(r, TabPageResources.Instance.DefaultTabPageIcon);
			}
			else
			{
				GUI.DrawTexture(r, TabPageResources.Instance.TabPageIcon);
			}
		}
	}
}
