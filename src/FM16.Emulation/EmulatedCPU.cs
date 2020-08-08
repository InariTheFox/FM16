using FM16.Common;
using System;
using System.Collections.Generic;

namespace FM16.Emulation
{
    public class EmulatedCPU : ICPU
    {
        private readonly IDictionary<byte, IOperation> _operations;

        public EmulatedCPU(IEnumerable<IOperation> operations)
        {
        }

        #region Interface Implementation

        /// <summary>
        /// The bus used to transmit and receive addresses and data
        /// in memory or I/O.
        /// </summary>
        public EmulatedBus AddressDataBus { get; }

        /// <summary>
        /// Signals when the processor is transmitting an address over
        /// the <see cref="AddressDataBus"/>, causing it to be latched
        /// by the recieving device.
        /// </summary>
        public bool AddressLatchEnable { get; set; }

        /// <summary>
        /// Signals when the processor is requesting a byte to be aligned to
        /// the high side of the data bus.
        /// </summary>
        public bool BusHighEnable { get; set; }

        /// <summary>
        /// Signals when the processor is accessing data from memory or
        /// I/O operations.
        /// </summary>
        public bool DataEnable { get; set; }

        /// <summary>
        /// Signals the direction of data to and from the processor to
        /// devices.
        /// </summary>
        public DataTransmitOrReceive DataTransmitReceive { get; set; }

        /// <summary>
        /// Signals acknowledgement of an interrupt cycle.
        /// </summary>
        public bool InterruptAcknowledge { get; set; }

        /// <summary>
        /// Signals whether memory or I/O is being accessed. Effectively latching
        /// the bus multiplexer when the processor is reading from memory.
        /// <c>true</c> if the processor is accessing memory, otherwise 
        /// <c>false</c> if the processor is accessing I/O.
        /// </summary>
        public MemoryOrIO MemoryIO { get; set; }

        /// <summary>
        /// Signals when the processor is currently executing a read operation
        /// from either memory or I/O.
        /// </summary>
        public bool Read { get; set; }

        /// <summary>
        /// Signals the processor from the addressed memory or I/O that data
        /// transfer has completed.
        /// </summary>
        public bool Ready { get; set; }

        /// <summary>
        /// Signals when the processor is currently executing a write operation
        /// to either memory or I/O.
        /// </summary>
        public bool Write { get; set; }

        #endregion

        #region General Purpose Registers

        /// <summary>
        /// Accumulator register.
        /// </summary>
        public ushort AX { get; internal set; }

        /// <summary>
        /// Base register.
        /// </summary>
        public ushort BX { get; internal set; }

        /// <summary>
        /// Count register.
        /// </summary>
        public ushort CX { get; internal set; }

        /// <summary>
        /// Data register.
        /// </summary>
        public ushort DX { get; internal set; }

        #region High Order Registers

        /// <summary>
        /// Hih order byte of the accumulator register.
        /// </summary>
        public byte AH => (byte)(AX >> 8);

        /// <summary>
        /// High order byte of the base register.
        /// </summary>
        public byte BH => (byte)(BX >> 8);

        /// <summary>
        /// High order byte of the count register.
        /// </summary>
        public byte CH => (byte)(CX >> 8);

        /// <summary>
        /// High order byte of the data register.
        /// </summary>
        public byte DH => (byte)(DX >> 8);

        #endregion

        #region Low Order Registers

        /// <summary>
        /// Low order byte of the accumulator register.
        /// </summary>
        public byte AL => (byte)(AX & 0xFF);

        /// <summary>
        /// Low order byte of the base register.
        /// </summary>
        public byte BL => (byte)(BX & 0xFF);

        /// <summary>
        /// Low order byte of the count register.
        /// </summary>
        public byte CL => (byte)(CX & 0xFF);

        /// <summary>
        /// Low order byte of the data register.
        /// </summary>
        public byte DL => (byte)(DX & 0xFF);

        #endregion

        #endregion

        #region Index Registers

        /// <summary>
        /// Base pointer register.
        /// </summary>
        public ushort BP { get; internal set; }

        /// <summary>
        /// Destination index register.
        /// </summary>
        public ushort DI { get; internal set; }

        /// <summary>
        /// Source index register.
        /// </summary>
        public ushort SI { get; internal set; }

        /// <summary>
        /// Stack pointer register.
        /// </summary>
        public ushort SP { get; internal set; }

