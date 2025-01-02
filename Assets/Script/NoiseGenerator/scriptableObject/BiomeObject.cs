using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New WeatherZoneObject", menuName = "NoiseGenerators/Object/BiomeObject")]
[System.Serializable]
public class Biome : ScriptableObject
{
    public string Name;
    public float MinTemperature;
    public float MaxTemperature;
}