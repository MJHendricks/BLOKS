using UnityEngine;
using UnityEngine.Tilemaps;

public class board : MonoBehaviour
{
    public shapeData[] shapes;
    public Tilemap tilemap { get; private set; }
    public piece active { get; private set; }
    public Vector3Int spawnpoint;
    public Vector2Int boardSize = new Vector2Int(10, 20);

    public RectInt Bounds
    {
        get
        {
            Vector2Int pos = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(pos, boardSize);
        }
    }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.active = GetComponentInChildren<piece>();
        

        for (int i = 0; i < this.shapes.Length; i++)
        {
            this.shapes[i].init();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, this.shapes.Length);
        shapeData data = this.shapes[random];
        this.active.init(this, this.spawnpoint, data);

        if (isValidPos(this.active, this.spawnpoint))
        {
            Set(this.active);
        }
        else
        {
            GameOver();
        }
        
    }

    private void GameOver()
    {
        this.tilemap.ClearAllTiles();
    }

    public void Set(piece p )
    {
        for (int i = 0; i< p.cells.Length; i++)
        {
            Vector3Int tilePos = p.cells[i] + p.pos;
            this.tilemap.SetTile(tilePos, p.data.tile);
        }
    }

    public void Clear(piece p)
    {
        for (int i = 0; i < p.cells.Length; i++)
        {
            Vector3Int tilePos = p.cells[i] + p.pos;
            this.tilemap.SetTile(tilePos, null);
        }
    }

    public bool isValidPos(piece p, Vector3Int pos)
    {
        RectInt bounds = this.Bounds;
        for (int i = 0; i < p.cells.Length; i++)
        {
            Vector3Int tilePos = p.cells[i] + pos;
            if (!bounds.Contains((Vector2Int)tilePos))
            {
                return false;
            }
            if (this.tilemap.HasTile(tilePos))
            {
                return false;
            }

        }
        return true; 
    }

    public void ClearLine()
    {
        RectInt bounds = this.Bounds;
        int row = Bounds.yMin;
        
        while (row < Bounds.yMax)
        {
            if(lineFull(row))
            {
                LineClear(row);
            }
            else
            {
                row++;
            }
        }
    }

    private bool lineFull (int row)
    {
        RectInt bounds = this.Bounds;
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int pos = new Vector3Int(col, row, 0);
            if (!this.tilemap.HasTile(pos))
            {
                return false;
            }
        }
        return true;
    }

    private void LineClear(int row)
    {
        RectInt bounds = this.Bounds;
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int pos = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(pos, null);
        }

        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int pos = new Vector3Int(col, row +1, 0);
                TileBase above = this.tilemap.GetTile(pos);
                pos = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(pos, above);
            }
            row++;
        }
    }

}
