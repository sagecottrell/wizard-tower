using wizardtower.events.features;
using wizardtower.events.ui;

namespace wizardtower.events.handlers;

public static class GeneralEvents
{
    public static Event<ShowingUIEvent> ShowingUIEvent { get; set; } = new();
    public static Event<ShowedUIEvent> ShowedUI { get; set; } = new();

    public static Event<HidingUIEvent> HidingUI { get; set; } = new();
    public static Event<HiddenUIEvent> HiddenUI { get; set; } = new();

    public static ShowingUIEvent OnShowingUI(ShowingUIEvent e) => ShowingUIEvent.InvokeSafely(e);
    public static ShowedUIEvent OnShowedUI(ShowedUIEvent e) => ShowedUI.InvokeSafely(e);

    public static HidingUIEvent OnHidingUI(HidingUIEvent e) => HidingUI.InvokeSafely(e);
    public static HiddenUIEvent OnHiddenUI(HiddenUIEvent e) => HiddenUI.InvokeSafely(e);
}
