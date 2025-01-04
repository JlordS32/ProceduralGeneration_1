using UnityEngine.Tilemaps;
using System.Collections.Generic;

[System.Serializable]
public class Biome
{
    public string Name;
    public float MinTemperature;
    public float MaxTemperature;
    public List<TileRule> TileRules;
}
