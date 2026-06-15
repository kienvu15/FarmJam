using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class LevelGenerator : MonoBehaviour
{

    public ChunkData[] chunks;

    public AnimalPiece animalPrefab;

    public Fence fencePrefab;

    public CameraController cameraController;

    private List<Vector2Int> freeCells = new List<Vector2Int>();

    public Vector2Int exitCell;

    [Header("Chunk Settings")]
    public int chunkCountX = 2;

    public int chunkCountY = 2;

    public int chunkSize = 4;

    [Header("Shuffle")]
    public int shuffleMoves = 0;

    [Header("Difficulty")]
    public int currentLevel = 1;

    [Header("Animal Spawn")]
    [Range(0.1f, 1f)]
    public float fillRate = 0.75f;


    void Start()
    {
        GenerateLevel();
        ApplyDifficulty();
    }

    #region Level
    void GenerateLevel()
    {
        exitCell =
           new Vector2Int(
            GridManager.Instance.width - 1,
            GridManager.Instance.height / 2);

        ApplyDifficulty();
        ClearLevel();

        GridManager.Instance.ResizeGrid(
        chunkCountX * chunkSize,
        chunkCountY * chunkSize);

        cameraController.FitCamera(
        GridManager.Instance.width,
        GridManager.Instance.height,
        GridManager.Instance.cellSize);

        for (int x = 0; x < chunkCountX; x++)
        {
            for (int y = 0; y < chunkCountY; y++)
            {
                SpawnRandomChunk(x, y);
            }
        }

        CollectFreeCells();

        SpawnAnimals();
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
    #endregion

    #region Chunk Placement
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
        if (content == CellContent.Fence)
        {
            Fence fence =
                Instantiate(fencePrefab);

            fence.Setup(pos);
        }
    }
    #endregion

    #region Difficulty
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
            100 + currentLevel * 5;
    }
    #endregion

    #region Animal Spawn
    void CollectFreeCells()
    {
        freeCells.Clear();

        for (int x = 0; x < GridManager.Instance.width; x++)
        {
            for (int y = 0; y < GridManager.Instance.height; y++)
            {
                if (GridManager.Instance.grid[x, y].cellType
                    == CellType.Empty)
                {
                    freeCells.Add(
                        new Vector2Int(x, y));
                }
            }
        }
    }

    void SpawnAnimals()
    {
        List<Vector2Int> shuffledCells =
            new List<Vector2Int>(freeCells);

        for (int i = 0; i < shuffledCells.Count; i++)
        {
            int randomIndex =
                Random.Range(
                    i,
                    shuffledCells.Count);

            Vector2Int temp =
                shuffledCells[i];

            shuffledCells[i] =
                shuffledCells[randomIndex];

            shuffledCells[randomIndex] =
                temp;
        }

        int animalTarget =
            Mathf.RoundToInt(
                shuffledCells.Count * fillRate);

        int animalCount = 0;
        for (int i = 0; i < animalTarget; i++)
        {
            Vector2Int cell =
                shuffledCells[i];
            bool canHorizontal =
                CanEscapeHorizontal(cell);

            bool canVertical =
                CanEscapeVertical(cell);

            if (!canHorizontal &&
                !canVertical)
            {
                continue;
            }

            AnimalPiece animal =
                Instantiate(animalPrefab);

            animalCount++;

            if (canHorizontal && canVertical)
            {
                animal.isHorizontal =
                    Random.value > 0.5f;
            }
            else
            {
                animal.isHorizontal =
                    canHorizontal;
            }

            animal.Setup(cell);

            if (animal.isHorizontal)
            {
                animal.transform.rotation =
                    Quaternion.Euler(
                        0,
                        90,
                        0);
            }
        }

        Debug.Log("Animal Spawned: " + animalCount);

        ReverseShuffle();
    }

    void ReverseShuffle()
    {
        AnimalPiece[] animals =
            FindObjectsOfType<AnimalPiece>();

        int successfulMoves = 0;

        int safety = 0;

        while (
            successfulMoves < shuffleMoves &&
            safety < shuffleMoves * 20)
        {
            safety++;

            AnimalPiece animal =
                animals[
                    Random.Range(
                        0,
                        animals.Length)];

            Vector2Int dir;

            if (animal.isHorizontal)
            {
                dir =
                    Random.value > 0.5f
                    ? Vector2Int.left
                    : Vector2Int.right;
            }
            else
            {
                dir =
                    Random.value > 0.5f
                    ? Vector2Int.up
                    : Vector2Int.down;
            }

            if (
                animal.SlideMoveForShuffle(
                    -dir))
            {
                successfulMoves++;
            }
        }

        Debug.Log(
            "Successful Shuffle = "
            + successfulMoves);
    }

    bool CanEscapeHorizontal(Vector2Int pos)
    {
        Vector2Int current = pos;

        while (true)
        {
            current += Vector2Int.left;

            if (!GridManager.Instance.IsInsideGrid(current))
                return true;

            if (!GridManager.Instance.IsCellFree(current))
                break;
        }

        current = pos;

        while (true)
        {
            current += Vector2Int.right;

            if (!GridManager.Instance.IsInsideGrid(current))
                return true;

            if (!GridManager.Instance.IsCellFree(current))
                break;
        }

        return false;
    }

    bool CanEscapeVertical(Vector2Int pos)
    {
        Vector2Int current = pos;

        while (true)
        {
            current += Vector2Int.up;

            if (!GridManager.Instance.IsInsideGrid(current))
                return true;

            if (!GridManager.Instance.IsCellFree(current))
                break;
        }

        current = pos;

        while (true)
        {
            current += Vector2Int.down;

            if (!GridManager.Instance.IsInsideGrid(current))
                return true;

            if (!GridManager.Instance.IsCellFree(current))
                break;
        }

        return false;
    }
    #endregion
}