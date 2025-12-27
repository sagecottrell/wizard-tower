@tool
extends Node3D

@onready var background: MeshInstance3D = $background;

@export var easing: float = 0.5;
@export var grow_time: float = 0.75;
var grow_timer_left: float = 0.0;
var grow_timer_right: float = 0.0;

var prev_left: float = 1;
var current_left: float = 1;
var size_left: float = 1;
@export var width_left: int:
	get:
		return int(size_left);
	set(value):
		prev_left = size_left;
		size_left = value;
		grow_timer_left = 0;

var prev_right: float = 1;
var current_right: float = 1;
var size_right: float = 1;
@export var width_right: int:
	get:
		return int(size_right);
	set(value):
		prev_right = size_right;
		size_right = value;
		grow_timer_right = 0;
	
var width: int:
	get:
		return int(size_right + size_left);

func _ready():
	# start out at the right size, we don't spawn in at the default size
	var w = size_right + size_left;
	current_left = size_left;
	current_right = size_right;
	background.scale.x = w;
	var mat: StandardMaterial3D = background.get_active_material(0);
	mat = mat.duplicate_deep(Resource.DEEP_DUPLICATE_ALL);
	background.set_surface_override_material(0, mat);
	mat.uv1_scale.x = w;
	background.position.x = (size_right - size_left) / 2;
		
func _process(delta: float) -> void:
	if current_right != size_right or current_left != size_left:
		do_scale(delta);

func do_scale(delta: float) -> void:
	var mat: StandardMaterial3D = background.get_active_material(0);
	if current_right != size_right and grow_timer_right < grow_time:
		var f = ease(grow_timer_right / grow_time, easing);
		current_right = (size_right - prev_right) * f + prev_right;
		if grow_timer_right + delta >= grow_time:
			current_right = size_right;
		grow_timer_right += delta;
	if current_left != size_left:
		var f = ease(grow_timer_left / grow_time, easing);
		current_left = (size_left - prev_left) * f + prev_left;
		if grow_timer_left + delta >= grow_time:
			current_left = size_left;
		grow_timer_left += delta;
		mat.uv1_offset.x = -fmod(current_left, 1);
	
	var w = current_left + current_right;
	background.scale.x = w;
	mat.uv1_scale.x = w;
	background.position.x = (current_right - current_left) / 2;
