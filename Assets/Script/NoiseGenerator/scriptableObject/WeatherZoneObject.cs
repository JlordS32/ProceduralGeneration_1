using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New WeatherZoneObject", menuName = "NoiseGenerators/Object/WeatherZone")]
[System.Serializable]
public class WeatherZoneObject : ScriptableObject
{
    public WeatherZone[] WeatherZones;
}

[System.Serializable]
public struct WeatherZone
{
    public float Temperature;
    public Tile Tile;
}
