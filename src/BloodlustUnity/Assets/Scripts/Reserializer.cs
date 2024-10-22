#if UNITY_EDITOR
using UnityEditor;

public static class Reserializer
{
    [MenuItem("Toolkit/Reserialize")]
    public static void Reserialize()
    {
        AssetDatabase.ForceReserializeAssets();
    }
}
#endif
