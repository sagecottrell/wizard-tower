extends Control


var building: BuildingState:
	get:
		return %buildingctx.building;


func _ready():
	building.building_name
	%buildingname.set_text;
