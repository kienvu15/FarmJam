public interface ICurrencyService
{
    int GetCoins();
    void AddCoins(int amount);
    bool TrySpendCoins(int amount);
    System.Action<int> OnCoinChanged { get; set; }
}