using FM16.Common;
using FM16.Emulation.Operations.States;

namespace FM16.Emulation.Operations
{
    public abstract class Operation : IOperation
    {
        protected State State { get; set; }

        public abstract bool Execute(EmulatedCPU cpu);

        bool IOperation.Execute(ICPU cpu)
            => Execute((EmulatedCPU)cpu);
    }
}