using System;
using UnityEngine;

public class FarmBuilding : MonoBehaviour
{
    [Header("Building Identity")]
    [SerializeField] private string buildingId = "NhaKho";
    [SerializeField] private string displayName = "Nhà Kho";

    [Header("Building Visual Levels")]
    [SerializeField] private GameObject[] levelModels;

    [Header("Upgrade Costs")]
    [SerializeField] private int[] upgradeCosts = { 100, 250 };

    private int currentLevel;

    public event Action<FarmBuilding> OnBuildingChanged;

    public string BuildingId => buildingId;
    public string DisplayName => displayName;
    public int CurrentLevel => currentLevel;

    public int MaxLevel
    {
        get
        {
            if (levelModels == null || levelModels.Length == 0)
            {
                return 0;
            }

            return levelModels.Length - 1;
        }
    }

    private void Start()
    {
        if (!IsSetupValid())
        {
            return;
        }

        LoadBuildingLevel();
        RefreshVisual();
    }

    public bool CanUpgrade()
    {
        if (!IsSetupValid())
        {
            return false;
        }

        return currentLevel < MaxLevel;
    }

    public int GetUpgradeCost()
    {
        if (!CanUpgrade())
        {
            return 0;
        }

        if (upgradeCosts == null || currentLevel >= upgradeCosts.Length)
        {
            Debug.LogError($"{displayName} lỗi: Thiếu Upgrade Cost cho level {currentLevel}.");
            return 0;
        }

        return upgradeCosts[currentLevel];
    }

    public void TryUpgrade()
    {
        if (!CanUpgrade())
        {
            Debug.Log($"{displayName} đã đạt cấp tối đa hoặc chưa setup đúng.");
            return;
        }

        int cost = GetUpgradeCost();

        if (cost <= 0)
        {
            Debug.LogError($"{displayName} lỗi: Cost nâng cấp không hợp lệ.");
            return;
        }

        bool paidSuccessfully = Dev3GameContext.Currency.TrySpendCoins(cost);

        if (!paidSuccessfully)
        {
            Debug.Log("Không đủ tiền nâng cấp.");
            return;
        }

        currentLevel++;
        currentLevel = Mathf.Clamp(currentLevel, 0, MaxLevel);

        Dev3GameContext.Data.SaveBuildingLevel(buildingId, currentLevel);

        RefreshVisual();
        PlayUpgradeEffect();

        OnBuildingChanged?.Invoke(this);

        Debug.Log($"{displayName} đã nâng cấp lên level {currentLevel}.");
    }

    private void LoadBuildingLevel()
    {
        currentLevel = Dev3GameContext.Data.GetBuildingLevel(buildingId);
        currentLevel = Mathf.Clamp(currentLevel, 0, MaxLevel);
    }

    private void RefreshVisual()
    {
        if (!IsSetupValid())
        {
            return;
        }

        for (int i = 0; i < levelModels.Length; i++)
        {
            if (levelModels[i] != null)
            {
                levelModels[i].SetActive(i == currentLevel);
            }
        }
    }

    private bool IsSetupValid()
    {
        if (string.IsNullOrEmpty(buildingId))
        {
            Debug.LogError($"{gameObject.name} lỗi: Chưa nhập Building Id.");
            return false;
        }

        if (levelModels == null || levelModels.Length == 0)
        {
            Debug.LogError($"{gameObject.name} lỗi: Chưa gán Level Models trong Inspector.");
            return false;
        }

        for (int i = 0; i < levelModels.Length; i++)
        {
            if (levelModels[i] == null)
            {
                Debug.LogError($"{gameObject.name} lỗi: Level Models Element {i} đang trống.");
                return false;
            }
        }

        return true;
    }

    private void PlayUpgradeEffect()
    {
        transform.localScale = Vector3.one * 1.15f;
        Invoke(nameof(ResetScale), 0.15f);
    }

    private void ResetScale()
    {
        transform.localScale = Vector3.one;
    }
}