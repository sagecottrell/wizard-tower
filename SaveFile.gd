@tool
class_name SaveFile
extends Node3D

@onready var building_scene = preload("res://building.tscn");

@export var buildings: Array[BuildingState];

var current_building: BuildingState:
	set(value):
		if value != current_building:
			current_building = value;
			on_change_building();

@export var wallet: ItemContainer;

@export var rooms_seen: Array[RoomDef];
@export var floors_seen: Array[FloorDef];
@export var transports_seen: Array[TransportDef];

func _ready():
	if current_building == null:
		current_building = buildings[0];
	on_change_building();


func on_change_building():
	$Building.queue_free();
	remove_child($Building);
	var b: BaseBuildingScript = building_scene.instantiate(PackedScene.GEN_EDIT_STATE_INSTANCE);
	b.name = "Building";
	b.data = current_building;
	add_child(b);
	
