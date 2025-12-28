@tool
class_name ITransport
extends Node3D

@export var kind: TransportDef;
@export var custom_name: String;
@export var id: TransportId;

@export var h_pos: int;
@export var bottom_floor: FloorId;

@export var height: int;
@export var occupancy: Array[IWorker];
