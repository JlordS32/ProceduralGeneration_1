using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New WeatherZoneObject", menuName = "NoiseGenerators/Object/TerrainObject")]
[System.Serializable]
public class TerrainObject : ScriptableObject
{
    public Terrain[] Terrains;
}

[System.Serializable]
public class Terrain
{
    public string Name;
    public float MinHeight;
    public float MaxHeight;
    public Biome[] Biomes;
}
