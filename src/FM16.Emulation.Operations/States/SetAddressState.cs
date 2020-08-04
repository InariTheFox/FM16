using FM16.Common;

namespace FM16.Emulation.Operations.States
{
    public class SetAddressState : State
    {
        private uint _address;

        public SetAddressState(Operation owner, uint address)
            : base(owner)
        {
            _address = address;
        }

        public override void Execute(EmulatedCPU cpu)
        {
            cpu.AddressLatchEnable = true;
            cpu.DataTransmitReceive = DataTransmitOrReceive.Receive;

            cpu.AddressDataBus.WriteDouble(_address);
        }
    }
}
