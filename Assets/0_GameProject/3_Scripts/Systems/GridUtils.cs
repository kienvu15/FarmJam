using UnityEngine;

public static class GridUtils
{
    public static float CellSize = 1.0f; 
    public static Vector3 GridOrigin = Vector3.zero; 

    public static Vector3 GetWorldPosition(Vector2Int gridPos)
    {
        float x = GridOrigin.x + (gridPos.x * CellSize);
        float z = GridOrigin.y + (gridPos.y * CellSize); 
        return new Vector3(x, 0f, z); 
    }

    public static Vector2Int GetGridPosition(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt((worldPos.x - GridOrigin.x) / CellSize);
        int z = Mathf.RoundToInt((worldPos.z - GridOrigin.y) / CellSize);
        return new Vector2Int(x, z);
    }
}