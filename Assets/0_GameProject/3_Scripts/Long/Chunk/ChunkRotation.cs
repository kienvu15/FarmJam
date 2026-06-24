using UnityEngine;

public static class ChunkRotation
{
    public static Vector2Int Rotate90(Vector2Int pos)
    {
        return new Vector2Int(
            pos.y,
            -pos.x
        );
    }

    public static Vector2Int Rotate180(Vector2Int pos)
    {
        return new Vector2Int(
            -pos.x,
            -pos.y
        );
    }

    public static Vector2Int Rotate270(Vector2Int pos)
    {
        return new Vector2Int(
            -pos.y,
            pos.x
        );
    }
}
