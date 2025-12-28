@tool
@icon("res://custom_godot_resources/floor-state-icon.svg")
class_name FloorState
extends Resource

@export var id: int;
@export var kind: FloorDef;

@export var size_left: int = 1;
@export var size_right: int = 1;
