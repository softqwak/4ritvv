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
        private List<Token> _tokens = new List<Token>();
        public List<string> lexems = new List<string>();

        private int _position;
        private int _line;
        private int _column;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _position = 0;
            _line = 0;
            _column = 0;
        }


        public void Parse()
        {
            Diagnostics._messagesError.Clear();
            Diagnostics._messagesWarning.Clear();
            ParseProgram();
        }

        private void ParseProgram()
        {
            while (!Match(TokenKind.EndOfFile))
            {
                ParseTopLevel();
            }
        }

        private void ParseTopLevel()
        {
            // В будущем здесь будет обработка функций, классов, и т.д.
            // Пока обрабатываем только операторы верхнего уровня
            ParseStatement();
        }



        public bool Expect(TokenKind kind)
        {
            if (!Match(kind))
                return false;
            else
            {
                Advance();
                return true;
            }
        }
        private void SynchronizeTo(params TokenKind[] kinds)
        {
            while (!Match(TokenKind.EndOfFile)) // пока не достигнут конец файла...
            {
                if (kinds.Contains(Peek().Kind)) // если текущий токен один из ожидаемых (например, ')' или ';')
                    return;                      // мы "синхронизировались", можно выйти

                Advance(); // иначе — просто пропускаем токены, пока не дойдём до нужного
            }
        }

        private Token Peek(int n = 0)
        {
            if (_position >= _tokens.Count || _position + n >= _tokens.Count)
                return new Token(TokenKind.EndOfFile, "", _position, _line, _column);
            return _tokens[_position + n];
        }

        private void Advance()
        {
            if (_position < _tokens.Count)
            {
                lexems.Add($"Процессинг токена: {Peek().Lexeme} ({Peek().Kind})");
            }
            _position++;
        }

        private bool Match(TokenKind kind)
        {
            return Peek().Kind == kind;
        }

        private void ParseExpression()
        {
            var token = Peek();
            if (token.Kind == TokenKind.EndOfFile) return; // Выход, если достигнут конец файла

            // Если токен неожидан, синхронизируем
            if (token.Kind == TokenKind.Invalid)
            {
                Diagnostics.Report(token.Line, token.Column, "Неверный токен");
                SynchronizeTo(TokenKind.Semicolon, TokenKind.OpenBrace);  // синхронизируем до ожидаемых символов
                return;
            }

            switch (token.Kind)
            {
                case TokenKind.NumberInt:
                case TokenKind.NumberFloat:
                case TokenKind.StringText:
                case TokenKind.Identifier:
                    Advance();
                    break;

                default:
                    Diagnostics.Report(token.Line, token.Column, "Ожидалось выражение");
                    SynchronizeTo(TokenKind.Semicolon, TokenKind.CloseBrace);
                    break;
            }
        }

        public void ParseStatement()
        {
            var token = Peek().Kind;
            var tokenNext = Peek(1).Kind;
            if (Match(TokenKind.Let))
            {
                ParseVariableDeclaration();
            }
            else if (token == TokenKind.Identifier && tokenNext == TokenKind.Colon)
            {
                // Объявление без 'let' (например, при глобальном определении)
                ParseVariableDeclaration();
            }
            else if (tokenNext == TokenKind.Assignment)
            {
                ParseAssignment();
            }
            else if (token == TokenKind.Print)
            {
                ParsePrint();
            }
            else
            {
                Diagnostics.Report(Peek().Line, Peek().Column, "Ожидался оператор или выражение");
                SynchronizeTo(TokenKind.Semicolon, TokenKind.CloseBrace, TokenKind.Identifier);
                Advance();
            }
        }

        private void ParseVariableDeclaration()
        {
            Expect(TokenKind.Let); // обязательно съедаем 'let' при наличии

            var token = Peek();

            if (token.Kind == TokenKind.Invalid)
            {
                Diagnostics.Report(token.Line, token.Column, "Некорректная переменная");
                SynchronizeTo(TokenKind.Semicolon);
                return;
            }

            if (!Expect(TokenKind.Identifier))
            {
                Diagnostics.Report(token.Line, token.Column, "Ожидался идентификатор переменной");
                SynchronizeTo(TokenKind.Semicolon);
                return;
            }

            // Проверка на тип (необязательный)
            if (Match(TokenKind.Colon))
            {
                Advance(); // съедаем ':'

                if (!(Expect(TokenKind.Int) || Expect(TokenKind.String) || Expect(TokenKind.Float)))
                {
                    Diagnostics.Report(Peek().Line, Peek().Column, "Ожидался тип после ':'");
                    SynchronizeTo(TokenKind.Semicolon);
                    return;
                }
            }

            // Проверка на инициализацию (= значение)
            if (Match(TokenKind.Assignment))
            {
                Advance();
                ParseExpression(); // разбираем выражение (например, число)
            }
            else
            {
                Diagnostics.Warning(Peek().Line, Peek().Column, $"Переменная '{Peek().Lexeme}' объявлена, но не инициализирована");
            }

            if (!Expect(TokenKind.Semicolon))
            {
                Diagnostics.Report(Peek().Line, Peek().Column, "Ожидался символ ';' в конце объявления переменной");
                SynchronizeTo(TokenKind.Semicolon);
            }

            // TODO: добавить узел в AST
        }


        private void ParseAssignment()
        {
            Advance();

            if (!Expect(TokenKind.Assignment))
            {
                Diagnostics.Report(Peek().Line, Peek().Column, "Ожидался оператор '=' после идентификатора");
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return;
            }

            ParseExpression(); // Пока просто литералы

            if (!Expect(TokenKind.Semicolon))
            {
                Diagnostics.Report(Peek().Line, Peek().Column, "Ожидался ';' после выражения");
            }

            // TODO: добавить в AST узел Assignment
        }

        private void ParsePrint()
        {
            Advance(); // съедаем 'print'

            if (!Expect(TokenKind.OpenParen))
            {
                Diagnostics.Report(Peek().Line, Peek().Column, "Ожидалась '(' после 'print'");
                SynchronizeTo(TokenKind.Semicolon, TokenKind.CloseBrace, TokenKind.EndOfFile);
                return;
            }

            if (Match(TokenKind.CloseParen))
            {
                Advance(); // пустой список
            }
            else
            {
                ParseExpression();

                while (true)
                {
                    if (Match(TokenKind.Comma))
                    {
                        Advance();
                        ParseExpression();
                    }
                    else if (Match(TokenKind.CloseParen))
                    {
                        break;
                    }
                    else
                    {
                        Diagnostics.Report(Peek().Line, Peek().Column, "Ожидалась ',' или ')'");
                        // Можно попытаться синхронизироваться до следующего возможного аргумента
                        SynchronizeTo(TokenKind.Comma, TokenKind.CloseParen, TokenKind.Semicolon);
                        if (Match(TokenKind.Comma))
                        {
                            Advance(); // пропускаем запятую и продолжаем
                            ParseExpression();
                            continue;
                        }
                        else if (Match(TokenKind.CloseParen))
                        {
                            break;
                        }
                        else
                        {
                            // если ни , ни ) — выходим
                            return;
                        }
                    }
                }

                if (!Expect(TokenKind.CloseParen))
                {
                    Diagnostics.Report(Peek().Line, Peek().Column, "Ожидалась ')'");
                    SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                    return;
                }
            }

            if (!Expect(TokenKind.Semicolon))
            {
                Diagnostics.Report(Peek().Line, Peek().Column, "Ожидался символ ';'");
            }
        }

        private void ParseBlock()
        {
            if (!Expect(TokenKind.OpenBrace))
            {
                Diagnostics.Report(Peek().Line, Peek().Column, "Ожидался '{' для начала блока");
                return;
            }

            while (!Match(TokenKind.CloseBrace) && !Match(TokenKind.EndOfFile))
            {
                ParseStatement();
            }

            if (!Expect(TokenKind.CloseBrace))
            {
                Diagnostics.Report(Peek().Line, Peek().Column, "Ожидался '}' для завершения блока");
            }

            // TODO: здесь можно будет возвращать AST-узел BlockNode
        }


    }


}
