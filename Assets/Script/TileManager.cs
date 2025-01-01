using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public enum Maptype { Perlin, Temperature, Height };
    [SerializeField] private Maptype _mapType;
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private bool _autoUpdate;

    // References
    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private TemperatureMapGenerator _temperatureMapGenerator;
    [SerializeField] private HeightMapGenerator _heightMapGenerator;

    // Properties
    public bool AutoUpdate { get { return _autoUpdate; } }

    private void Awake() {
        // Constructor initialisation.
        _mapGenerator = new MapGenerator();
        _temperatureMapGenerator = new TemperatureMapGenerator();
        _heightMapGenerator = new HeightMapGenerator();
    }

    // Method to generate the tilemap
    public void GenerateMap()
    {
        // Clear tiles before loading new ones.
        _tilemap.ClearAllTiles();

        // Calculate offset
        int xOffset = -_width / 2;
        int yOffset = -_height / 2;

        // Selecting map dynamically
        Tile[,] tiles;
        switch(_mapType) {
            case var _ when _mapType == Maptype.Perlin:
                BaseGenerate();
                tiles = _mapGenerator.Generate(_width, _height);
                break;
            case var _ when _mapType == Maptype.Temperature:
                tiles = _temperatureMapGenerator.Generate(_width, _height);
                break;
            case var _ when _mapType == Maptype.Height:
                tiles = _heightMapGenerator.Generate(_width, _height);
                break;
            default:
                tiles = new Tile[0, 0];
                Debug.LogWarning("No map type selected");
                break;
        }

        // Loop through the defined width and height to place tiles
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector3Int tilePosition = new(x + xOffset, y + yOffset, 0);
                _tilemap.SetTile(tilePosition, tiles[x, y]);
            }
        }
    }

    private void BaseGenerate() {
        if (_temperatureMapGenerator.TemperatureMap == null) {
            _temperatureMapGenerator.Generate(_width, _height);
            _mapGenerator.SetTemperatureMap(_temperatureMapGenerator.TemperatureMap);
        }
        if (_heightMapGenerator.HeightMap == null) {
            _heightMapGenerator.Generate(_width, _height);
            _mapGenerator.SetHeightMap(_heightMapGenerator.HeightMap);
        }
    }
}