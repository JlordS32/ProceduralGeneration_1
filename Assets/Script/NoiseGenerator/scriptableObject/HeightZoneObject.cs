using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New WeatherZoneObject", menuName = "NoiseGenerators/Object/HeightZone")]
[System.Serializable]
public class HeightZoneObject : ScriptableObject {
    public HeightZone[] HeightZones;
}

[System.Serializable]
public struct HeightZone
{
    public Tile Tile;
    public float MinHeight;
    public float MaxHeight;
}