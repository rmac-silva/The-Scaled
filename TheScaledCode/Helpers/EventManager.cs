using MegaCrit.Sts2.Core.Entities.Players;

public static class EventManager
{
    public static event Action<Player> OnSetupAdded = delegate { };
    public static event Action<Player> OnAmbushTriggered = delegate { };

    public static void InvokeSetupAdded(Player player)
    {
        OnSetupAdded?.Invoke(player);
    }
    public static void InvokeAmbushTriggered(Player player)
    {
        OnAmbushTriggered?.Invoke(player);
    }
}