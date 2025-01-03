using UnityEngine.Tilemaps;
using System.Collections.Generic;

[System.Serializable]
public class Biome
{
    public string Name;
    public float MinTemperature;
    public float MaxTemperature;
    public Tile Tile; // TODO: Remove this once TileRule is implemented.
    public List<TileRule> TileRules;
}
