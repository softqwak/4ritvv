using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public class SimpleLexer
    {
        private readonly string _source;
        private int _position;
        private int _line;
        private int _column;

        public SimpleLexer(string source)
        {
            _source = source;
            _position = 0;
            _line = 1;
            _column = 1;
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
            // Пропуск пробелов и символов новой строки
            while (_position < _source.Length && char.IsWhiteSpace(_source[_position]))
            {
                if (_source[_position] == '\n' || _source[_position] == '\r')
                {
                    if (_source[_position] == '\r' && _position + 1 < _source.Length && _source[_position + 1] == '\n')
                    {
                        _position++; // Пропустить '\n' после '\r'
                    }

                    _line++;
                    _column = 1;
                }
                else
                {
                    _column++;
                }

                _position++;
            }

            if (_position >= _source.Length)
                return new Token(TokenKind.EndOfFile, string.Empty, _position, _line, _column);

            char ch = _source[_position];

            if (char.IsLetter(ch))
            {
                int start = _position;
                int startLine = _line;
                int startColumn = _column;

                while (_position < _source.Length && char.IsLetterOrDigit(_source[_position]))
                {
                    _position++;
                    _column++;
                }

                string word = _source.Substring(start, _position - start);
                return new Token(KeywordToKind(word), word, start, startLine, startColumn);
            }

            if (char.IsDigit(ch))
            {
                int start = _position;
                int startLine = _line;
                int startColumn = _column;

                while (_position < _source.Length && char.IsDigit(_source[_position]))
                {
                    _position++;
                    _column++;
                }

                string number = _source.Substring(start, _position - start);
                return new Token(TokenKind.NumberInt, number, start, startLine, startColumn);
            }

            // Обработка строк
            if (ch == '"')
            {
                int start = _position;
                int startLine = _line;
                int startColumn = _column;

                _position++; // пропустить открывающую кавычку
                _column++;

                var builder = new StringBuilder();
                while (_position < _source.Length && _source[_position] != '"')
                {
                    builder.Append(_source[_position]);
                    _position++;
                    _column++;
                }

                if (_position < _source.Length && _source[_position] == '"')
                {
                    _position++; // пропустить закрывающую кавычку
                    _column++;
                    string content = builder.ToString();
                    return new Token(TokenKind.StringText, content, start, startLine, startColumn);
                }
                else
                {
                    // незавершённая строка
                    return new Token(TokenKind.Invalid, _source.Substring(start, _position - start), start, startLine, startColumn);
                }
            }

            // Проверка на составные операторы (например, ==, !=, <=, >=)
            if (ch == '=')
            {
                if (_position + 1 < _source.Length && _source[_position + 1] == '=')
                {
                    _position += 2; // Пропускаем оба символа '=='
                    _column += 2;
                    return new Token(TokenKind.Equal, "==", _position - 2, _line, _column);
                }
                return new Token(TokenKind.Assignment, "=", _position++, _line, _column++);
            }
            if (ch == '!')
            {
                if (_position + 1 < _source.Length && _source[_position + 1] == '=')
                {
                    _position += 2; // Пропускаем оба символа '!='
                    _column += 2;
                    return new Token(TokenKind.NotEqual, "!=", _position - 2, _line, _column);
                }
                return new Token(TokenKind.Not, "!", _position++, _line, _column++);
            }
            if (ch == '<')
            {
                if (_position + 1 < _source.Length && _source[_position + 1] == '=')
                {
                    _position += 2; // Пропускаем оба символа '<='
                    _column += 2;
                    return new Token(TokenKind.LessEqual, "<=", _position - 2, _line, _column);
                }
                return new Token(TokenKind.Less, "<", _position++, _line, _column++);
            }
            if (ch == '>')
            {
                if (_position + 1 < _source.Length && _source[_position + 1] == '=')
                {
                    _position += 2; // Пропускаем оба символа '>='
                    _column += 2;
                    return new Token(TokenKind.GreaterEqual, ">=", _position - 2, _line, _column);
                }
                return new Token(TokenKind.Greater, ">", _position++, _line, _column++);
            }
            if (ch == '&')
            {
                if (_position + 1 < _source.Length && _source[_position + 1] == '&')
                {
                    _position += 2; // Пропускаем оба символа '&&'
                    _column += 2;
                    return new Token(TokenKind.AndAnd, "&&", _position - 2, _line, _column);
                }
                return new Token(TokenKind.Invalid, "&", _position++, _line, _column++);
            }
            if (ch == '|')
            {
                if (_position + 1 < _source.Length && _source[_position + 1] == '|')
                {
                    _position += 2; // Пропускаем оба символа '&&'
                    _column += 2;
                    return new Token(TokenKind.OrOr, "||", _position - 2, _line, _column);
                }
                return new Token(TokenKind.Invalid, "|", _position++, _line, _column++);
            }

            if (ch == '+')
            {
                if (_position + 1 < _source.Length && _source[_position + 1] == '+')
                {
                    _position += 2; // Пропускаем оба символа '++'
                    _column += 2;
                    return new Token(TokenKind.PlusPlus, "++", _position - 2, _line, _column);
                }
                return new Token(TokenKind.Plus, "+", _position++, _line, _column++);
            }

            // Обработка комментариев "//"
            if (ch == '/' && _position + 1 < _source.Length && _source[_position + 1] == '/')
            {
                int start = _position;
                _position += 2; // Пропустить "//"
                _column += 2;

                // Пропускаем весь комментарий
                while (_position < _source.Length && _source[_position] != '\n' && _source[_position] != '\r')
                {
                    _position++;
                    _column++;
                }

                return new Token(TokenKind.DoubleSlash, _source.Substring(start, _position - start), start, _line, _column);
            }

            // Прочие символы
            int tokenPos = _position++;
            var tokenChar = _source[tokenPos];
            switch (tokenChar)
            {
                case ':': return new Token(TokenKind.Colon, ":", tokenPos, _line, _column);
                case '{': return new Token(TokenKind.OpenBrace, "{", tokenPos, _line, _column);
                case '}': return new Token(TokenKind.CloseBrace, "}", tokenPos, _line, _column);
                case '+': return new Token(TokenKind.Plus, "+", tokenPos, _line, _column);
                case '-': return new Token(TokenKind.Minus, "-", tokenPos, _line, _column);
                case '*': return new Token(TokenKind.Asterisk, "*", tokenPos, _line, _column);
                case '/': return new Token(TokenKind.Slash, "/", tokenPos, _line, _column);
                case '%': return new Token(TokenKind.Percent, "%", tokenPos, _line, _column);
                case '(': return new Token(TokenKind.OpenParen, "(", tokenPos, _line, _column);
                case ')': return new Token(TokenKind.CloseParen, ")", tokenPos, _line, _column);
                case ';': return new Token(TokenKind.Semicolon, ";", tokenPos, _line, _column);
                case ',': return new Token(TokenKind.Comma, ",", tokenPos, _line, _column);
                case '!': return new Token(TokenKind.Not, "!", tokenPos, _line, _column);
            }

            return new Token(TokenKind.Invalid, tokenChar.ToString(), tokenPos, _line, _column);
        }


        private static TokenKind KeywordToKind(string word)
        {
            switch (word)
            {
                case "int": return TokenKind.Int;
                case "string": return TokenKind.String;
                case "float": return TokenKind.Float;

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