using UnityEngine;

public class Fence : MonoBehaviour
{
    public void Setup(Vector2Int pos)
    {
        transform.position =
            GridManager.Instance.GridToWorld(pos);

        GridManager.Instance.grid
            [pos.x, pos.y]
            .cellType = CellType.Fence;
    }
}
