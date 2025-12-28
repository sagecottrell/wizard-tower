@tool
@icon("res://custom_godot_resources/transport-state-icon.svg")
class_name TransportState
extends Resource

@export var kind: TransportDef;
@export var custom_name: String;
@export var id: int;

@export var h_pos: int;
@export var bottom_floor: int;

@export var height: int;
@export var occupancy: Array[WorkerState];
