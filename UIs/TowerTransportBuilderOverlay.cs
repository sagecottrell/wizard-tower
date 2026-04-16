using Godot;
using System.Collections.Generic;
using wizardtower.actions.ui;
using wizardtower.containers;
using wizardtower.events;
using wizardtower.events.ui;
using wizardtower.resource_types;
using wizardtower.state;

namespace wizardtower.UIs.build_menu;


public partial class TowerTransportBuilderOverlay(TowerScript tower) : Node3D()
{
    [Signal]
    public delegate void OnTransportBuildEventHandler();
}

