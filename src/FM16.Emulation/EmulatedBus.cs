using FM16.Common;

namespace FM16.Emulation
{
    public class EmulatedBus : IBus
    {
        private uint _value;

        public byte ReadByte() => throw new System.NotImplementedException();

        public uint ReadDouble() => throw new System.NotImplementedException();

        public ushort ReadWord() => throw new System.NotImplementedException();

        public void WriteByte(byte value)
            => _value = value;

        public void WriteDouble(uint value)
            => _value = value;

        public void WriteWord(ushort value)
            => _value = value;
    }
}