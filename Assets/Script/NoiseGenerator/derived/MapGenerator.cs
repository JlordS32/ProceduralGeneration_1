using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class MapGenerator
{
    [Header("Perlin Map Generator Params")]
    [SerializeField] private TerrainObject _terrainObj;

    // Temperature map
    private float[,] _temperatureMap;
    public void SetTemperatureMap(float[,] temperatureMap) => _temperatureMap = temperatureMap;

    // Height map
    private float[,] _heightMap;
    public void SetHeightMap(float[,] heightMap) => _heightMap = heightMap;

    // Variables
    private Tile[,] _tiles;

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
                float temperature = _temperatureMap[x, y];

                foreach (Terrain terrain in _terrainObj.Terrains)
                {
                    if (elevation >= terrain.MinHeight && elevation < terrain.MaxHeight)
                    {
                        Biome biome = SelectBiome(temperature, terrain);
                        if (biome != null)
                        {
                            _tiles[x, y] = biome.Tile;
                        }
                        else
                        {
                            Debug.LogWarning($"No valid biome for temperature {temperature} at ({x}, {y})");
                            _tiles[x, y] = null; // TODO: Assign a default tile.
                        }

                        _tiles[x, y] = biome.Tile;
                        break;
                    }
                }
            }
        }

        return _tiles;
    }

    private Biome SelectBiome(float temperature, Terrain terrain)
    {
        List<Biome> biomes = new List<Biome>();

        // Collect matching biomes
        foreach (Biome biome in terrain.Biomes)
        {
            if (temperature <= biome.MaxTemperature)
            {
                biomes.Add(biome);
            }
        }

        // Handle no matching biomes
        if (biomes.Count == 0)
        {
            Debug.LogWarning($"No biome matches temperature {temperature} in terrain {terrain.Name}");
            return null; // Or assign a default biome
        }

        // Select a random biome from the matches
        int randomIndex = Random.Range(0, biomes.Count);
        return biomes[randomIndex];
    }

}
