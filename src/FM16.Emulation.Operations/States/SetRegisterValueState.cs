using System;

namespace FM16.Emulation.Operations.States
{
    public class SetRegisterValueState : State
    {
        public SetRegisterValueState(Operation owner)
            : base(owner) { }

        public override void Execute(EmulatedCPU cpu) 
            => throw new NotImplementedException();
    }
}
