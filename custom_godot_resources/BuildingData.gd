@tool
@icon("res://custom_godot_resources/building-icon.svg")
class_name BuildingData
extends Resource

@export var building_name: String;
@export var alive: bool;
@export var floors: Dictionary[FloorId, FloorData] = {};
@export var rooms: Dictionary[RoomId, RoomData] = {};
@export var transports: Dictionary[TransportId, TransportData] = {};
@export var workers: Dictionary[WorkerId, WorkerData] = {};

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
@export var time_per_day_seconds: float = 5 * 60;
@export var day_started: bool;
