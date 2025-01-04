using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class MapGenerator
{
    [Header("Perlin Map Generator Params")]
    [SerializeField] private TerrainObject _terrainObj;
    [SerializeField] private Tile _defaultTile;

    // Temperature map
    private float[,] _temperatureMap;
    public void SetTemperatureMap(float[,] temperatureMap) => _temperatureMap = temperatureMap;

    // Height map
    private float[,] _heightMap;
    public void SetHeightMap(float[,] heightMap) => _heightMap = heightMap;

    // Variables
    private Tile[,] _tiles;
    private WaveCollapse _waveCollapse;
    private HashSet<TileRule> _cachedTileRules;
    private int _cachedTerrainHash;

    // TODO: Use WaveCollapse Function to dynamically place tile depending on the region/terrain area.'
    public Tile[,] Generate(int width, int height)
    {
        InitialiseWFC(width, height);

        // Initialise the tile 2D array
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
                    if (Utility.WithinRange(elevation, terrain.MinHeight, terrain.MaxHeight))
                    {
                        Biome biome = SelectBiome(temperature, terrain);
                        
                        if (biome != null)
                        {
                            Tile tile = _waveCollapse.GetTile(x, y, biome);

                            if (tile != null) {
                                _tiles[x, y] = tile;
                            } else {
                                _tiles[x, y] = _defaultTile;
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"No valid biome for temperature {temperature} at ({x}, {y})");
                            _tiles[x, y] = _defaultTile; // Default Tile
                        }

                        break;
                    }
                }
            }
        }

        return _tiles;
    }

    private void InitialiseWFC(int width, int height)
    {
        int currentHash = Utility.GetTerrainHash(_terrainObj);

        if (_cachedTileRules == null || _cachedTerrainHash != currentHash)
        {
            // Initialize _cachedTileRules if it's null
            _cachedTileRules ??= new HashSet<TileRule>();

            foreach (Terrain terrain in _terrainObj.Terrains)
            {
                foreach (Biome biome in terrain.Biomes)
                {
                    _cachedTileRules.UnionWith(biome.TileRules); 
                }
            }

            _cachedTerrainHash = currentHash;
            Debug.Log("Creating cached tile rules.");
        }
        else
        {
            Debug.Log(_cachedTileRules.Count);
            Debug.Log("Reusing cached tile");
        }

        _waveCollapse = new WaveCollapse(width, height, _cachedTileRules);
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
