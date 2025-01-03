using UnityEngine.Tilemaps;

[System.Serializable]
public class Biome
{
    public string Name;
    public float MinTemperature;
    public float MaxTemperature;
    public Tile Tile; // TODO: Update this to use TileRule instead
}
