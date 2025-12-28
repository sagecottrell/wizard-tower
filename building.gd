class_name Building
extends Node3D

@export var building_name: String;
@export var alive: bool;
@export var floors: Dictionary[FloorId, IFloor] = {};
@export var rooms: Dictionary[RoomId, IRoom] = {};
@export var transports: Dictionary[TransportId, ITransport] = {};
@export var workers: Dictionary[WorkerId, IWorker] = {};

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
