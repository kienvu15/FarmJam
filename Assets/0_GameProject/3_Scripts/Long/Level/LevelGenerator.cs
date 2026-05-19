using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public ChunkData[] chunks;

    public AnimalPiece animalPrefab;

    public Fence fencePrefab;

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        int chunkSize = 4;

        for (int x = 0; x < 2; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                ChunkData randomChunk =
                    chunks[
                        Random.Range(0, chunks.Length)];

                int[] rotations =
                {
                0,
                90,
                180,
                270
            };

                int randomRotation =
                    rotations[
                        Random.Range(
                            0,
                            rotations.Length)];

                Vector2Int offset =
                    new Vector2Int(
                        x * chunkSize,
                        y * chunkSize);

                PlaceChunk(
                    randomChunk,
                    offset,
                    randomRotation);
            }
        }
        ShuffleLevel(100);
    }

    void PlaceChunk(

        ChunkData chunk,
        Vector2Int offset,
        int rotation)
    {
        foreach (var cell in chunk.cells)
        {
            Vector2Int rotatedPos =
                GetRotatedPosition(
                    cell.position,
                    rotation);

            Vector2Int finalPos =
                rotatedPos + offset;

            if (!GridManager.Instance.IsInsideGrid(finalPos))
                continue;

            if (!GridManager.Instance.IsCellFree(finalPos))
                continue;

            SpawnCell(
                cell.content,
                finalPos);
        }
        Vector2Int GetRotatedPosition(
        Vector2Int pos,
            int rotation)
        {
            switch (rotation)
            {
                case 90:
                    return ChunkRotation.Rotate90(pos);

                case 180:
                    return ChunkRotation.Rotate180(pos);

                case 270:
                    return ChunkRotation.Rotate270(pos);

                default:
                    return pos;
            }
        }
    }

    void SpawnCell(
        CellContent content,
        Vector2Int pos)
    {
        switch (content)
        {
            case CellContent.Animal:

                AnimalPiece animal =
                    Instantiate(
                        animalPrefab);

                animal.Setup(pos);

                break;

            case CellContent.Fence:

                Fence fence =
                    Instantiate(
                        fencePrefab);

                fence.Setup(pos);

                break;
        }
    }
    
    void ShuffleLevel(int moveCount)
    {
        AnimalPiece[] animals =
            FindObjectsOfType<AnimalPiece>();

        Vector2Int[] dirs =
        {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

        for (int i = 0; i < moveCount; i++)
        {
            AnimalPiece randomAnimal =
                animals[
                    Random.Range(
                        0,
                        animals.Length)];

            Vector2Int randomDir =
                dirs[
                    Random.Range(
                        0,
                        dirs.Length)];

            randomAnimal.TryMove(randomDir);
        }
    }
}