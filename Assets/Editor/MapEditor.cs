using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileManager))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Target script
        TileManager tilePlacer = (TileManager)target;

        // If AutoUpdate is true, generate the map when the inspector is updated
        if (DrawDefaultInspector())
        {
            if (tilePlacer.AutoUpdate)
            {
                tilePlacer.GenerateMap();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            tilePlacer.GenerateMap();
        }
    }
}
