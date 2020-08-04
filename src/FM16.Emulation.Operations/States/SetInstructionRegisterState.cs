namespace FM16.Emulation.Operations.States
{
    public class SetInstructionRegisterState : State
    {
        private ushort _value;

        public SetInstructionRegisterState(ushort value)
            : base(null)
        {
            _value = value;
        }

        public override void Execute(EmulatedCPU cpu)
        {

        }
    }
}
