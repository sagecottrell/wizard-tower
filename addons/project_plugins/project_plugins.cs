#if TOOLS
using Godot;
using System;
using wizardtower.addons.project_plugins;

[Tool]
public partial class project_plugins : EditorPlugin
{

	private NumericDictInspector? _numericDictInspector;

	public override void _EnterTree()
	{
		_numericDictInspector = new NumericDictInspector();
		AddInspectorPlugin(_numericDictInspector);
    }

	public override void _ExitTree()
	{
        if (_numericDictInspector is not null)
        {
			RemoveInspectorPlugin(_numericDictInspector);
        }
    }
}
#endif
