using System;

namespace FM16.Emulation.Operations
{
    public class OperationOpCodeAttribute : Attribute
    {
        public OperationOpCodeAttribute(byte opCode)
            : this(opCode, null) { }

        public OperationOpCodeAttribute(byte opCode, byte? prefix = null)
        {
            OpCode = opCode;
            Prefix = prefix;
        }

        public byte OpCode { get; }

        public byte? Prefix { get; }
    }
}
