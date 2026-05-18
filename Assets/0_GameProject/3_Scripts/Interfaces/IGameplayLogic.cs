using UnityEngine;

public interface IGameplayLogic
{
    void InitializeLevel(int levelNumber);
    bool CanAnimalMove(Vector2Int gridPosition, Vector2Int direction);
    void SelectAnimal(Vector2Int gridPosition);
    System.Action<Vector2Int, Vector3[]> OnAnimalMoveApproved { get; set; }
    System.Action<int[]> OnAnimalsMerged { get; set; }
    System.Action OnLevelWon { get; set; }
    System.Action OnLevelLost { get; set; }
}