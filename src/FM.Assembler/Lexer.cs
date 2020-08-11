using System;
using System.Collections.Generic;
using System.Text;

namespace FM.Assembler
{
    public class Lexer
    {
        private char? _ch;
        private int _position;
        private Token _token;

        public Lexer(string text, bool moveToFirstToken)
        {
            Text = text;
            SetPosition(0);

            if (moveToFirstToken)
            {
                NextToken();
            }
        }

        public string Text { get; }

        internal Token CurrentToken { get; private set; }

        internal int Position => _position;

        internal Token NextToken()
        {
            var token = NextToken(out var error);

            if (error != null)
            {
                throw error;
            }

            return token;
        }

        internal Token PeekNextToken()
        {
            TryPeekNextToken(out var outToken, out var error);

            if (error != null)
            {
                throw error;
            }

            return outToken;
        }

        internal void SetPosition(int position)
            => _position = position;

        private void AdvanceToNextOccurranceOf(char ch)
        {
            NextChar();

            while (_ch.HasValue && _ch != ch)
            {
                NextChar();
            }
        }

        private void AdvanceThroughBalancedExpression(char startChar, char endChar)
        {
            int currentDepth = 1;

            while (currentDepth > 0)
            {
                if (_ch == '\'')
                {
                    AdvanceToNextOccurranceOf('\'');
                }

                if (_ch == startChar)
                {
                    currentDepth++;
                }
                else if (_ch == endChar)
                {
                    currentDepth--;
                }
                else if (_ch == null)
                {
                    throw new InvalidOperationException("Unbalanced expression.");
                }

                NextChar();
            }
        }

        private bool IsValidDigit()
            => _ch.HasValue && char.IsDigit(_ch.Value);

        private bool IsValidNonStartingCharForIdentifier()
            => _ch.HasValue && char.IsLetterOrDigit(_ch.Value);

        private bool IsValidStartingCharForIdentifier()
            => _ch.HasValue && char.IsLetter(_ch.Value);

        private bool IsValidWhiteSpace()
            => _ch.HasValue && char.IsWhiteSpace(_ch.Value);

        private void NextChar()
        {
            if (_position < Text.Length)
            {
                _position++;

                if (_position < Text.Length)
                {
                    _ch = Text[_position];
                    return;
                }
            }

            _ch = null;
        }

        private Token NextToken(out Exception exception)
        {
            exception = null;

            var kind = TokenKind.Unknown;
            int tokenPos = _position;

            switch (_ch)
            {
                case '\'':
                case '\"':

                    break;
            }

            _token.Kind = kind;
            _token.Text = Text.Substring(tokenPos, _position - tokenPos);
            _token.Position = tokenPos;

            return _token;
        }

        private bool TryPeekNextToken(out Token resultToken, out Exception error)
        {
            int savedTextPos = _position;
            char? savedChar = _ch;
            Token savedToken = _token;

            resultToken = NextToken(out error);

            _position = savedTextPos;
            _ch = savedChar;
            _token = savedToken;

            return error == null;
        }
    }
}
