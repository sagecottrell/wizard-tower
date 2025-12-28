extends Node3D

@onready var hotel: BaseRoomScript = $Hotel1;

func _ready():
	var aabb = hotel.background();
	var size = aabb.size;
	var s = Vector3(constants.ROOM_WIDTH / size.x, constants.ROOM_HEIGHT / size.y, 1);
	hotel.global_scale(s);
