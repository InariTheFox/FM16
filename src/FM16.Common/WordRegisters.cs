namespace FM16.Common
{
    public enum ByteRegisters
    {
        AL = 0x00,
        CL = 0x01,
        DL = 0x02,
        BL = 0x03,
        AH = 0x04,
        CH = 0x05,
        DH = 0x06,
        BH = 0x07
    }

    public enum WordRegisters
    {
        AX = 0x00,
        CX = 0x01,
        DX = 0x02,
        BX = 0x03,
        SP = 0x04,
        BP = 0x05,
        SI = 0x06,
        DI = 0x07
    }

    public enum SegmentRegisters : byte
    {
        ES = 0x00,
        CS = 0x01,
        SS = 0x02,
        DS = 0x03
    }
}