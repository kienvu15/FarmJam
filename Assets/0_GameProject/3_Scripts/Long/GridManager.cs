using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public int width = 8;
    public int height = 8;

    public float cellSize = 2f;

    public GridCell[,] grid;

    private void Awake()
    {
        Instance = this;

        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new GridCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new GridCell();

                grid[x, y].cellType = CellType.Empty;
            }
        }
    }

    public bool IsInsideGrid(Vector2Int pos)
    {
        return pos.x >= 0 &&
               pos.x < width &&
               pos.y >= 0 &&
               pos.y < height;
    }

    public bool IsCellFree(Vector2Int pos)
    {
        if (!IsInsideGrid(pos))
            return false;

        return grid[pos.x, pos.y].cellType == CellType.Empty;
    }

    public Vector3 GridToWorld(Vector2Int pos)
    {
        return new Vector3(
            pos.x * cellSize,
            0,
            pos.y * cellSize
        );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos =
                    new Vector3(
                        x * cellSize,
                        0,
                        y * cellSize);

                Gizmos.DrawWireCube(
                    pos,
                    new Vector3(
                        cellSize,
                        0.1f,
                        cellSize));
            }
        }
    }
}
