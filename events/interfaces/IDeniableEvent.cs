
namespace wizardtower.events.interfaces;

public interface IDeniableEvent
{
    bool IsAllowed { get; set; }
}
