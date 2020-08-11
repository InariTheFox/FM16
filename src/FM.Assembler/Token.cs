namespace FM.Assembler
{
    internal struct Token
    {
        public TokenKind Kind { get; set; }

        public int Position { get; set; }

        public string Text { get; set; }
    }
}