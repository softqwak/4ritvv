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

        // Парсит первичное выражение (числа, строки, идентификаторы) и возвращает узел AST
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
                if (double.TryParse(token.Lexeme, out double value))
                {
                    return new FloatExprAst(token.Line, token.Column, value);
                }
                Error("Некорректное вещественное число", token);
                return null;
            }
            else if (Match(TokenKind.StringText))
            {
                Advance();
                return new StringExprAst(token.Line, token.Column, token.Lexeme);
            }
            else if (Match(TokenKind.Identifier))
            {
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
    }
}