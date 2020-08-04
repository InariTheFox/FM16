using System;

namespace FM16.Common
{
    public interface ICPU
    {
        IBus AddressDataBus { get; }

        bool AddressLatchEnable { get; }

        bool BusHighEnable { get; }

        bool DataEnable { get; }

        DataTransmitOrReceive DataTransmitReceive { get; }

        bool InterruptAcknowledge { get; }

        MemoryOrIO MemoryIO { get; }

        bool Read { get; }

        bool Ready { get; }

        bool Write { get; }

        void Clock();

        byte GetRegister(ByteRegisters register);

        ushort GetRegister(WordRegisters register);

        void Interrupt();

        void NonMaskableInterrupt();

        void Reset();

        void SetRegister(ByteRegisters register, byte value);

        void SetRegister(ByteRegisters register, int value);

        void SetRegister(WordRegisters register, ushort value);

        void SetRegister(WordRegisters register, int value);
    }
}
