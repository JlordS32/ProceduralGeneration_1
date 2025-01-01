using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class MapGenerator : NoiseGenerator
{
    [Header ("Perlin Map Generator Params")]
    [SerializeField] private TerrainType[] _terrainType;
    
    // Temperature map
    private float[,] _temperatureMap;
    public void SetTemperatureMap(float[,] temperatureMap) => _temperatureMap = temperatureMap;

    public override Tile[,] Generate(int width, int height)
    {
        // Generate the Perlin noise map
        float[,] noiseMap = Noise.GenerateNoiseMap(width, height, _seed, _noiseScale, _octaves, _lacunarity, _persistance, _offset);

        // Initialize the Tile array
        _tiles = new Tile[width, height];

        // Map the noise values to terrain types
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float currentHeight = noiseMap[x, y];

                // Determine the appropriate terrain tile based on noise height
                foreach (TerrainType terrain in _terrainType)
                {
                    if (currentHeight <= terrain.NoiseHeight)
                    {
                        _tiles[x, y] = terrain.tile;
                        break;
                    }
                }
            }
        }

        return _tiles;
    }
}

[System.Serializable]
public struct TerrainType
{
    public string Name;
    public float NoiseHeight;
    public Tile tile;
}

public struct Biome {
    public float temperature;
}