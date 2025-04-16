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
        public bool Expect(TokenKind kind)
        {
            if (!Match(kind))
            {
                return false;
            }
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
            _position++;
        }

        private bool Match(TokenKind kind)
        {
            return Peek().Kind == kind;
        }

        public void Parse()
        {
            Diagnostics._messagesWarning.Clear();
            Diagnostics._messagesError.Clear();

            while (!Match(TokenKind.EndOfFile))
            {
                Token token = Peek();
                try
                {
                    lexems.Add($"Token: {token.Kind} -> '{token.Lexeme}'");

                    if (Match(TokenKind.Print)) // Сначала проверяем print
                    {
                        ParsePrintStatement();
                        continue; // переходим к следующей конструкции
                    }
                    else if (Match(TokenKind.Let) || Match(TokenKind.Identifier)) // Проверяем переменную после print
                    {
                        ParseVariableDeclarationOrAssignment();
                        continue;
                    }
                    else
                    {
                        // Неизвестная конструкция
                        Diagnostics.Report(token.Line, token.Column, $"Неизвестная конструкция: '{token.Lexeme}'");
                    }

                }
                catch (Exception ex)
                {
                    Diagnostics.Report(token.Line, token.Column, "Неожиданная ошибка при разборе: " + ex.Message);
                }

                Advance(); // по умолчанию двигаемся дальше
            }
        }

        private bool ParseExpression()
        {
            var token = Peek();

            // Обработка одного выражения

            if (Match(TokenKind.Identifier) || Match(TokenKind.Int) || Match(TokenKind.Float) || Match(TokenKind.StringText))
            {
                Advance();
                return true;
            }

            // Обработка ошибки, если выражение невалидно
            Diagnostics.Report(token.Line, token.Column, $"Ожидался идентификатор, число или строка, но получен {token.Kind}");
            Advance();
            return false;
        }

        private void ParsePrintStatement()
        {
            Advance(); // пропустить 'print'

            if (!Expect(TokenKind.OpenParen))
                return;

            // пустой вызов допустим: print();
            if (Match(TokenKind.CloseParen))
            {
                Advance();
            }
            else
            {
                // как минимум один аргумент
                if (!ParseExpression())
                {
                    Diagnostics.Report(Peek().Line, Peek().Column, "Ожидалось выражение внутри print(...)");
                    // пробуем восстановиться
                    SynchronizeTo(TokenKind.CloseParen, TokenKind.Semicolon);
                }
                else
                {
                    // поддержка нескольких аргументов через запятую
                    while (Match(TokenKind.Comma))
                    {
                        Advance(); // пропустить запятую

                        if (!ParseExpression())
                        {
                            Diagnostics.Report(Peek().Line, Peek().Column, "Ожидалось выражение после запятой");
                            break;
                        }
                    }

                    // После последнего выражения не должно быть запятой
                    if (Match(TokenKind.Identifier) || Match(TokenKind.StringText) || Match(TokenKind.Int) || Match(TokenKind.Float))
                    {
                        var nextToken = Peek();
                        if (nextToken.Kind != TokenKind.Comma && nextToken.Kind != TokenKind.CloseParen)
                        {
                            Diagnostics.Report(nextToken.Line, nextToken.Column, "Ожидалась запятая или закрывающая скобка, но найдено " + nextToken.Lexeme);
                        }
                    }
                }

                if (!Expect(TokenKind.CloseParen))
                {
                    // если нет закрывающей скобки — восстановимся до точки с запятой
                    SynchronizeTo(TokenKind.Semicolon);
                }
            }

            if (!Expect(TokenKind.Semicolon))
            {
                Diagnostics.Report(Peek().Line, Peek().Column, "Ожидалась ';' после вызова print");
                // можно либо восстановиться, либо продолжить
            }
        }

        

        // Новый метод для проверки отсутствия запятой между выражениями
        private void CheckForMissingComma()
        {
            var token = Peek();
            if (Match(TokenKind.Identifier) || Match(TokenKind.StringText))
            {
                // Это выражение (например, строка или идентификатор), за которым не идёт запятая, то это ошибка
                var nextToken = Peek(1); // посмотри на следующий токен
                if (nextToken.Kind != TokenKind.Comma && nextToken.Kind != TokenKind.CloseParen)
                {
                    Diagnostics.Report(token.Line, token.Column, "Ожидалась запятая между аргументами");
                }
            }
        }

        private void ParseVariableDeclarationOrAssignment()
        {
            // Поддержка ключевого слова let (необязательное)
            if (Match(TokenKind.Let))
                Advance(); // пропускаем 'let'

            if (!Match(TokenKind.Identifier))
            {
                Diagnostics.Report(Peek().Line, Peek().Column, "Ожидалось имя переменной");
                return;
            }

            var name = Peek();
            Advance();

            string type = null;

            // необязательное указание типа
            if (Match(TokenKind.Colon))
            {
                Advance(); // пропускаем ':'

                if (!Match(TokenKind.Int) && !Match(TokenKind.String) && !Match(TokenKind.Float))
                {
                    Diagnostics.Report(Peek().Line, Peek().Column, "Ожидался тип после ':'");
                    return;
                }

                type = Peek().Lexeme;
                Advance(); // пропускаем тип
            }

            // Проверка на знак присваивания '='
            if (Match(TokenKind.Assignment))
            {
                Advance(); // пропускаем '='

                if (!ParseExpression())
                {
                    Diagnostics.Report(Peek().Line, Peek().Column, "Ожидалось выражение после '='");
                    return;
                }
            }
            else
            {
                // Если нет присваивания, это просто объявление без инициализации
                Diagnostics.Warning(Peek().Line, Peek().Column, $"Переменная '{name.Lexeme}' объявлена, но не инициализирована.");
            }

            if (!Expect(TokenKind.Semicolon))
            {
                Diagnostics.Report(Peek().Line, Peek().Column, "Ожидалась ';' в конце объявления переменной");
            }

            // Для отладки (можно убрать позже)
            lexems.Add($"Variable Declaration: {name.Lexeme}" + (type != null ? $" : {type}" : "") + (Match(TokenKind.Assignment) ? " = ...;" : " ;"));
        }


    }
}
