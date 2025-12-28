@tool
extends Control
const base = "res://custom_godot_resources/%s"
var regex = RegEx.new()
func _ready():
	regex.compile("^\\wState.gd$")

func _on_button_pressed() -> void:
	var dir = DirAccess.open(base % "")
	if dir:
		dir.list_dir_begin()
		var file_name = dir.get_next()
		while file_name != "":
			if dir.current_is_dir():
				print("Found directory: " + file_name)
			elif regex.search(file_name):
				print("Found file: " + file_name)
				var file = FileAccess.open(base % file_name, FileAccess.READ)
				var text = file.get_as_text(true)
				file.close()
				gen_file(file_name.replace(".gd", ".signaled.gd"), text)
				
			file_name = dir.get_next()
			
		# var file = FileAccess.open(file_name, FileAccess.READ);
	else:
		print('unable to load custom resources')



const tpl = """
signal {name}__on_changed(old: {type}, new: {type});
@export var {name}: {type}:
	get:
		return {name};
	set(value):
		if {name} != value:
			{name}__on_changed.emit({name}, value);
		return {name} = value;
		
"""
func gen_file(path: String, content: String):
	var file = FileAccess.open(base % path, FileAccess.WRITE)
	
	for line in content.split('\n', false):
		if not line.begins_with("@export var "):
			continue
		var parts = line.split(" ", false, 3)[2]
		var f = {"name": parts[2], "type": parts[3]}
		
		file.store_string(tpl.format(f))
	file.close()
