@tool
@icon("res://custom_godot_resources/research-icon.svg")
class_name ResearchDef
extends Resource

@export var display_name: String;
@export var readme: String;
@export var icon: Texture2D;
@export var cost: ItemContainer;
@export var prereqs: Array[ResearchDef];

# use to prevent access until a certain tier, even if all pre-reqs are complete
@export var available_at_tier: TierDef;

# rooms unlocked via this research
@export var rooms: Array[RoomDef];
# floors unlocked via this research
@export var floors: Array[FloorDef];
# transports unlocked via this research
@export var transports: Array[TransportDef];

# set to zero for infinite
@export var max_times_researchable: int = 1;
