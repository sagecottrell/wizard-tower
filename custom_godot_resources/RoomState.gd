@tool
@icon("res://custom_godot_resources/room-state-icon.svg")
class_name RoomState
extends Resource

@export var id: int;

@export var floor_position: int;
@export var kind: RoomDef;

@export var bottom_floor: int;

@export var resources_needs: ItemContainer;
@export var waiting_since: float;
@export var working: bool;

@export var storage: ItemContainer;

@export var total_produced: ItemContainer;
@export var workers: Dictionary[WorkerDef, int];
@export var workers_delivering_out: Dictionary[WorkerDef, int];

@export var output_priorities: Dictionary[int, OutputPrio.OutputPrio];
@export var output_strategy: RoomOutputStrategy;

@export var produced_workers_committed: Dictionary[int, Commit];

@export var times_produced_today: int;
@export var pending_deliveries_in: ItemContainer;

func _save():
	print("saved room state")
