using System;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace FM16.Emulation.App
{
    public class CPURegistersFacade
    {
        private readonly EmulatedCPU _cpu;

        public CPURegistersFacade() { }

        internal CPURegistersFacade(EmulatedCPU cpu)
        {
            _cpu = cpu;
        }

        [Category("General Registers")]
        [Description("Accumulator High (AH) byte register.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        [PropertyOrder(1)]
        public byte AH => _cpu.AH;

        [Category("General Registers")]
        [Description("Accumulator Low (AL) byte register.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        [PropertyOrder(2)]
        public byte AL => _cpu.AL;

        [Category("General Registers")]
        [Description("Accumulator (AX) word register.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        [PropertyOrder(0)]
        public ushort AX => _cpu.AX;

        [Category("General Registers")]
        [Description("Base High (BH) byte register.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        [PropertyOrder(10)]
        public byte BH => _cpu.BH;

        [Category("General Registers")]
        [Description("Base Low (BL) byte register.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        [PropertyOrder(11)]
        public byte BL => _cpu.BL;

        [Category("Index Registers")]
        [Description("The Base Pointer (BP) register stores the location in memory where the stack begins.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        [PropertyOrder(1)]
        public ushort BP => _cpu.BP;

        [Category("General Registers")]
        [Description("Base (BX) word register.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        [PropertyOrder(9)]
        public ushort BX => _cpu.BX;

        [Category("General Registers")]
        [Description("Count High (CH) byte register.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        [PropertyOrder(4)]
        public byte CH => _cpu.CH;

        [Category("General Registers")]
        [Description("Count Low (CL) byte register.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        [PropertyOrder(5)]
        public byte CL => _cpu.CL;

        [Category("Stack Registers")]
        [Description("The Code Segment (CS) register stores the location of code in memory.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public ushort CS => _cpu.CS;

        [Category("General Registers")]
        [Description("Count (CX) word register.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        [PropertyOrder(3)]
        public ushort CX => _cpu.CX;

        [Category("General Registers")]
        [Description("Data High (DH) byte register.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        [PropertyOrder(7)]
        public byte DH => _cpu.DH;

        [Category("Index Registers")]
        [Description("The Destination Index (DI) register stores the offset from the base register (BX or BP).")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        [PropertyOrder(2)]
        public ushort DI => _cpu.DI;

        [Category("General Registers")]
        [Description("Data Low (DL) byte register.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        [PropertyOrder(8)]
        public byte DL => _cpu.DL;

        [Category("General Registers")]
        [Description("Data (DX) word register.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        [PropertyOrder(6)]
        public ushort DX => _cpu.DX;

        [Category("Stack Registers")]
        [Description("The Data Segment (DS) register stores the location of data in memory.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public ushort DS => _cpu.DS;

        [Category("Stack Registers")]
        [Description("The Extra Segment (ES) register stores the location of miscellaneous data in memory, usually used for far pointer addressing.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public ushort ES => _cpu.ES;

        [Category("Instruction Registers")]
        [Description("The program counter or Instruction Pointer (IP) stores the location of next instruction in memory.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public ushort IP => _cpu.IP;

        [Category("Instruction Registers")]
        [Description("The Instruction Register (IR) stores the currently executing instruction.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public ushort IR => _cpu.IR;

        [Category("Index Registers")]
        [Description("The Source Index (SI) register stores the offset from the base register (BX or BP).")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        [PropertyOrder(3)]
        public ushort SI => _cpu.SI;

        [Category("Index Registers")]
        [Description("Stack Pointer (SP) register stores the location in memory where the next value in the stack will be stored.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        [PropertyOrder(0)]
        public ushort SP => _cpu.SP;

        [Category("Stack Registers")]
        [Description("The Stack Segment (SS) register stores the location of the stack in memory.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public ushort SS => _cpu.SS;

        [Category("Instruction Registers")]
        [Description("Instruction Step counter.")]
        [TypeConverter(typeof(HexadecimalTypeConverter))]
        public byte Step => _cpu.Step;
    }
}