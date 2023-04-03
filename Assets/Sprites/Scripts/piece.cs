
using UnityEngine;

public class piece : MonoBehaviour
{
    public board board { get; private set; }
    public shapeData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int pos {  get; private set; }
    public int rotationIndex { get; private set; }

    public void init(board board, Vector3Int pos, shapeData data)
    {
        this.board = board;
        this.pos = pos;
        this.data = data;
        this.rotationIndex = 0;

        if (this.cells  == null )
        {
            this.cells = new Vector3Int[data.cells.Length];

        }
        for (int i = 0; i< data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int) data.cells[i];
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(-1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(1);
        }

        this.board.Clear(this);
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            Move(Vector2Int.right);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2Int.down);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Hardrop();
        }
        this.board.Set(this);
    }

    private void Hardrop() // make piece fall right to bottom
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }
    }

    private bool Move(Vector2Int trans) // move position of pieces along gameboard
    {
        Vector3Int newPos = this.pos;
        newPos.x += trans.x;
        newPos.y += trans.y;

        bool valid = this.board.isValidPos(this, newPos);

        if (valid)
        {
            this.pos = newPos;
        }
        return valid;
    }

    private void Rotate(int dir) // rotate pieces 
    {
        this.rotationIndex = Wrap(this.rotationIndex + dir, 0, 4);

        for (int i  = 0; i < this.cells.Length; i++)
        {
            Vector3 cell = this.cells[i];

            int x, y;
        }
    }

    private int Wrap(int input, int min, int max) //if index is out of bounds
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
}
