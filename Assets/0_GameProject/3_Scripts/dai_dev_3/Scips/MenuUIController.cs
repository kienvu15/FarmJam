using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject rewardPanel;

    [Header("Scene")]
    [SerializeField] private string gameplaySceneName = "";

    private void Start()
    {
        CloseAllPanels();
    }

    public void OnClickPlay()
    {
        if (string.IsNullOrWhiteSpace(gameplaySceneName))
        {
            Debug.Log("[MenuUIController] PLAY được bấm. Chưa gắn gameplaySceneName nên tạm thời chưa load scene.");
            return;
        }

        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OpenUpgradePanel()
    {
        CloseAllPanels();

        if (upgradePanel != null)
        {
            upgradePanel.SetActive(true);
        }
    }

    public void OpenSettingsPanel()
    {
        CloseAllPanels();

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    public void OpenRewardPanel()
    {
        CloseAllPanels();

        if (rewardPanel != null)
        {
            rewardPanel.SetActive(true);
        }
    }

    public void CloseAllPanels()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        if (rewardPanel != null)
        {
            rewardPanel.SetActive(false);
        }
    }
}