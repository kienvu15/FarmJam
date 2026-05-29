using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanelUI : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TMP_Text buildingNameText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text costText;

    [Header("Buttons")]
    [SerializeField] private Button upgradeButton;

    [Header("Default Selected Building")]
    [SerializeField] private FarmBuilding selectedBuilding;

    private void Start()
    {
        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(OnUpgradeClicked);
        }

        if (selectedBuilding != null)
        {
            selectedBuilding.OnBuildingChanged += HandleBuildingChanged;
            Refresh(selectedBuilding);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (upgradeButton != null)
        {
            upgradeButton.onClick.RemoveListener(OnUpgradeClicked);
        }

        if (selectedBuilding != null)
        {
            selectedBuilding.OnBuildingChanged -= HandleBuildingChanged;
        }
    }

    public void Show(FarmBuilding building)
    {
        if (selectedBuilding != null)
        {
            selectedBuilding.OnBuildingChanged -= HandleBuildingChanged;
        }

        selectedBuilding = building;

        if (selectedBuilding != null)
        {
            selectedBuilding.OnBuildingChanged += HandleBuildingChanged;
        }

        gameObject.SetActive(true);
        Refresh(selectedBuilding);
    }

    private void OnUpgradeClicked()
    {
        if (selectedBuilding == null)
        {
            Debug.LogWarning("Chưa chọn công trình để nâng cấp.");
            return;
        }

        selectedBuilding.TryUpgrade();
        Refresh(selectedBuilding);
    }

    private void HandleBuildingChanged(FarmBuilding building)
    {
        Refresh(building);
    }

    private void Refresh(FarmBuilding building)
    {
        if (building == null)
        {
            return;
        }

        buildingNameText.text = building.DisplayName;
        levelText.text = $"Level: {building.CurrentLevel}/{building.MaxLevel}";

        if (building.CanUpgrade())
        {
            costText.text = $"Cost: {building.GetUpgradeCost()}";
            upgradeButton.interactable = true;
        }
        else
        {
            costText.text = "Đã nâng cấp tối đa";
            upgradeButton.interactable = false;
        }
    }
}