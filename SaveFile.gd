@tool
class_name SaveFile
extends Node3D

@onready var building_scene = preload("res://building.tscn");

@export var towers: Array[TowerState] = []

@export_custom(PROPERTY_HINT_EXPRESSION, "hi,there") var expr: String = ""

var current_building: Node:
	set(value):
		if value != current_building:
			current_building = value;
			on_change_building();

@export var wallet: Node;

@export var rooms_seen: Array[RoomDefinition];
@export var floors_seen: Array[Node];
@export var transports_seen: Array[TransportDefinition];

func _ready():
	on_change_building();


func on_change_building():
	$Building.queue_free();
	remove_child($Building);
	var b: BaseBuildingScript = building_scene.instantiate(PackedScene.GEN_EDIT_STATE_INSTANCE);
	b.name = "Building";
	b.data = current_building;
	add_child(b);
	
