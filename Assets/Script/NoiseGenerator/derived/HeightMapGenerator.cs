using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class HeightMapGenerator : NoiseGenerator
{
    [Header("Height Map Params")]
    [SerializeField] private HeightZoneObject _heightZoneObj;
    [SerializeField] private float _minHeight;
    [SerializeField] private float _maxHeight;

    // Variables
    private float[,] _heightMap;

    public float[,] HeightMap { get { return _heightMap; } }

    public override Tile[,] Generate(int width, int height)
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(width, height, _seed, _noiseScale, _octaves, _lacunarity, _persistance, _offset);

        // Initialize the Tile array
        _tiles = new Tile[width, height];
        _heightMap = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float currentHeight = noiseMap[x, y];
                float elevation = Mathf.Lerp(_minHeight, _maxHeight, currentHeight);

                foreach (HeightZone zone in _heightZoneObj.HeightZones)
                {
                    if (elevation >= zone.MinHeight && elevation < zone.MaxHeight)
                    {
                        _tiles[x, y] = zone.Tile;
                        _heightMap[x, y] = elevation;
                        break;
                    }
                }
            }
        }

        return _tiles;
    }
}