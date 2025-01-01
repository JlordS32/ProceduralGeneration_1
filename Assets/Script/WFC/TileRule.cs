using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewTileRule", menuName = "WFC/TileRule")]
[System.Serializable]
public class TileRule : ScriptableObject
{
    public TileBase tile;
    public float probability = 0.5f;
    public List<TileRule> upNeighbors;
    public List<TileRule> downNeighbors;
    public List<TileRule> leftNeighbors;
    public List<TileRule> rightNeighbors;
}
