@tool
class_name IWorker
extends Node3D

@export var id: WorkerId;
@export var kind: WorkerDef;
@export var room_start: RoomId;
@export var room_end: RoomId;
@export var floor_id: FloorId;
@export var h_pos: int;

# used if the destination is on a different floor
@export var next_step_pos: int;
@export var next_step_floor: FloorId;

@export var capacity: int;
@export var payload_kind: ItemDef;
@export var speed: float;

enum Status {
	Idle,
	Working,
	Confused,
}
@export var status: Status;

# failure to pathfind to the destination accrues confusion.
# too much confusion will result in the worker returning resources to the source, and despawning
@export var confusion: int;
