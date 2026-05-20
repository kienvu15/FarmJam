using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public ChunkData[] chunks;

    public AnimalPiece animalPrefab;

    public Fence fencePrefab;

    [Header("Chunk Settings")]
    public int chunkCountX = 2;

    public int chunkCountY = 2;

    public int chunkSize = 4;

    [Header("Shuffle")]
    public int shuffleMoves = 50;

    [Header("Difficulty")]
    public int currentLevel = 1;

    void Start()
    {
        GenerateLevel();
        ApplyDifficulty();
    }

    void GenerateLevel()
    {
        ApplyDifficulty();
        ClearLevel();

        GridManager.Instance.ResizeGrid(
        chunkCountX * chunkSize,
        chunkCountY * chunkSize);

        for (int x = 0; x < chunkCountX; x++)
        {
            for (int y = 0; y < chunkCountY; y++)
            {
                SpawnRandomChunk(x, y);
            }
        }

        ShuffleLevel(shuffleMoves);
    }

    void SpawnRandomChunk(int x, int y)
    {
        ChunkData randomChunk =
            chunks[
                Random.Range(0, chunks.Length)];

        int randomRotation =
            GetRandomRotation();

        Vector2Int offset =
            new Vector2Int(
                x * chunkSize,
                y * chunkSize);

        PlaceChunk(
            randomChunk,
            offset,
            randomRotation);
    }

    int GetRandomRotation()
    {
        int[] rotations =
        {
        0,
        90,
        180,
        270
    };

        return rotations[
            Random.Range(
                0,
                rotations.Length)];
    }

    void ClearLevel()
    {
        AnimalPiece[] animals =
            FindObjectsOfType<AnimalPiece>();

        foreach (var animal in animals)
        {
            Destroy(animal.gameObject);
        }

        Fence[] fences =
            FindObjectsOfType<Fence>();

        foreach (var fence in fences)
        {
            Destroy(fence.gameObject);
        }

        ResetGrid();
    }

    void ResetGrid()
    {
        for (int x = 0;
             x < GridManager.Instance.width;
             x++)
        {
            for (int y = 0;
                 y < GridManager.Instance.height;
                 y++)
            {
                GridManager.Instance.grid[x, y]
                    .cellType = CellType.Empty;
            }
        }
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

    void ApplyDifficulty()
    {
        chunkCountX =
            Mathf.Clamp(
                2 + currentLevel / 5,
                2,
                6);

        chunkCountY =
            Mathf.Clamp(
                2 + currentLevel / 5,
                2,
                6);

        shuffleMoves =
            20 + currentLevel * 5;
    }
}