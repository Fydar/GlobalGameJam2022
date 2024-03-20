using System.IO;
using UnityEditor;
using UnityEngine;

public class TabPageResources : ScriptableObject
{
	private static TabPageResources InstanceCache;

	public Texture2D TabManagerIcon;
	public Texture2D TabPageIcon;
	public Texture2D DefaultTabPageIcon;

	[Header("Transitions")]
	public Texture2D AppearIcon;
	public Texture2D FadeIcon;
	public Texture2D GrowIcon;
	public Texture2D ShinkIcon;

	public static TabPageResources Instance
	{
		get
		{
			if (InstanceCache == null)
			{
				string path = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(CreateInstance<TabPageResources>()));
				string dir = Path.GetDirectoryName(path);
				InstanceCache = AssetDatabase.LoadAssetAtPath<TabPageResources>(Path.Combine(dir, $"{typeof(TabPageResources).Name}.asset"));

				if (InstanceCache == null)
				{
					InstanceCache = CreateInstance<TabPageResources>();
					AssetDatabase.CreateAsset(InstanceCache, Path.Combine(dir, $"{typeof(TabPageResources).Name}.asset"));
				}
			}

			return InstanceCache;
		}
	}
}
