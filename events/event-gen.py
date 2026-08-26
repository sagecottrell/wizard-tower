import pathlib
import re


dir = pathlib.Path(__file__).parent

filter_re = re.compile(r"^([A-Z][a-z]+)+([A-Z][a-z]+ed)Event.cs$")

for event_file in dir.rglob("*Event.cs"):
    if not filter_re.match(event_file.name):
        continue
    text = event_file.read_text()

    cls_name = event_file.name.removesuffix('.cs')
    new_name = cls_name.replace("ed", "ing", count=1)

    text = text.replace(f'class {cls_name}', f'class {new_name}')
    text = text.replace(': BaseEvent, ', ': BaseEvent, IDeniableEvent, ')
    text = text.replace('{', '{\n    public bool IsAllowed { get; set; } = true;', count=1)

    new_file = event_file.with_name(new_name + '.cs')
    if new_file.exists():
        continue
    
    new_file.write_text(text)

