using UnityEngine;
using UnityEngine.Tilemaps;

public enum Shapes
{
    I,
    O,
    T,
    J,
    L,
    S,
    Z
}

[System.Serializable]
public struct shapeData
{
    public Shapes shape;
    public Tile tile;
    public Vector2Int[] cells { get; private set; }
    public Vector2Int[,] wallKicks { get; private set; }

    public void init()
    {
        this.cells = Data.Cells[this.shape];
        this.wallKicks = Data.WallKicks[this.shape];
    }
}