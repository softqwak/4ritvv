using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class SimpleLexer
    {
        private readonly string _source;
        private int _position;

        public SimpleLexer(string source)
        {
            _source = source;
        }

        public List<Token> Tokenize()
        {
            var tokens = new List<Token>();
            Token token;
            do
            {
                token = NextToken();
                tokens.Add(token);
            } while (token.Kind != TokenKind.EndOfFile);
            return tokens;
        }

        private Token NextToken()
        {
            while (_position < _source.Length && char.IsWhiteSpace(_source[_position]))
                _position++;

            if (_position >= _source.Length)
                return new Token(TokenKind.EndOfFile, string.Empty, _position);

            char ch = _source[_position];

            if (char.IsLetter(ch))
            {
                int start = _position;
                while (_position < _source.Length && char.IsLetterOrDigit(_source[_position]))
                    _position++;

                string word = _source.Substring(start, _position - start);
                return new Token(KeywordToKind(word), word, start);
            }

            if (char.IsDigit(ch))
            {
                int start = _position;
                while (_position < _source.Length && char.IsDigit(_source[_position]))
                    _position++;

                string number = _source.Substring(start, _position - start);
                return new Token(TokenKind.Int, number, start);
            }

            int tokenPos = _position++;
            switch (ch)
            {
                case '=': return new Token(TokenKind.Assignment, "=", tokenPos);
                case ':': return new Token(TokenKind.Colon, ":", tokenPos);
                case '"': return new Token(TokenKind.DoubleQuotes, "\"", tokenPos);

                case '{': return new Token(TokenKind.OpenBrace, "{", tokenPos);
                case '}': return new Token(TokenKind.CloseBrace, "}", tokenPos);

                case '+': return new Token(TokenKind.Plus, "+", tokenPos);
                case '-': return new Token(TokenKind.Minus, "-", tokenPos);
                case '*': return new Token(TokenKind.Asterisk, "*", tokenPos);
                case '/': return new Token(TokenKind.Slash, "/", tokenPos);
                case '(': return new Token(TokenKind.OpenParen, "(", tokenPos);
                case ')': return new Token(TokenKind.CloseParen, ")", tokenPos);
                
                case ';': return new Token(TokenKind.Semicolon, ";", tokenPos);
            }

            return new Token(TokenKind.Invalid, ch.ToString(), tokenPos);
        }

        private static TokenKind KeywordToKind(string word)
        {
            switch (word)
            {
                case "int": return TokenKind.Int;
                case "string": return TokenKind.String;
                case "float": return TokenKind.Float;

                case "//": return TokenKind.DoubleSlash;

                case "if": return TokenKind.If;
                case "else": return TokenKind.Else;
                case "while": return TokenKind.While;
                case "for": return TokenKind.For;
                case "fn": return TokenKind.Fn;
                case "return": return TokenKind.Return;
                case "class": return TokenKind.Class;
                case "extends": return TokenKind.Extends;
                case "new": return TokenKind.New;
                case "virt": return TokenKind.Virt;
                case "impl": return TokenKind.Impl;
                case "let": return TokenKind.Let;
                case "del": return TokenKind.Del;
                case "print": return TokenKind.Print;                

                default: return TokenKind.Identifier;
            }
        }
    }
}
