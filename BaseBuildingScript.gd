@tool
class_name BaseBuildingScript
extends Node3D

@export var data: BuildingState:
	set(value):
		data = value;
		on_set_data();

const floor_name = "floor %s";

func on_set_data() -> void:
	for floor_id in data.floors:
		print("floor", floor_id);
		var f = data.floors[floor_id];
		var node = f.kind.scene.instantiate();
		node.data = f;
		node.name = floor_name % floor_id;
		
		var existing = find_child(floor_name % floor_id);
		if existing != null:
			existing.queue_free();
			remove_child(existing);
		add_child(node);
