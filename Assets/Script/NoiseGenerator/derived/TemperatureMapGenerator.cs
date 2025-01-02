using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TemperatureMapGenerator : NoiseGenerator
{
    [Header("Temperature Map Params")]
    [SerializeField] private WeatherZoneObject _weatherZones;
    [SerializeField] private int _minTemperature;
    [SerializeField] private int _maxTemperature;

    // Variables
    private float[,] _tempMap;
    private float[,] _noiseMap;
    public float[,] TemperatureMap { get { return _tempMap; } }

    public override void Generate(int width, int height)
    {
        _noiseMap = Noise.GenerateNoiseMap(width, height, _seed, _noiseScale, _octaves, _lacunarity, _persistance, _offset);

        // Initialize the Tile array
        _tiles = new Tile[width, height];
        _tempMap = new float[width, height];

        // Map the noise values to terrain types
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float currentHeight = _noiseMap[x, y];
                // Calculate the interporation between min and max temperatured base on noise value.
                float temperature = Mathf.Lerp(_minTemperature, _maxTemperature, currentHeight);

                foreach (WeatherZone zone in _weatherZones.WeatherZones)
                {
                    if (temperature <= zone.Temperature)
                    {
                        _tiles[x, y] = zone.Tile;
                        _tempMap[x, y] = temperature;
                        break;
                    }
                }
            }
        }
    }

    public override Tile[,] GenerateTiles(int width, int height)
    {
        _noiseMap = Noise.GenerateNoiseMap(width, height, _seed, _noiseScale, _octaves, _lacunarity, _persistance, _offset);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float currentHeight = _noiseMap[x, y];
                // Calculate the interporation between min and max temperatured base on noise value.
                float temperature = Mathf.Lerp(_minTemperature, _maxTemperature, currentHeight);

                foreach (WeatherZone zone in _weatherZones.WeatherZones)
                {
                    if (temperature <= zone.Temperature)
                    {
                        _tiles[x, y] = zone.Tile;
                        _tempMap[x, y] = temperature;
                        break;
                    }
                }
            }
        }

        return _tiles;
    }
}