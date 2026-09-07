from collections import defaultdict
import pathlib
import re


def _indent(i: int, string: str, spacer = "    ") -> str:
    indent = spacer * i
    return "\n".join(indent + p for p in string.splitlines())

dir = pathlib.Path(__file__).parent

filter_re = re.compile(r"^([A-Z][a-z]+)+([A-Z][a-z]+ed)([A-Z][a-z]+)*Event.cs$")
param_map_re = re.compile(r"    public (?P<type>\w+(<(\w+,? ?)+>)?) (?P<prop>\w+) { get; set; } = (?P<param>\w+);")
param_re = re.compile(r'((\(|, )(?P<type>\w+(<(\w+,? ?)+>)?) (?P<name>\w+))')

events: dict[tuple[str, ...], list[str]] = defaultdict(list)

pairs: dict[tuple[str, ...], dict[str, str]] = defaultdict(dict) # pre-event to post-event

for event_file in dir.rglob("*Event.cs"):
    text = event_file.read_text()

    if "Generated from ./events/" in text[:100] or ': BaseEvent' not in text:
        continue

    cls_name = event_file.name.removesuffix('.cs')
    path_parts = event_file.parent.relative_to(dir).parts
    events[path_parts].append(cls_name)

    if not filter_re.match(event_file.name):
        continue

    new_name = cls_name.replace("ed", "ing", count=1)
    new_file = event_file.with_name(new_name + '.cs')
    events[path_parts].append(new_name)

    pairs[path_parts][new_name] = cls_name

    if new_file.exists() and "Generated from ./events/" not in new_file.read_text()[:100]:
        continue

    # print(f'updating {event_file}')
    text = text.replace(f'class {cls_name}', f'class {new_name}')
    text = text.replace(': BaseEvent, ', ': BaseEvent, IDeniableEvent, ')
    text = text.replace('{', '{\n    public bool IsAllowed { get; set; } = true;', count=1)
    text = text.replace("{ get; }", "{ get; set; }")

    prop_map: dict[str, str] = {}
    param_types: list[dict[str, str]] = []
    for match in param_re.findall(text[text.index("class"):text.index(':')]):
        prop_map[match[-1]] = ""
        param_types.append(param_re.match(match[0]).groupdict())

    for prop in param_map_re.finditer(text):
        d = prop.groupdict()
        if d['param'] in prop_map:
            prop_map[d['param']] = d['prop']

    text += f"""

public static class {cls_name}Extensions {{
    public static {cls_name} Into(this {new_name} old) {{
        return new({", ".join(
f"{k}: old.{v}"
for k, v in prop_map.items()
        )}) {{ Source = old, }};
    }}

    public static {new_name} {new_name}(this IEvent ev, {", ".join(
        f"{groupdict['type']} {groupdict['name']}"
        for groupdict in param_types
    )})
    {{
        return new({", ".join(k['name'] for k in param_types)}) {{ Source = ev }};
    }}
}}
"""

    if "wizardtower.events.interfaces" not in text:
        text = f"""
using wizardtower.events.interfaces;

{text}
"""

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
using System.Diagnostics.CodeAnalysis;
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

        template += _indent(i=1, spacer=indent, string=f"""
{event_prop}
public static {event} On{stripped}({event} e) => {stripped}.InvokeSafely(e);
public static {event} On{stripped}({event} e, BaseEvent source) {{ 
    e.Source = source; 
    return {stripped}.InvokeSafely(e); 
}}
""")

    for pre, post in pairs[parts].items():
        stripped = pre.removesuffix('Event')
        for part in reversed(parts):
            stripped = stripped.removeprefix(part)

        template += _indent(i=1, spacer=indent, string=f"""
public static bool Try{stripped}({pre} pre, [NotNullWhen(true)] out {post}? e) {{
    e = null;
    if (On{stripped}(pre).IsAllowed) {{
        e = pre.Into();
        return true;
    }}
    return false;
}}
""")

    template += "".join('\n' + ("    " * i) + '}' for i in reversed(range(len(parts))))
    file.write_text(template)
