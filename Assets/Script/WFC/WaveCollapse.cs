using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class WaveCollapse
{
    // Variables
    private Cell[,] _cells;
    private TileRule _selectedCell;
    private int _width, _height;
    private TileBase[,] _tiles;

    public WaveCollapse(int width, int height, List<TileRule> tileRules)
    {
        // Initialise 2D Cells of width and height
        _cells = new Cell[width, height];
        _tiles = new TileBase[width, height];
        _width = width;
        _height = height;

        // Instantiate each cells
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // By default, the cell should not be collapsed and we give it all N tiles
                _cells[x, y] = new Cell(false, new List<TileRule>(tileRules));
            }
        }
    }

    public TileBase[,] GetTiles()
    {
        while (!IsComplete())
        {
            // Find the cell with the lowest entropy that is not collapsed
            (int x, int y) = FindLowestEntropyCell();

            // Collapse the selected cell
            Collapse(x, y);

            // Update the tiles array
            _tiles[x, y] = _cells[x, y].Options[0].tile;
        }

        return _tiles;
    }

    private (int, int) FindLowestEntropyCell()
    {
        int minEntropy = int.MaxValue;
        int selectedX = -1, selectedY = -1;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (!_cells[x, y].IsCollapsed && _cells[x, y].Entropy < minEntropy)
                {
                    minEntropy = _cells[x, y].Entropy;
                    selectedX = x;
                    selectedY = y;
                }
            }
        }

        return (selectedX, selectedY);
    }


    private bool IsComplete()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (!_cells[x, y].IsCollapsed)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void Collapse(int x, int y)
    {
        // Calculate cumulative probabilities
        float totalWeight = 0f;
        foreach (TileRule option in _cells[x, y].Options)
        {
            totalWeight += option.probability;
        }

        // Choose a random value within the cumulative weight
        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;
        foreach (TileRule option in _cells[x, y].Options)
        {
            cumulativeWeight += option.probability;
            if (randomValue <= cumulativeWeight)
            {
                _selectedCell = option;
                break;
            }
        }

        // Collapse the cell and update tile
        _cells[x, y].IsCollapsed = true;
        _cells[x, y].Options = new List<TileRule> { _selectedCell };

        // Start propagating neighboring cells
        Propagate(x, y);
    }

    private void Propagate(int x, int y)
    {
        // Define directions
        Vector2Int[] directions = new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        // Iterate through each direction
        foreach (Vector2Int direction in directions)
        {
            // Calculate offsets
            int newX = x + direction.x;
            int newY = y + direction.y;

            // Check if the new coordinates are within bounds
            if (newX >= 0 && newY >= 0 && newX < _cells.GetLength(0) && newY < _cells.GetLength(1))
            {
                // Only propagate if the neighboring cell is not collapsed
                if (!_cells[newX, newY].IsCollapsed)
                {
                    _cells[newX, newY].UpdateCell(_selectedCell, direction);
                }
            }
        }
    }
}

public struct Cell
{
    public bool IsCollapsed;
    public List<TileRule> Options;
    public int Entropy;

    public Cell(bool isCollapsed, List<TileRule> options)
    {
        IsCollapsed = isCollapsed;
        Options = options;
        Entropy = options.Count;
    }

    public void UpdateCell(TileRule selectedCell, Vector2Int direction)
    {
        List<TileRule> validOptions = new List<TileRule>();

        foreach (TileRule rule in Options)
        {
            if (IsCompatible(selectedCell, rule, direction))
                validOptions.Add(rule);
        }

        Options = validOptions;
        Entropy = validOptions.Count;
    }

    public readonly bool IsCompatible(TileRule selectedCell, TileRule rule, Vector2Int direction)
    {
        return direction switch
        {
            var _ when direction == Vector2Int.up => selectedCell.upNeighbors.Contains(rule),
            var _ when direction == Vector2Int.down => selectedCell.downNeighbors.Contains(rule),
            var _ when direction == Vector2Int.left => selectedCell.leftNeighbors.Contains(rule),
            var _ when direction == Vector2Int.right => selectedCell.rightNeighbors.Contains(rule),
            _ => false,
        };
    }
}
