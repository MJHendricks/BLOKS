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

    public void init()
    {
        this.cells = data.Cells[this.shape];
    }
}