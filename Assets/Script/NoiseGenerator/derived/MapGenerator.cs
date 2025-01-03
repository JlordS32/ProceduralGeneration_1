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
    private WaveCollapse _waveCollapse;
    private int _width, _height;

    // TODO: Check constructor can work with waveCollapse function during runtime.
    public MapGenerator(int width, int height) {
        _width = width;
        _height = height;

        // // TODO: Memoize the fetched TileRules.
        // // Initialize the Tile array
        // List<TileRule> biomeTiles = new();
        // _tiles = new Tile[width, height];
        // foreach (Terrain terrain in _terrainObj.Terrains) {
        //     foreach (Biome biome in terrain.Biomes) {
        //         foreach(TileRule tileRule in biome.TileRules) {
        //             biomeTiles.Add(tileRule);
        //         }
        //     }
        // }

        // _waveCollapse = new WaveCollapse(width, height, biomeTiles);
    }

    // TODO: Use WaveCollapse Function to dynamically place tile depending on the region/terrain area.'
    public Tile[,] Generate(int width, int height)
    {
        InitialiseWFC(width, height);

        // Map the noise values to terrain types
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float elevation = _heightMap[x, y];
                float temperature = _temperatureMap[x, y];

                foreach (Terrain terrain in _terrainObj.Terrains)
                {
                    if (Utility.WithinRange(elevation, terrain.MinHeight, terrain.MaxHeight))
                    {
                        Biome biome = SelectBiome(temperature, terrain);
                        if (biome != null)
                        {
                            _tiles[x, y] = _waveCollapse.GetTile(x, y, biome.TileRules);
                        }
                        else
                        {
                            Debug.LogWarning($"No valid biome for temperature {temperature} at ({x}, {y})");
                            _tiles[x, y] = null; 
                        }

                        _tiles[x, y] = biome.Tile;
                        break;
                    }
                }
            }
        }

        return _tiles;
    }

    private void InitialiseWFC(int width, int height) {
        List<TileRule> biomeTiles = new();
        _tiles = new Tile[width, height];
        foreach (Terrain terrain in _terrainObj.Terrains) {
            foreach (Biome biome in terrain.Biomes) {
                foreach(TileRule tileRule in biome.TileRules) {
                    biomeTiles.Add(tileRule);
                }
            }
        }

        _waveCollapse = new WaveCollapse(width, height, biomeTiles);
    }

    private Biome SelectBiome(float temperature, Terrain terrain)
    {
        List<Biome> biomes = new();

        // Collect matching biomes
        foreach (Biome biome in terrain.Biomes)
        {
            if (Utility.WithinRange(temperature, biome.MinTemperature, biome.MaxTemperature))
            {
                biomes.Add(biome);
            }
        }

        // Handle no matching biomes
        if (biomes.Count == 0)
        {
            Debug.LogWarning($"No biome matches temperature {temperature} in terrain {terrain.Name}");
            return null; 
        }

        // Select a random biome from the matches
        int randomIndex = Random.Range(0, biomes.Count);
        return biomes[randomIndex];
    }

}