        #endregion

        #region Stack Registers

        /// <summary>
        /// Code segment register.
        /// </summary>
        public ushort CS { get; internal set; }

        /// <summary>
        /// Data segment register.
        /// </summary>
        public ushort DS { get; internal set; }

        /// <summary>
        /// Extra segment register.
        /// </summary>
        public ushort ES { get; internal set; }

        /// <summary>
        /// Stack segment register.
        /// </summary>
        public ushort SS { get; internal set; }

        #endregion

        #region Internal Registers

        /// <summary>
        /// Instruction pointer register.
        /// </summary>
        public ushort IP { get; set; }

        /// <summary>
        /// Instruction register. Stores the currently executing instruction
        /// <see cref="OpCode" /> and <see cref="ModRM"/> values.
        /// </summary>
        public ushort IR { get; internal set; }

        /// <summary>
        /// Addressing mode, register and register/memory source and 
        /// destination flags.
        /// </summary>
        public byte ModRM => (byte)(IR & 0xFF);

        /// <summary>
        /// Operation code identifying the current instruction.
        /// </summary>
        public byte OpCode => (byte)(IR >> 8);

        /// <summary>
        /// The current execution step of the processor.
        /// </summary>
        public byte Step { get; set; }

        #endregion

        /// <summary>
        /// The currently executing operation. This becomes available after
        /// Step 1.
        /// </summary>
        public IOperation CurrentOperation { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Determines whether the Program Counter (IP) advances during the
        /// next clock tick.
        /// </summary>
        public bool ProgramCounterEnabled { get; set; }

        IBus ICPU.AddressDataBus => AddressDataBus;

        /// <summary>
        /// Signals the processor that a new clock tick has occurred.
        /// </summary>
        public void Clock()
        {
            if (Step == 0x00)
            {
                AddressLatchEnable = true;
                DataTransmitReceive = DataTransmitOrReceive.Receive;

                var address = GetAddress(SegmentRegisters.CS, IP);
                AddressDataBus.WriteDouble(address);
            }
            else if (Step == 0x01)
            {
                AddressLatchEnable = false;
                Read = DataEnable = true;

                IR = AddressDataBus.ReadWord();
                ProgramCounterEnabled = true;
            }
            else
            {
                if (CurrentOperation == null)
                {
                    CurrentOperation = FindOperation(OpCode);

                    if (CurrentOperation == null)
                    {
                        Halt(true);
                        throw new InvalidOperationException();
                    }
                }

                // If the operation is complete, it returns false, then we
                // can dispose of it.
                if (CurrentOperation.Execute(this))
                {
                    CurrentOperation = null;
                }
            }

            // Advance the counters. The program counter only advances if
            // the latch is set. The latch is reset at the end of each clock
            // tick.
            if (ProgramCounterEnabled)
            {
                IP++;
                ProgramCounterEnabled = false;
            }

            Step++;
        }

        /// <summary>
        /// Returns an address calculated using a base and offset.
        /// </summary>
        /// <param name="segment">The base segment.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>The calculated memory address.</returns>
        public uint GetAddress(SegmentRegisters segment, ushort offset)
            => (uint)((GetRegister(segment) << 4) + IP);

        /// <summary>
        /// Returns the value of the register provided.
        /// </summary>
        /// <param name="register">
        /// The byte-sized register to retrieve the value from.
        /// </param>
        /// <returns>A byte value.</returns>
        public byte GetRegister(ByteRegisters register)
        {
            switch (register)
            {
                case ByteRegisters.AL:
                    return AL;
                case ByteRegisters.CL:
                    return CL;
                case ByteRegisters.DL:
                    return DL;
                case ByteRegisters.BL:
                    return BL;
                case ByteRegisters.AH:
                    return AH;
                case ByteRegisters.CH:
                    return CH;
                case ByteRegisters.DH:
                    return DH;
                case ByteRegisters.BH:
                    return BH;
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns the value of the register provided.
        /// </summary>
        /// <param name="register">
        /// The word-sized register to retrieve the value from.
        /// </param>
        /// <returns>A word value.</returns>
        public ushort GetRegister(WordRegisters register)
        {
            switch (register)
            {
                case WordRegisters.AX:
                    return AX;
                case WordRegisters.CX:
                    return CX;
                case WordRegisters.DX:
                    return DX;
                case WordRegisters.BX:
                    return BX;
                case WordRegisters.SP:
                    return SP;
                case WordRegisters.BP:
                    return BP;
                case WordRegisters.SI:
                    return SI;
                case WordRegisters.DI:
                    return DI;
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns the value of the segment register provided.
        /// </summary>
        /// <param name="register">
        /// The segment register to retrieve the value from.
        /// </param>
        /// <returns>A word value.</returns>
        public ushort GetRegister(SegmentRegisters register)
        {
            switch (register)
            {
                case SegmentRegisters.ES:
                    return ES;
                case SegmentRegisters.CS:
                    return CS;
                case SegmentRegisters.SS:
                    return SS;
                case SegmentRegisters.DS:
                    return DS;
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Halts the processor and stops any further processing of
        /// operations.
        /// </summary>
        public void Halt(bool isFaulted)
        {

        }

        /// <summary>
        /// Signals to the processor that an interrupt is requested.
        /// </summary>
        public void Interrupt()
        {

        }

        /// <summary>
        /// Sets the <see cref="Step"/> register to a specific value
        /// in order to allow for branched executions in micro-code.
        /// </summary>
        /// <param name="step"></param>
        public void JumpToStep(byte step)
            => Step = step;

        /// <summary>
        /// Signals to the processor that a non-maskable interrupt 
        /// is requested.
        /// </summary>
        public void NonMaskableInterrupt()
        {

        }

        /// <summary>
        /// Resets the processor to it's default state.
        /// </summary>
        public void Reset()
        {
            // Reset registers to their default value.
            AX = CX = DX = BX = 0x0000;
            SP = BP = SI = DI = 0x0000;
            ES = DS = SS = 0x0000;
            CS = 0xFFFF;
            IP = 0x0000;
            IR = 0x0000;
            Step = 0x00;

            // Reset state signals.
            AddressLatchEnable = BusHighEnable = DataEnable = false;
            Read = Write = InterruptAcknowledge = false;

            // Reset data flow signals.
            DataTransmitReceive = DataTransmitOrReceive.Transmit;
            MemoryIO = MemoryOrIO.Memory;
        }

        public void SetRegister(ByteRegisters register, byte value)
        {
            switch (register)
            {
                case ByteRegisters.AL:
                    AX = (ushort)((AH << 8) + value);
                    break;
                case ByteRegisters.CL:
                    CX = (ushort)((CH << 8) + value);
                    break;
                case ByteRegisters.DL:
                    DX = (ushort)((DH << 8) + value);
                    break;
                case ByteRegisters.BL:
                    BX = (ushort)((BH << 8) + value);
                    break;
                case ByteRegisters.AH:
                    AX = (ushort)((value << 8) + AL);
                    break;
                case ByteRegisters.CH:
                    CX = (ushort)((value << 8) + CL);
                    break;
                case ByteRegisters.DH:
                    DX = (ushort)((value << 8) + DL);
                    break;
                case ByteRegisters.BH:
                    BX = (ushort)((value << 8) + BL);
                    break;
            }
        }

        public void SetRegister(ByteRegisters register, int value)
            => SetRegister(register, (byte)value);

        public void SetRegister(WordRegisters register, ushort value)
        {
            switch (register)
            {
                case WordRegisters.AX:
                    AX = value;
                    return;
                case WordRegisters.CX:
                    CX = value;
                    return;
                case WordRegisters.DX:
                    DX = value;
                    return;
                case WordRegisters.BX:
                    BX = value;
                    return;
                case WordRegisters.SP:
                    SP = value;
                    return;
                case WordRegisters.BP:
                    BP = value;
                    return;
                case WordRegisters.SI:
                    SI = value;
                    return;
                case WordRegisters.DI:
                    DI = value;
                    return;
            }
        }

        public void SetRegister(SegmentRegisters register, ushort value)
        {
            switch (register)
            {
                case SegmentRegisters.ES:
                    ES = value;
                    return;
                case SegmentRegisters.CS:
                    throw new ArgumentException("CS Register cannot be set. Use jump instead.", nameof(register));
                case SegmentRegisters.SS:
                    SS = value;
                    break;
                case SegmentRegisters.DS:
                    DS = value;
                    break;
            }

            throw new InvalidOperationException();
        }

        public void SetRegister(WordRegisters register, int value)
            => SetRegister(register, (ushort)value);

        private IOperation FindOperation(byte opCode)
        {
            return null;
        }
    }
}
