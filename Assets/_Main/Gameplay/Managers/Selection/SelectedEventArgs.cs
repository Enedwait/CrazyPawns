using Main.Common.Behaviours;

namespace Main.Gameplay.Managers.Selection
{
    public record SelectedEventArgs(ISelectionManager Manager, ISelectable Selectable);

    public record DeselectedEventArgs(ISelectionManager Manager, ISelectable Selectable);
}
