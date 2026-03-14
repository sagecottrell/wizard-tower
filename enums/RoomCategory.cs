using System;

namespace wizardtower.enums;

[Flags]
public enum RoomCategory : uint
{
    None = 0,
    LivingQuarters = 1 << 0,
    Workshop = 1 << 1,
    Storage = 1 << 2,
    ResearchLab = 1 << 3,
    DiningHall = 1 << 4,
    Recreation = 1 << 5,
}
