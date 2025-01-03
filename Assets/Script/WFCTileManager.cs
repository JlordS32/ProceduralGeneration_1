using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class WFCTileManager : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private List<TileRule> _tileRules;
    [SerializeField] private bool _autoUpdate;

    // References
    private WaveCollapse _waveCollapseFunction;

    // Properties
    public bool AutoUpdate { get { return _autoUpdate; } }

    // Method to generate the tilemap
    public void GenerateMap()
    {
        _tilemap.ClearAllTiles();

        int xOffset = -_width / 2;
        int yOffset = -_height / 2;

        _waveCollapseFunction = new WaveCollapse(_width, _height, _tileRules);
        TileBase[,] tiles = _waveCollapseFunction.GetTiles();

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
}