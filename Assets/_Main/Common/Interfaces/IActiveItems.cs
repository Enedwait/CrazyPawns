using System.Collections.Generic;

namespace Main.Common.Interfaces
{
    public interface IActiveItems<T>
    {
        IReadOnlyList<T> ActiveItems { get; }
    }
}
