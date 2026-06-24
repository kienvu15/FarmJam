using UnityEngine;

public class FarmBuilding : MonoBehaviour
{
    [Header("Building Info")]
    [SerializeField] private string buildingId = "NhaKho";
    [SerializeField] private string displayName = "Nhà Kho";

    [Header("Level Settings")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int maxLevel = 3;
    [SerializeField] private int baseCapacity = 10;
    [SerializeField] private int capacityPerLevel = 5;
    [SerializeField] private int baseUpgradeCost = 50;
    [SerializeField] private int upgradeCostPerLevel = 25;

    [Header("Visual Models By Level")]
    [SerializeField] private GameObject[] levelModels;

    [Header("UI")]
    [SerializeField] private UpgradePanelUI upgradePanelUI;

    [Header("Effects")]
    [SerializeField] private GameObject selectedHighlight;
    [SerializeField] private ParticleSystem upgradeEffect;

    public string BuildingId => buildingId;
    public string DisplayName => displayName;
    public int CurrentLevel => currentLevel;
    public int CurrentCapacity => baseCapacity + ((currentLevel - 1) * capacityPerLevel);
    public int NextCapacity => baseCapacity + (currentLevel * capacityPerLevel);
    public int UpgradeCost => baseUpgradeCost + ((currentLevel - 1) * upgradeCostPerLevel);

    private void Start()
    {
        LoadBuildingLevel();
        RefreshVisual();

        if (selectedHighlight != null)
        {
            selectedHighlight.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        SelectBuilding();
    }

    public void SelectBuilding()
    {
        if (selectedHighlight != null)
        {
            selectedHighlight.SetActive(true);
        }

        if (upgradePanelUI != null)
        {
            upgradePanelUI.Show(this);
        }
        else
        {
            Debug.LogWarning($"[FarmBuilding] {displayName} chưa được gắn UpgradePanelUI.");
        }
    }

    public void UpgradeBuilding()
    {
        if (currentLevel >= maxLevel)
        {
            Debug.Log($"[FarmBuilding] {displayName} đã đạt cấp tối đa.");
            return;
        }

        if (GameContext.Currency == null)
        {
            Debug.LogWarning("[FarmBuilding] GameContext.Currency chưa sẵn sàng.");
            return;
        }

        bool success = GameContext.Currency.TrySpendCoins(UpgradeCost);

        if (!success)
        {
            Debug.Log($"[FarmBuilding] Không đủ tiền để nâng cấp {displayName}.");
            return;
        }

        currentLevel++;

        RefreshVisual();
        PlayUpgradeEffect();
        SaveBuildingLevel();

        Debug.Log($"[FarmBuilding] Đã nâng cấp {displayName} lên Lv.{currentLevel}.");
    }

    private void LoadBuildingLevel()
    {
        if (GameContext.Data == null)
        {
            Debug.LogWarning("[FarmBuilding] GameContext.Data chưa sẵn sàng. Dùng level mặc định trong Inspector.");
            return;
        }

        currentLevel = GameContext.Data.GetBuildingLevel(buildingId);

        if (currentLevel < 1)
        {
            currentLevel = 1;
        }

        if (currentLevel > maxLevel)
        {
            currentLevel = maxLevel;
        }
    }

    private void SaveBuildingLevel()
    {
        if (GameContext.Data == null)
        {
            Debug.LogWarning("[FarmBuilding] GameContext.Data chưa sẵn sàng. Chưa lưu được level.");
            return;
        }

        GameContext.Data.SaveBuildingLevel(buildingId, currentLevel);
    }

    private void RefreshVisual()
    {
        if (levelModels == null || levelModels.Length == 0)
        {
            return;
        }

        for (int i = 0; i < levelModels.Length; i++)
        {
            if (levelModels[i] == null)
            {
                continue;
            }

            bool shouldShow = i == currentLevel - 1;
            levelModels[i].SetActive(shouldShow);
        }
    }

    private void PlayUpgradeEffect()
    {
        if (upgradeEffect != null)
        {
            upgradeEffect.Play();
        }
    }
}