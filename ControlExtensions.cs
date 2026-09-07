using Godot;

namespace wizardtower;

public static class ControlExtensions
{
    public static string LineHeightImage(this Control _label, Texture2D? texture, string? url = null)
    {
        var text = "";
        if (string.IsNullOrWhiteSpace(texture?.ResourcePath))
            text = "[i]image not found[/i]";
        else
            text = $"[img height=1em]{texture?.ResourcePath}[/img]";
        if (url != null)
        {
            if (string.IsNullOrWhiteSpace(url))
                url = texture?.ResourcePath;
            text = $"[url={url}]{text}[/url]";
        }
        return text;
    }
}
