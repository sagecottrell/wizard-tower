from collections import defaultdict
import pathlib
import re


dir = pathlib.Path(__file__).parent

filter_re = re.compile(r"^([A-Z][a-z]+)+([A-Z][a-z]+ed)([A-Z][a-z]+)*Event.cs$")

events: dict[tuple[str, ...], list[str]] = defaultdict(list)

for event_file in dir.rglob("*Event.cs"):
    if not filter_re.match(event_file.name):
        continue
    text = event_file.read_text()

    cls_name = event_file.name.removesuffix('.cs')
    new_name = cls_name.replace("ed", "ing", count=1)
    new_file = event_file.with_name(new_name + '.cs')
    events[event_file.parent.relative_to(dir).parts].append(cls_name)
    events[event_file.parent.relative_to(dir).parts].append(new_name)

    if new_file.exists():
        continue

    text = text.replace(f'class {cls_name}', f'class {new_name}')
    text = text.replace(': BaseEvent, ', ': BaseEvent, IDeniableEvent, ')
    text = text.replace('{', '{\n    public bool IsAllowed { get; set; } = true;', count=1)
    text = text.replace("{ get; }", "{ get; set; }")
    new_file.write_text(f"""
/**
Generated from ./events/{event_file.relative_to(dir)}
**/

{text}""".strip())
    

# import json
# print(json.dumps({".".join(key): value for key, value in events.items()}, indent=4))


handlers_folder = dir / 'handlers'
for parts, handlers in events.items():
    file_name = f'{'.'.join(parts)}.Events.cs'
    file = handlers_folder / file_name

    namespaces = [
        f"using wizardtower.events.{".".join(parts[:i + 1])};"
        for i in range(len(parts))
    ]

    template = f"""
using wizardtower.events.features;
{"\n".join(namespaces)}

namespace wizardtower.events.handlers;
    """

    indent = ""
    for i, part in enumerate(parts):
        template += f'\n{i * '    '}public static partial class {part.capitalize() + ('Events' if i == 0 else '')} {{'
        indent = (i + 1) * "    "
    
    for event in handlers:
        stripped = event.removesuffix('Event')
        for part in reversed(parts):
            stripped = stripped.removeprefix(part)
        event_prop = f"public static Event<{event}> {stripped} {{ get; set; }} = new();"
        method = f"public static {event} On{stripped}({event} e) => {stripped}.InvokeSafely(e);"

        template += f"\n{indent}{event_prop}\n{indent}{method}"

    template += "".join('\n' + ("    " * i) + '}' for i in reversed(range(len(parts))))
    file.write_text(template)
