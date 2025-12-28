@tool
@icon("res://custom_godot_resources/tier-icon.svg")
class_name TierDef
extends Resource

@export var display_name: String;
@export var readme: String;
@export var icon: Texture2D;
@export var rooms: Array[RoomDef] = [];
@export var research: Array[ResearchDef] = [];
@export var floors: Array[FloorDef] = [];
@export var transports: Array[TransportDef] = [];
