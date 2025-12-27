@tool
@icon("res://custom_godot_resources/transport-icon.svg")
class_name TransportDef
extends Resource

@export var display_name: String;
@export var readme: String;
@export var cost_to_build: ItemContainer;
@export var display: PackedScene;
@export var min_height: int;
@export var max_height: int;

@export var can_stop_at_floor: Array[FloorDef] = [];
