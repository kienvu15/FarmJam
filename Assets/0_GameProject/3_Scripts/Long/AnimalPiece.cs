using UnityEngine;
using System.Collections;

public class AnimalPiece : MonoBehaviour
{
    public Vector2Int gridPosition;
    private bool isMoving;

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
        if (isMoving)
            return false;

        Vector2Int currentPos =
            gridPosition;

        while (true)
        {
            Vector2Int next =
                currentPos + dir;

            if (!GridManager.Instance
                .IsCellFree(next))
            {
                break;
            }

            currentPos = next;
        }

        if (currentPos == gridPosition)
            return false;

        GridManager.Instance.grid
            [gridPosition.x, gridPosition.y]
            .cellType = CellType.Empty;

        gridPosition = currentPos;

        GridManager.Instance.grid
            [gridPosition.x, gridPosition.y]
            .cellType = CellType.Occupied;

        StartCoroutine(
            SmoothMove(
                GridManager.Instance
                .GridToWorld(gridPosition)));

        return true;
    }

}
