using Godot;

namespace wizardtower;

public static class ControlExtensions
{
    public static string LineHeightImage(this Control label, Texture2D? texture, int fontSize = 16, string fontName = "default", bool fullHeight = true)
    {
        if (string.IsNullOrWhiteSpace(texture?.ResourcePath))
            return "[i]image not found[/i]";
        var font = label.GetThemeFont(fontName);
        var height = font.GetHeight(fontSize);
        if (!fullHeight)
        {
            height -= font.GetAscent() + font.GetDescent();
        }
        return $"[img height={height}]{texture?.ResourcePath}[/img]";
    }
}
