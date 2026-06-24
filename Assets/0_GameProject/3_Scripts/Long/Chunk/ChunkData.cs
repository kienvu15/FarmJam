using UnityEngine;

[CreateAssetMenu(menuName = "Chunk Data")]
public class ChunkData : ScriptableObject
{
    public int width = 4;
    public int height = 4;

    public ChunkCellData[] cells;
}
