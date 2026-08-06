using System.Collections.Generic;

namespace Main.Common.Classes.Pools
{
    public interface IActiveItems<T>
    {
        IReadOnlyList<T> ActiveItems { get; }
    }
}
