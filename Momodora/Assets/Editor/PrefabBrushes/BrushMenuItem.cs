using UnityEngine;

namespace UnityEditor.Tilemaps
{
    static internal partial class AssetCreation
    {
        [MenuItem("Assets/Create/2D/Brushes/Prefab Brush", priority = 3)]
        static void CreatePrefabBrush()
        {
            ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance<PrefabBrush>(), "New Prefab Brush.asset");
        }

        [MenuItem("Assets/Create/2D/Brushes/Prefab Random Brush",
            priority = 4)]
        static void CreatePrefabRandomBrush()
        {
            ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance<PrefabRandomBrush>(),
                "New Prefab Random Brush.asset");
        }

        [MenuItem("Assets/Create/2D/Brushes/Random Brush", priority = 5)]
        static void CreateRandomBrush()
        {
            ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance<RandomBrush>(), "New Random Brush.asset");
        }
    }
}