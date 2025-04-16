using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        public readonly List<String> lexems;
        private int _position;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        public void Parse()
        {
            while (!Match(TokenKind.EndOfFile))
            {
                Token token = Peek();
                try
                {
                    lexems.Add($"Token: {token.Kind} -> '{token.Lexeme}'");
                }
                catch (Exception ex) { }
                
                Advance();
            }
        }

        private Token Peek()
        {
            if (_position >= _tokens.Count)
                return new Token(TokenKind.EndOfFile, "", _position);
            return _tokens[_position];
        }

        private void Advance()
        {
            _position++;
        }

        private bool Match(TokenKind kind)
        {
            return Peek().Kind == kind;
        }
    }
}
