using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class HeightMapGenerator : NoiseGenerator
{
    [Header("Height Map Params")]
    [SerializeField] private HeightZoneObject _heightZoneObj;
    [SerializeField] private float _minHeight;
    [SerializeField] private float _maxHeight;
    [SerializeField] private float _heightOffset;
    
    [Header("Adjust Noise Params to offset borders")]
    [SerializeField] private float _borderSize; 
    [SerializeField] private float _borderScaleOffset; 

    // Variables
    private float[,] _heightMap;
    private float[,] _noiseMap;

    public float[,] HeightMap { get { return _heightMap; } }

    public override void Generate(int width, int height)
    {
        _noiseMap = Noise.GenerateNoiseMap(width, height, _seed, _noiseScale, _octaves, _lacunarity, _persistance, _offset);
        _heightMap = new float[width, height];
        _tiles = new Tile[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float currentHeight = _noiseMap[x, y];

                float elevation = Mathf.Lerp(_minHeight, _maxHeight, currentHeight) + _heightOffset;

                foreach (HeightZone zone in _heightZoneObj.HeightZones)
                {
                    if (elevation >= zone.MinHeight && elevation < zone.MaxHeight)
                    {
                        _heightMap[x, y] = elevation;
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

                float elevation = Mathf.Lerp(_minHeight, _maxHeight, currentHeight) + _heightOffset;

                foreach (HeightZone zone in _heightZoneObj.HeightZones)
                {
                    if (elevation >= zone.MinHeight && elevation < zone.MaxHeight)
                    {
                        _tiles[x, y] = zone.Tile;
                        break;
                    }
                }
            }
        }

        return _tiles;
    }
}
