public static class PathHelper
{
    public static string GetScenePathScaled(string innerPath)
    {
        if (innerPath.StartsWith('/'))
        {
            string text = innerPath;
            innerPath = text.Substring(1, text.Length - 1);
        }

        return "res://TheScaled/scenes/" + innerPath + ".tscn";
    }
}