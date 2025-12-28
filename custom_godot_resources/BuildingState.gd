@tool
@icon("res://custom_godot_resources/building-state-icon.svg")
class_name BuildingState
extends Resource

@export var building_name: String;
@export var alive: bool;
@export var floors: Dictionary[int, FloorState];
@export var rooms: Dictionary[int, RoomState];
@export var transports: Dictionary[int, TransportState];
@export var workers: Dictionary[int, WorkerState];

# what research has been completed, and how many times (will usually be 1)
@export var research_completed: Dictionary[ResearchDef, int];

@export var room_id_counter: int;
@export var worker_id_counter: int;
@export var transport_id_counter: int;

@export var top_floor: int;
@export var max_width: int;
@export var max_height: int;
@export var max_depth: int;

@export var wallet: ItemContainer;

@export var rng_initial_seed: int;
@export var rng_state: int;

@export var rating: int;
@export var new_things_acked: Dictionary;

@export var time_seconds: float;
@export var time_per_day_seconds: float;
@export var day_started: bool;
