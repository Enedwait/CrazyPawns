using Main.Common.Behaviours;
using UnityEngine.Events;

namespace Main.Gameplay.Managers.Selection
{
    public interface ISelectionManager : IManager
    {
        event UnityAction<SelectedEventArgs> onSelected;
        event UnityAction<DeselectedEventArgs> onDeselected;

        bool Select(ISelectable selectable);
        bool Deselect(ISelectable selectable);
    }
}
