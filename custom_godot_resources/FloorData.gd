@tool
@icon("res://custom_godot_resources/floor-icon.svg")
class_name FloorData
extends Resource

@export var id: FloorId;
@export var kind: FloorDef;

@export var size_left: int = 1;
@export var size_right: int = 1;
