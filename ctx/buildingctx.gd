class_name BuildingContext
extends Node

var _building: Node;
var building: Node:
	get:
		if _building == null:
			# search up tree for a building context provider
			var current = get_parent()
			while current != null and current is not BaseBuildingScript:
				current = current.get_parent()
			if current is BaseBuildingScript:
				_building = current.data
		return _building

func _exit_tree() -> void:
	building = null;
