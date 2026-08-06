using Main.Common.Behaviours;
using Main.Common.Interfaces;

namespace Main.Infrastructure.Controls.Providers
{
    public abstract class AbstractInputProvider : AbstractMonoBehaviourExtended, IInputProvider
    {
        public abstract void ResetValues();
    }
}
