
using UnityEngine;

public class piece : MonoBehaviour
{
    public board board { get; private set; }
    public shapeData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int pos {  get; private set; }
    public int rotationIndex { get; private set; }

    public float stepDelay = 0.5f;
    public float lockDelay = 0.5f;

    public float stepTime;
    public float lockTime;

    public void init(board board, Vector3Int pos, shapeData data)
    {
        this.board = board;
        this.pos = pos;
        this.data = data;
        this.rotationIndex = 0;
        this.stepTime = Time.time + this.stepDelay;
        this.lockTime = 0f;

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
        this.board.Clear(this);
        this.lockTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Q)) //rotate anticlockwise?
        {
            Rotate(-1);
        }
        if (Input.GetKeyDown(KeyCode.E)) // rotate clockwise
        {
            Rotate(1);
        }

        this.board.Clear(this);
        if (Input.GetKeyDown(KeyCode.A)) //move piece to the left
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D)) { //move piece to the right
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
        if (Time.time >= this.stepTime)
        {
            Step();
        }
        this.board.Set(this);
    }

    private void Hardrop() // make piece fall right to bottom
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }
        Lock();
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
            this.lockTime = 0f;
        }
        return valid;
    }

    private void Rotate(int dir) // rotate pieces 
    {
        int initRotation = this.rotationIndex;
        this.rotationIndex = Wrap(this.rotationIndex + dir, 0, 4);

        ApplyRotation(dir);

        if (!TestKicks(this.rotationIndex, dir))
        {
            this.rotationIndex = initRotation;
            ApplyRotation(-dir);
        }
    }

    private void ApplyRotation(int dir) //all the code for rotating blocks
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3 cell = this.cells[i];

            int x, y;

            switch (this.data.shape)
            {
                case Shapes.I:
                case Shapes.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * dir) + (cell.y * Data.RotationMatrix[1] * dir));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * dir) + (cell.y * Data.RotationMatrix[3] * dir));

                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * dir) + (cell.y * Data.RotationMatrix[1] * dir));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * dir) + (cell.y * Data.RotationMatrix[3] * dir));
                    break;
            }
            this.cells[i] = new Vector3Int(x, y, 0);
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

    private int GetWallKickIndex(int rotationIndex, int rotationdir)
    {
        int wallKickIndex = rotationIndex * 2;
        if (rotationIndex < 0)
        {
            wallKickIndex--;
        }
        return Wrap(wallKickIndex, 0 , this.data.wallKicks.GetLength(0));
    }

    private bool TestKicks(int rotationIndex, int rotationdir)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationdir);
        for (int i = 0; i <  this.data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i];
            if (Move(translation))
            {
                return true;
            }
        }
        return false;
    }

    private void Step()
    {
        this.stepTime = Time.time + this.stepDelay;
        Move(Vector2Int.down);

        if (this.lockTime >= this.lockDelay)
        {
            Lock();
        }
    }

    private void Lock()
    {
        this.board.Set(this);
        this.board.ClearLine();
        this.board.SpawnPiece();
    }
}
