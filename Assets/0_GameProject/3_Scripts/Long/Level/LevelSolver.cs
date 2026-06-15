using System.Collections.Generic;
using UnityEngine;

public static class LevelSolver
{
    public static bool HasPath(
        Vector2Int start,
        Vector2Int exitPos)
    {
        Queue<Vector2Int> queue =
            new Queue<Vector2Int>();

        HashSet<Vector2Int> visited =
            new HashSet<Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);

        Vector2Int[] dirs =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        while (queue.Count > 0)
        {
            Vector2Int current =
                queue.Dequeue();

            if (current == exitPos)
                return true;

            foreach (var dir in dirs)
            {
                Vector2Int next =
                    current + dir;

                if (!GridManager.Instance
                    .IsInsideGrid(next))
                    continue;

                if (!GridManager.Instance
                    .IsCellFree(next))
                    continue;

                if (visited.Contains(next))
                    continue;

                visited.Add(next);

                queue.Enqueue(next);
            }
        }

        return false;
    }
}
