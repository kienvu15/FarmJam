using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;

    private void OnEnable()
    {
        if (Dev3GameContext.Currency != null)
        {
            Dev3GameContext.Currency.OnCoinChanged += UpdateCoinUI;
        }
    }

    private void Start()
    {
        if (coinText == null)
        {
            Debug.LogError("CoinUI lỗi: Chưa kéo CoinText vào ô Coin Text trong Inspector.");
            return;
        }

        int currentCoins = Dev3GameContext.Currency.GetCoins();
        UpdateCoinUI(currentCoins);
    }

    private void OnDisable()
    {
        if (Dev3GameContext.Currency != null)
        {
            Dev3GameContext.Currency.OnCoinChanged -= UpdateCoinUI;
        }
    }

    private void UpdateCoinUI(int coins)
    {
        if (coinText == null)
        {
            Debug.LogError("CoinUI lỗi: coinText đang bị null.");
            return;
        }

        coinText.text = coins.ToString();
    }
}