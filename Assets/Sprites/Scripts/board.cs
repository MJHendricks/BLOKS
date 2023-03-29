using UnityEngine;

public class board : MonoBehaviour
{
    public shapeData[] shapes;

    private void Awake()
    {
        for (int i = 0; i < this.shapes.length; i++)
        {
            this.shapes[i].init();
        }
    }
}
