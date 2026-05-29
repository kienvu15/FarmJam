using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonUI : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName = "Gameplay";

    public void PlayGame()
    {
        Debug.Log("PLAY button clicked.");

        if (Application.CanStreamedLevelBeLoaded(gameplaySceneName))
        {
            SceneManager.LoadScene(gameplaySceneName);
        }
        else
        {
            Debug.LogWarning("Chưa có scene Gameplay trong Build Settings. Tạm thời chỉ log khi bấm PLAY.");
        }
    }
}