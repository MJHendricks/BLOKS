
using UnityEngine;

public class piece : MonoBehaviour
{
    public board board { get; private set; }
    public shapeData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int pos {  get; private set; }

    public void init(board board, Vector3Int pos, shapeData data)
    {
        this.board = board;
        this.pos = pos;
        this.data = data;

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
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            Move(Vector2Int.right);
        }
        this.board.Set(this);
    }

    private bool Move(Vector2Int trans)
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
}
