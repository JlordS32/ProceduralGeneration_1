using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class NoiseGenerator {
    [Header ("Base Noise Generator Params")]
    [SerializeField] protected float _noiseScale = 1f;
    [SerializeField] protected int _seed;
    [SerializeField] protected int _octaves = 4;
    [SerializeField] protected float _lacunarity;
    [Range(0, 1)]
    [SerializeField] protected float _persistance = 0.5f;
    [SerializeField] protected Vector2 _offset;

    // Variables
    protected Tile[,] _tiles;

    // Abstract methods
    public abstract Tile[,] Generate(int width, int height);
}