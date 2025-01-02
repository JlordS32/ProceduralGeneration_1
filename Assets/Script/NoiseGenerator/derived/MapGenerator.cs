using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class MapGenerator
{
    [Header ("Perlin Map Generator Params")]
    [SerializeField] private TerrainObject _terrainObj;
    
    // Temperature map
    private float[,] _temperatureMap;
    public void SetTemperatureMap(float[,] temperatureMap) => _temperatureMap = temperatureMap;

    // Height map
    private float[,] _heightMap;
    public void SetHeightMap(float[,] heightMap) => _heightMap = heightMap;

    // Variables
    private Tile [,] _tiles;

    public Tile[,] Generate(int width, int height)
    {
        // Initialize the Tile array
        _tiles = new Tile[width, height];

        // Map the noise values to terrain types
        for (int x = 0; x < width; x++)
        { 
            for (int y = 0; y < height; y++)
            {
                float elevation = _heightMap[x, y];

                // Determine the appropriate terrain tile based on noise height
                foreach (TerrainType terrain in _terrainObj.Terrains)
                {
                    if (elevation >= terrain.MinHeight && elevation < terrain.MaxHeight)
                    {
                        _tiles[x, y] = terrain.Tile;
                        break;
                    }
                }
            }
        }

        return _tiles;
    }
}
