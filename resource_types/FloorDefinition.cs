using Godot;
using Godot.Collections;
using System.Diagnostics;

namespace wizardtower.resource_types;

[Tool]
[Icon("res://resource_types/floor-icon.svg")]
[GlobalClass]
[LoadDefinitions("res://floors/")]
[DebuggerDisplay("{Name}")]
public partial class FloorDefinition : Resource, INamedResource<FloorDefinition>
{
    [Export]
    public string? Name { get; set; }

    [Export]
    public Texture2D? Icon { get; set; }

    [Export]
    public string? Readme { get; set; }

    [Export]
    public string? Description { get; set; }

    [Export]
    public PackedScene? FloorBackgroundTileScene { get; set; }

    [Export]
    public NumericDict<ItemDefinition, uint> CostToBuildPerUnit { get; set; } = [];

    [Export]
    public Array<int>? CanBuildAtElevations { get; set; }

    [Export]
    public bool CanBuildInBasement { get; set; }

    public bool CanBuildFloorAt(int elevation)
    {
        if (elevation < 0 && CanBuildInBasement)
            return true;
        return CanBuildAtElevations is null || CanBuildAtElevations.Contains(elevation);
    }
}
