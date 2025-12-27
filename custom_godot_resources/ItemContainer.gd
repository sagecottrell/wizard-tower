@tool
@icon("res://custom_godot_resources/item-container-icon.svg")
class_name ItemContainer
extends Resource

@export var items: Dictionary[ItemDef, int] = {};


func without_zeroes() -> ItemContainer:
	remove_zeroes();
	return self;

func remove_zeroes():
	for item in items:
		var amount = items[item];
		if amount == 0:
			items.erase(item);

func add(other: ItemContainer) -> ItemContainer:
	var new = ItemContainer.new();
	for item in items.merged(other.items).keys():
		new.items[item] = items.get(item, 0) + other.items.get(item, 0)
	return new;

func sub(other: ItemContainer) -> ItemContainer:
	var new = ItemContainer.new();
	for item in items.merged(other.items).keys():
		new.items[item] = items.get(item, 0) - other.items.get(item, 0)
	return new;

func mul(amount: int) -> ItemContainer:
	var new = ItemContainer.new();
	for item in items:
		new.items[item] = items[item] * amount;
	return new;

func gte(other: ItemContainer) -> bool:
	for item in other:
		if items.get(item, 0) < other.items.get(item, 0):
			return false;
	return true;
