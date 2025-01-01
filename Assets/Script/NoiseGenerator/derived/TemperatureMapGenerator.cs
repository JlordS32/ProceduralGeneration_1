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
    public float[,] TemperatureMap { get { return _tempMap; } }

    public override Tile[,] Generate(int width, int height)
    {
        // Generate the Perlin noise map
        float[,] noiseMap = Noise.GenerateNoiseMap(width, height, _seed, _noiseScale, _octaves, _lacunarity, _persistance, _offset);

        // Initialize the Tile array
        _tiles = new Tile[width, height];
        _tempMap = new float[width, height];

        // Map the noise values to terrain types
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float currentHeight = noiseMap[x, y];
                // Calculate the interporation between min and max temperatured base on noise value.
                float temperature = Mathf.Lerp(_minTemperature, _maxTemperature, currentHeight);

                foreach (WeatherZone zone in _weatherZones.weatherZones)
                {
                    if (temperature <= zone.temperature)
                    {
                        _tiles[x, y] = zone.tile;
                        _tempMap[x, y] = temperature;
                        break;
                    }
                }
            }
        }

        return _tiles;
    }
}