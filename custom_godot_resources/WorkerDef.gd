@tool
@icon("res://custom_godot_resources/worker-icon.svg")
class_name WorkerDef
extends Resource

@export var display_name: String;
@export var sprite_moving: PackedScene;
@export var sprite_stationary: PackedScene;
@export var portrait: Texture2D;

@export var base_planning_capability: int = 4;
@export var base_movement_speed: float = 4;
@export var base_capacity: int = 2;
