using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI coinText;

    private void Start()
    {
        RefreshCoinText();

        if (GameContext.Currency != null)
        {
            GameContext.Currency.OnCoinChanged += UpdateCoinUI;
        }
        else
        {
            Debug.LogWarning("[CurrencyUI] GameContext.Currency chưa sẵn sàng.");
        }
    }

    private void OnDestroy()
    {
        if (GameContext.Currency != null)
        {
            GameContext.Currency.OnCoinChanged -= UpdateCoinUI;
        }
    }

    private void RefreshCoinText()
    {
        if (coinText == null)
        {
            Debug.LogWarning("[CurrencyUI] Chưa gắn coinText trong Inspector.");
            return;
        }

        if (GameContext.Currency == null)
        {
            coinText.text = "0";
            return;
        }

        int coins = GameContext.Currency.GetCoins();
        coinText.text = coins.ToString();
    }

    private void UpdateCoinUI(int newCoins)
    {
        if (coinText == null)
        {
            return;
        }

        coinText.text = newCoins.ToString();
    }
}