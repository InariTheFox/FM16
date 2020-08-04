using System;

namespace FM16.Emulation.Operations.States
{
    public class GetImmediateValueState : State
    {
        public GetImmediateValueState(Operation owner)
            : base(owner) { }

        public override void Execute(EmulatedCPU cpu) 
            => throw new NotImplementedException();
    }
}
