namespace FM.Assembler
{
    internal enum TokenKind
    {
        Unknown,
        End,
        Label,
        SegmentDefinition,
        Operation,
        StringLiteral,
        IntegerLiteral,
        OpenParen,
        CloseParen,
        Comma,
        Colon,
        SemiColon,
        Register
    }
}