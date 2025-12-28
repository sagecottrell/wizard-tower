@tool
@icon("res://custom_godot_resources/floor-icon.svg")
class_name FloorDef
extends Resource

@export var name: String;
@export var readme: String;
@export var scene: PackedScene;
@export var cost_to_build: ItemContainer;
