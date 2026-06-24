using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanelUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI buildingNameText;
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private TextMeshProUGUI nextLevelText;
    [SerializeField] private TextMeshProUGUI effectText;
    [SerializeField] private TextMeshProUGUI costText;

    [Header("Button")]
    [SerializeField] private Button upgradeButton;

    private FarmBuilding currentBuilding;

    private void Awake()
    {
        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(OnClickUpgrade);
        }
    }

    private void OnDestroy()
    {
        if (upgradeButton != null)
        {
            upgradeButton.onClick.RemoveListener(OnClickUpgrade);
        }
    }

    public void Show(FarmBuilding building)
    {
        currentBuilding = building;

        if (currentBuilding == null)
        {
            Debug.LogWarning("[UpgradePanelUI] Không có building để hiển thị.");
            return;
        }

        gameObject.SetActive(true);
        Refresh();
    }

    public void Hide()
    {
        currentBuilding = null;
        gameObject.SetActive(false);
    }

    public void Refresh()
    {
        if (currentBuilding == null)
        {
            return;
        }

        if (buildingNameText != null)
        {
            buildingNameText.text = currentBuilding.DisplayName;
        }

        if (currentLevelText != null)
        {
            currentLevelText.text = $"Cấp hiện tại\nLv. {currentBuilding.CurrentLevel}";
        }

        if (nextLevelText != null)
        {
            nextLevelText.text = $"Cấp tiếp theo\nLv. {currentBuilding.CurrentLevel + 1}";
        }

        if (effectText != null)
        {
            effectText.text = $"Tăng sức chứa\n+{currentBuilding.CurrentCapacity}  ➜  +{currentBuilding.NextCapacity}";
        }

        if (costText != null)
        {
            costText.text = $"Chi phí nâng cấp\n⭐ {currentBuilding.UpgradeCost}";
        }
    }

    private void OnClickUpgrade()
    {
        if (currentBuilding == null)
        {
            return;
        }

        currentBuilding.UpgradeBuilding();
        Refresh();
    }
}