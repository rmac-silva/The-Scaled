// RelicTracker.ModLog - Mod Logger borrowed from BetterSpire2
using Godot;

public static class ModLog
{
    private static string? _logPath;

    private static readonly object _lock = new object();

    private static string LogPath =>
        _logPath
        ?? (_logPath = Path.Combine(OS.GetUserDataDir(), "The Scaled", "thescaled.log"));

    public static void Init()
    {
        try
        {


            string? directory = Path.GetDirectoryName(LogPath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(
                LogPath,
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] The Scaled log started\n  OS: {OS.GetName()} / {OS.GetDistributionName()}\n  Godot: {Engine.GetVersionInfo()["string"]}\n"
            );
        }
        catch (Exception ex)
        {
            GD.PrintErr($"[The Scaled] Failed to initialize log: {ex.Message}");
        }
    }

    public static void Info(this object? caller, string message)
    {
        try
        {
            string senderName = caller?.GetType().Name ?? "Static";
            lock (_lock)
            {
                File.AppendAllText(LogPath, $"[{DateTime.Now:HH:mm:ss}] [{senderName}.cs] [INFO] {message}\n");
            }
        }
        catch { }
    }

    public static void Error(this object caller, string context, Exception ex)
    {
        try
        {
            string senderName = caller?.GetType().Name ?? "Static";
            lock (_lock)
            {
                File.AppendAllText(
                    LogPath,
                    $"[{DateTime.Now:HH:mm:ss}] [{senderName}.cs] [ERROR] in {context}: {ex}\n"
                );
            }
        }
        catch { }
    }

    public static void Warning(this object caller, string message)
    {
        try
        {
            string senderName = caller?.GetType().Name ?? "Static";
            lock (_lock)
            {
                File.AppendAllText(LogPath, $"[{DateTime.Now:HH:mm:ss}] [{senderName}.cs] [WARNING] {message}\n");
            }
        }
        catch { }
    }
}
