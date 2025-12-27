@tool
@icon("res://custom_godot_resources/room-icon.svg")
class_name RoomDef
extends Resource

@export var display_name: String = "room";
@export var readme: String;

@export var height: int = 1;
@export var width: int = 1;
@export var sprite_active: PackedScene;
@export var sprite_empty: PackedScene;
@export var sprite_active_night: PackedScene;
@export var sprite_empty_night: PackedScene;
@export var build_thumb: PackedScene;
@export var cost_to_build: ItemContainer;
@export var category: String;

@export var upgrades: Dictionary[RoomDef, ItemContainer] = {};

@export var resource_requirements: ItemContainer;
@export var production: ItemContainer;
@export var workers_required: Dictionary[WorkerDef, int] = {};

@export var max_productions_per_day: int = 1;

@export var produce_to_wallet: bool = false;
@export var workers_produced: Dictionary[WorkerDef, int] = {};
