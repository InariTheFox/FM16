using System;

namespace FM16.Emulation.Operations
{
    public class OperationMnemonicAttribute : Attribute
    {
        public OperationMnemonicAttribute(string mnemonic)
        {
            Mnemonic = mnemonic;
        }

        public string Mnemonic { get; }
    }
}
