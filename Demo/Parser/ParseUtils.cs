using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public partial class Parser
    {
        private bool Match(TokenKind kind) => Peek().Kind == kind;

        private bool Match(params TokenKind[] kinds)
        {
            var currentKind = Peek().Kind;
            foreach (var kind in kinds)
            {
                if (currentKind == kind)
                    return true;
            }
            return false;
        }

        public bool Expect(TokenKind kind)
        {
            if (Match(kind))
            {
                Advance();
                return true;
            }
            return false;
        }

        public bool Expect(TokenKind kind, string expectedMessage = null)
        {
            if (Match(kind))
            {
                Advance();
                return true;
            }

            Error(expectedMessage ?? $"Ожидался {kind}");
            return false;
        }

        private Token Peek(int n = 0)
        {
            if (_position + n >= _tokens.Count)
                return new Token(TokenKind.EndOfFile, "", _position, _line, _column);
            if (_position + n > 0)
                return _tokens[_position + n];
            else
                return _tokens[_position];
        }

        private void Advance()
        {
            if (_position < _tokens.Count)
            {
                lexems.Add($"Процессинг токена: {Peek().Lexeme} ({Peek().Kind}) Category: {Peek().Category}");
            }
            _position++;
        }

        private void Error(string message, Token token = null)
        {
            if (token == null)
                token = Peek(-1);
            Diagnostics.Report(token.Line, token.Column, message);
        }

        private void Warning(string message, Token token = null)
        {
            if (token == null)
                token = Peek();
            Diagnostics.Warning(token.Line, token.Column, message);
        }

        private void SynchronizeTo(params TokenKind[] kinds)
        {
            while (!Match(TokenKind.EndOfFile))
            {
                if (kinds.Contains(Peek().Kind))
                    return;
                Advance();
            }
        }



        private void SyncToEndOfStatement() =>
            SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);

        private void SyncToBlockEnd() =>
            SynchronizeTo(TokenKind.CloseBrace, TokenKind.OpenBrace, TokenKind.EndOfFile);
    }
}
