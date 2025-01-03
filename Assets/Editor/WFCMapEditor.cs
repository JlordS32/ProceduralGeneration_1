using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(WFCTileManager))]
public class WFCMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Target script
        WFCTileManager tilePlacer = (WFCTileManager)target;

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