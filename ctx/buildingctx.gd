class_name BuildingContext
extends Node

var building: Node;

func _ready():
	# search up tree for a building context provider
	var current = get_parent();
	while current != null and current is not BaseBuildingScript:
		current = current.get_parent();
	if current is BaseBuildingScript:
		building = current.data

func _exit_tree() -> void:
	building = null;
