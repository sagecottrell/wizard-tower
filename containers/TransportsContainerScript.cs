using Godot;
using wizardtower.state;

namespace wizardtower.containers;

public partial class TransportsContainerScript(TowerScript tower) : Node3D()
{
    public TowerScript Tower { get; } = tower;
    public TowerState State { get; } = tower.State;

    public override void _Ready()
    {
        foreach (var transport in State.Transports.Values)
            SetupTransportationDisplay(transport);
    }


    public void SetupTransportationDisplay(TransportState newTransport)
    {
        var transport = new TransportScript()
        {
            State = newTransport,
        };
        AddChild(transport);

        //for (var i = 0; i < newTransport.Height; i++)
        //{
        //    if (Floors.TryGetValue(newTransport.Elevation + i, out var fs))
        //    {
        //        fs.SetPositionVisible(newTransport.HorizontalPosition, newTransport.Definition.Width, false);
        //    }
        //}
    }
}
