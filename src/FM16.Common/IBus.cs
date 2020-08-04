namespace FM16.Common
{
    public interface IBus
    {
        byte ReadByte();

        uint ReadDouble();

        ushort ReadWord();

        void WriteByte(byte value);

        void WriteDouble(uint value);

        void WriteWord(ushort value);
    }
}