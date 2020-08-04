namespace FM16.Emulation.Operations.States
{
    public abstract class State
    {
        protected State(Operation operation)
        {
            Operation = operation;
        }

        public Operation Operation { get; }

        public abstract void Execute(EmulatedCPU cpu);
    }
}
