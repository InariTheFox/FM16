using FM16.Common;
using FM16.Emulation.Operations.States;

namespace FM16.Emulation.Operations.Move
{
    [OperationMnemonic("MOV")]
    [OperationOpCode(0xB0)]
    public class MoveImmToReg : Operation
    {
        public MoveImmToReg()
        { }

        public override bool Execute(EmulatedCPU cpu)
        {
            if (State == null)
            {
                var address = cpu.GetAddress(SegmentRegisters.CS, cpu.IP);
                State = new SetAddressState(this, address);

                cpu.ProgramCounterEnabled = true;
            }

            State.Execute(cpu);

            return false;
        }
    }
}
