public interface IDataService
{
    int GetCurrentLevel();
    void SetCurrentLevel(int level);
    int GetBuildingLevel(string buildingId);
    void SaveBuildingLevel(string buildingId, int level);
    void SaveGame();
    void LoadGame();
}