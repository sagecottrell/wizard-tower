@tool
@icon("res://custom_godot_resources/research-icon.svg")
class_name ResearchDef
extends Resource

@export var display_name: String;
@export var readme: String;
@export var icon: Texture2D;
@export var unlock_tier: TierDef;
@export var cost: ItemContainer;
