using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class HeightMapGenerator : NoiseGenerator
{
    [Header ("Height Map Params")]
    [SerializeField] private  HeightZoneObject _heightZoneObj;
    [SerializeField] private float _maxHeight;
    
    // Variables
    private float[,] _heightMap;

    public float[,] HeightMap { get { return _heightMap; } }

    public override Tile[,] Generate(int width, int height)
    {
        float[,] heightMap = Noise.GenerateNoiseMap(width, height, _seed, _noiseScale, _octaves, _lacunarity, _persistance, _offset);

        // Initialize the Tile array
        _tiles = new Tile[width, height];
        _heightMap = new float[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                float currentHeight = heightMap[x, y];
                float elevation = Mathf.Lerp(-10, _maxHeight, currentHeight);

                foreach (HeightZone zone in _heightZoneObj.HeightZones) {
                    if (elevation <= zone.Height) {
                        _tiles[x, y] = zone.Tile;
                        break;
                    }
                }
            }
        }

        return _tiles;
    }
}