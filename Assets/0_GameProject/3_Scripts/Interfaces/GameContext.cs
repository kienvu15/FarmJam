public static class GameContext
{
    public static ICurrencyService Currency { get; set; }
    public static IDataService Data { get; set; }
    public static IGameplayLogic Gameplay { get; set; }
    public static ISoundService Sound { get; set; }
}