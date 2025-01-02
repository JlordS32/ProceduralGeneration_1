using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

[CustomEditor(typeof(TileManager))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Target script
        TileManager tileManager = (TileManager)target;

        // If AutoUpdate is true, generate the map when the inspector is updated
        if (DrawDefaultInspector())
        {
            if (tileManager.AutoUpdate)
            {
                tileManager.GenerateMap();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            tileManager.GenerateMap();
        }
    }

    private void OnSceneGUI()
    {
        TileManager tileManager = (TileManager)target;

        if (tileManager != null)
        {
            Vector2 mousePosition = Event.current.mousePosition;
            Vector3 worldMousePosition = HandleUtility.GUIPointToWorldRay(mousePosition).origin;

            // Convert the world mouse position to the tilemap's cell position
            Tilemap tilemap = tileManager.GetComponent<Tilemap>();
            float[,] heightMap;
            float[,] temperatureMap;
            Vector3Int tilePosition = tilemap.WorldToCell(worldMousePosition);

            // If null, regenerate maps
            if (tileManager.HeightMap == null) tileManager.GenerateMap();
            heightMap = tileManager.HeightMap;

            if (tileManager.HeightMap == null) tileManager.GenerateMap();
            temperatureMap = tileManager.TemperatureMap;

            // Check if the tile position is within the bounds of the heightMap and temperatureMap
            int widthOffset = tileManager.Width / 2;
            int heightOffset = tileManager.Height / 2;

            // Avoid accessing out of bounds if the mouse is outside the valid range
            if (tilePosition.x + widthOffset >= 0 && tilePosition.x + widthOffset < heightMap.GetLength(0) &&
                tilePosition.y + heightOffset >= 0 && tilePosition.y + heightOffset < heightMap.GetLength(1))
            {
                // Display information in the Scene view
                Handles.BeginGUI();
                GUI.Label(new Rect(10, 10, 300, 20), $"Mouse Position: {worldMousePosition}");
                GUI.Label(new Rect(10, 30, 300, 20), $"Tile Position: {tilePosition}");

                // Check and display height and temperature values
                GUI.Label(new Rect(10, 50, 300, 20), $"Height: {heightMap[tilePosition.x + widthOffset, tilePosition.y + heightOffset]}");
                GUI.Label(new Rect(10, 70, 300, 20), $"Temperature: {temperatureMap[tilePosition.x + widthOffset, tilePosition.y + heightOffset]} C");

                Handles.EndGUI();
            }
            else
            {
                Handles.BeginGUI();
                GUI.Label(new Rect(10, 10, 300, 20), "Mouse position is outside of the tilemap.");
                Handles.EndGUI();
            }
        }
    }
}
