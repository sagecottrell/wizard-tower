class_name IRoom;
extends Node3D

@onready var bg: MeshInstance3D = $background;

@export var id: RoomId;

@export var floor_position: int;
@export var kind: RoomDef;

@export var bottom_floor: FloorId;

@export var resources_needs: ItemContainer;
@export var waiting_since: float;
@export var working: bool;

@export var storage: ItemContainer;

@export var total_produced: ItemContainer;
@export var workers: Dictionary[WorkerDef, int];
@export var workers_delivering_out: Dictionary[WorkerDef, int];

enum OutputPrio {
	Prio,
	Never,
}
@export var output_priorities: Dictionary[RoomId, OutputPrio];
@export var output_strategy: RoomOutputStrategy;

class Commit extends Resource:
	# use this class to get around the limitation on generic dictionaries
	@export var workers: Dictionary[WorkerId, int];
@export var produced_workers_committed: Dictionary[RoomId, Commit];

@export var times_produced_today: int;
@export var pending_deliveries_in: ItemContainer;

func background() -> AABB:
	return get_node_aabb(bg);


func get_node_aabb(node : Node, exclude_top_level_transform: bool = true) -> AABB:
	var bounds : AABB = AABB()

	# Do not include children that is queued for deletion
	if node.is_queued_for_deletion():
		return bounds

	# Get the aabb of the visual instance
	if node is VisualInstance3D:
		bounds = node.get_aabb();

	# Recurse through all children
	for child in node.get_children():
		var child_bounds : AABB = get_node_aabb(child, false)
		if bounds.size == Vector3.ZERO:
			bounds = child_bounds
		else:
			bounds = bounds.merge(child_bounds)

	if !exclude_top_level_transform:
		bounds = node.transform * bounds

	return bounds
