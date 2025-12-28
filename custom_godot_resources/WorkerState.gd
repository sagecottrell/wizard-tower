@tool
@icon("res://custom_godot_resources/worker-state-icon.svg")
class_name WorkerState
extends Resource

@export var id: int;
@export var kind: WorkerDef;
@export var room_start: int;
@export var room_end: int;
@export var floor_id: int;
@export var h_pos: int;

# used if the destination is on a different floor
@export var next_step_pos: int;
@export var next_step_floor: int;

@export var capacity: int;
@export var payload_kind: ItemDef;
@export var speed: float;

enum Status {
	Idle,
	Working,
	Confused,
}
@export var status: Status;

# failure to pathfind to the destination accrues confusion.
# too much confusion will result in the worker returning resources to the source, and despawning
@export var confusion: int;
