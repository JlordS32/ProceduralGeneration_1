using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New WeatherZoneObject", menuName = "NoiseGenerators/Object/TerrainObject")]
[System.Serializable]
public class TerrainObject : ScriptableObject {
    public TerrainType[] Terrains;
}

[System.Serializable]
public struct TerrainType
{
    public string Name;
    public float MinHeight;
    public float MaxHeight;
    public float Temperature;
    public Tile Tile;
}