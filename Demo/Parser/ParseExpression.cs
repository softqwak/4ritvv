using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace Demo
{
    public partial class Parser
    {
        // Парсит арифметическое выражение (+, -) и возвращает узел AST
        private ExprAst ParseExpressionAst()
        {
            var left = ParseTermAst();
            if (left == null)
            {
                return null;
            }

            while (Match(TokenKind.Plus, TokenKind.Minus))
            {
                var opToken = Peek();
                var op = opToken.Lexeme; // "+" или "-"
                Advance(); // Съедаем '+' или '-'
                var right = ParseTermAst();
                if (right == null)
                {
                    return null;
                }
                left = new BinaryExprAst(opToken.Line, opToken.Column, op, left, right);
            }

            return left;
        }

        // Парсит терм (*, /, %) и возвращает узел AST
        private ExprAst ParseTermAst()
        {
            var left = ParseFactorAst();
            if (left == null)
            {
                return null;
            }

            while (Match(TokenKind.Asterisk, TokenKind.Slash, TokenKind.Percent))
            {
                var opToken = Peek();
                var op = opToken.Lexeme; // "*", "/" или "%"
                Advance(); // Съедаем '*', '/' или '%'
                var right = ParseFactorAst();
                if (right == null)
                {
                    return null;
                }
                left = new BinaryExprAst(opToken.Line, opToken.Column, op, left, right);
            }

            return left;
        }

        // Парсит фактор (числа, строки, идентификаторы, скобки) и возвращает узел AST
        private ExprAst ParseFactorAst()
        {
            if (Match(TokenKind.OpenParen))
            {
                var openParenToken = Peek();
                Advance(); // Съедаем '('
                var expr = ParseExpressionAst();
                if (expr == null)
                {
                    return null;
                }
                if (!Expect(TokenKind.CloseParen, "Ожидалась ')'"))
                {
                    return null;
                }
                return expr;
            }
            else
            {
                return ParseUnaryAst();
            }
        }

        // Парсит унарное выражение (например, ++i) и возвращает узел AST
        private ExprAst ParseUnaryAst()
        {
            if (Match(TokenKind.PlusPlus))
            {
                var opToken = Peek();
                Advance(); // Съедаем '++'
                var operand = ParsePrimaryAst();
                if (operand == null)
                {
                    return null;
                }
                return new UnaryExprAst(opToken.Line, opToken.Column, "++", operand);
            }
            return ParsePrimaryAst();
        }

        // Парсит первичное выражение (числа, строки, идентификаторы, доступ к полям) и возвращает узел AST
        private ExprAst ParsePrimaryAst()
        {
            var token = Peek();
            if (Match(TokenKind.NumberInt))
            {
                Advance();
                if (int.TryParse(token.Lexeme, out int value))
                {
                    return new IntExprAst(token.Line, token.Column, value);
                }
                Error("Некорректное целое число", token);
                return null;
            }
            else if (Match(TokenKind.NumberFloat))
            {
                Advance();
                // Проверяем содержимое token.Lexeme для отладки
                if (string.IsNullOrWhiteSpace(token.Lexeme))
                {
                    Error("Пустой или некорректный токен для вещественного числа", token);
                    return null;
                }

                // Используем InvariantCulture для корректного разбора точки как десятичного разделителя
                if (double.TryParse(token.Lexeme, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                {
                    return new FloatExprAst(token.Line, token.Column, value);
                }

                // Выводим значение token.Lexeme для диагностики
                MessageBox.Show($"Не удалось распознать вещественное число: '{token.Lexeme}'");
                Error($"Некорректное вещественное число: '{token.Lexeme}'", token);
                return null;
            }
            else if (Match(TokenKind.StringText))
            {
                Advance();
                return new StringExprAst(token.Line, token.Column, token.Lexeme);
            }
            else if (Match(TokenKind.Identifier))
            {
                // Проверяем, является ли это доступом к полю (например, obj.field)
                var nextToken = Peek(1);
                if (nextToken.Kind == TokenKind.Period)
                {
                    return ParseMemberAccessExpr();
                }
                // Обычный идентификатор
                Advance();
                return new IdentifierExprAst(token.Line, token.Column, token.Lexeme);
            }
            else
            {
                Error("Ожидалось значение или выражение", token);
                Advance();
                return null;
            }
        }


        // Парсит выражение new
        // Формат: new Square((i + 1) * 10)
        private StmtAst ParseNewExpression()
        {
            var newToken = Peek(); // Съедено 'new'
            Advance();
            // Ожидаем имя класса
            if (!Expect(TokenKind.Identifier, "Ожидалось имя класса после new"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }
            var className = Peek(-1).Lexeme;

            // Парсим аргументы
            var arguments = new List<ExprAst>();
            if (!Expect(TokenKind.OpenParen, "Ожидалась '(' для начала списка аргументов"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            if (!Match(TokenKind.CloseParen))
            {
                arguments = ParseArgumentList();
                if (arguments == null)
                {
                    SynchronizeTo(TokenKind.CloseParen, TokenKind.Semicolon, TokenKind.EndOfFile);
                    return null;
                }
            }
            else
            {
                Advance();
            }

            return new NewStmt(newToken.Line, newToken.Column, className, arguments);
        }
    }
}