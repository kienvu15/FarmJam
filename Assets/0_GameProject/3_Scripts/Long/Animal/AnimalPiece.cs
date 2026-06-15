using UnityEngine;
using System.Collections;

public class AnimalPiece : MonoBehaviour
{
    #region Variables

    public Vector2Int gridPosition;
    private bool isMoving;
    public bool isHorizontal;

    #endregion

    #region Setup

    public void Setup(Vector2Int pos)
    {
        gridPosition = pos;

        transform.position =
            GridManager.Instance.GridToWorld(pos);

        OccupyCell();
    }

    void OccupyCell()
    {
        GridManager.Instance.grid
            [gridPosition.x, gridPosition.y]
            .cellType = CellType.Occupied;
    }

    #endregion

    #region Move

    public bool TryMove(Vector2Int dir)
    {
        if (isMoving)
            return false;

        Vector2Int target =
            gridPosition + dir;

        if (!GridManager.Instance.IsCellFree(target))
            return false;

        GridManager.Instance.grid
            [gridPosition.x, gridPosition.y]
            .cellType = CellType.Empty;

        gridPosition = target;

        GridManager.Instance.grid
            [gridPosition.x, gridPosition.y]
            .cellType = CellType.Occupied;

        StartCoroutine(
            SmoothMove(
                GridManager.Instance.GridToWorld(
                    gridPosition)));

        return true;
    }

    IEnumerator SmoothMove(Vector3 target)
    {
        isMoving = true;

        Vector3 start =
            transform.position;

        float time = 0;

        float duration = 0.15f;

        while (time < duration)
        {
            time += Time.deltaTime;

            transform.position =
                Vector3.Lerp(
                    start,
                    target,
                    time / duration);

            yield return null;
        }

        transform.position = target;

        isMoving = false;
    }

    public bool SlideMove(Vector2Int dir)
    {
        if (isHorizontal)
        {
            if (dir != Vector2Int.left &&
                dir != Vector2Int.right)
            {
                return false;
            }
        }
        else
        {
            if (dir != Vector2Int.up &&
                dir != Vector2Int.down)
            {
                return false;
            }
        }

        if (isMoving)
            return false;

        Vector2Int currentPos =
            gridPosition;

        bool escaped = false;

        while (true)
        {
            Vector2Int next =
                currentPos + dir;

            if (!GridManager.Instance.IsInsideGrid(next))
            {
                escaped = true;
                break;
            }

            if (!GridManager.Instance.IsCellFree(next))
            {
                break;
            }

            currentPos = next;
        }

        if (currentPos == gridPosition && !escaped)
            return false;

        GridManager.Instance.grid
            [gridPosition.x, gridPosition.y]
            .cellType = CellType.Empty;

        gridPosition = currentPos;

        GridManager.Instance.grid
            [gridPosition.x, gridPosition.y]
            .cellType = CellType.Occupied;

        if (escaped)
        {
            StartCoroutine(
                EscapeMove(dir));
        }
        else
        {
            StartCoroutine(
                SmoothMove(
                    GridManager.Instance
                    .GridToWorld(gridPosition)));
        }

        return true;
    }

    public bool SlideMoveForShuffle(Vector2Int dir)
    {
        if (isMoving)
            return false;

        Vector2Int currentPos = gridPosition;

        while (true)
        {
            Vector2Int next = currentPos + dir;

            if (!GridManager.Instance.IsInsideGrid(next))
            {
                break;
            }

            if (!GridManager.Instance.IsCellFree(next))
            {
                break;
            }

            currentPos = next;
        }

        if (currentPos == gridPosition)
            return false;

        GridManager.Instance.grid[
            gridPosition.x,
            gridPosition.y]
            .cellType = CellType.Empty;

        gridPosition = currentPos;

        GridManager.Instance.grid[
            gridPosition.x,
            gridPosition.y]
            .cellType = CellType.Occupied;

        transform.position =
            GridManager.Instance.GridToWorld(gridPosition);

        return true;
    }

    #endregion

    #region Escape

    public void Escape()
    {
        GridManager.Instance.grid
            [gridPosition.x, gridPosition.y]
            .cellType = CellType.Empty;

        Destroy(gameObject);
    }

    IEnumerator EscapeMove(Vector2Int dir)
    {
        isMoving = true;

        GridManager.Instance.grid
            [gridPosition.x, gridPosition.y]
            .cellType = CellType.Empty;

        Vector3 target =
            transform.position +
            new Vector3(
                dir.x * 4,
                0,
                dir.y * 4);

        Vector3 start =
            transform.position;

        float time = 0;

        while (time < 0.3f)
        {
            time += Time.deltaTime;

            transform.position =
                Vector3.Lerp(
                    start,
                    target,
                    time / 0.3f);

            yield return null;
        }

        Debug.Log("Animal escaped: " + gridPosition);

        Destroy(gameObject);
    }

    #endregion
}