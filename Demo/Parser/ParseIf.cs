using System;

namespace Demo
{
    public partial class Parser
    {
        // Парсит конструкцию if-else и возвращает узел AST
        private StmtAst ParseIf()
        {
            var ifToken = Peek();
            Advance(); // Съедаем 'if'

            // Проверяем опциональные скобки
            bool hasParens = Match(TokenKind.OpenParen);
            if (hasParens)
            {
                Advance(); // Съедаем '('
            }

            // Парсим условие
            var condition = ParseConditionAst();
            if (condition == null)
            {
                SynchronizeTo(TokenKind.OpenBrace, TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            // Проверяем закрывающую скобку, если были открывающие
            if (hasParens && !Expect(TokenKind.CloseParen, "Ожидалась закрывающая скобка ')'"))
            {
                SynchronizeTo(TokenKind.OpenBrace, TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            // Парсим блок then
            var thenBody = ParseBlock();
            if (thenBody == null)
            {
                SynchronizeTo(TokenKind.Else, TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            // Проверяем наличие else
            StmtAst elseBody = null;
            if (Match(TokenKind.Else))
            {
                Advance(); // Съедаем 'else'
                elseBody = ParseBlock();
                if (elseBody == null)
                {
                    SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                    return null;
                }
            }

            // Создаём узел AST
            return new IfStmtAst(ifToken.Line, ifToken.Column, condition, thenBody, elseBody);
        }

        // Парсит условие (логическое выражение) и возвращает узел AST
        private ExprAst ParseConditionAst()
        {
            return ParseLogicalOrAst();
        }

        // Парсит логическое ИЛИ (||) и возвращает узел AST
        private ExprAst ParseLogicalOrAst()
        {
            var left = ParseLogicalAndAst();
            if (left == null)
            {
                return null;
            }

            while (Match(TokenKind.OrOr))
            {
                var opToken = Peek();
                var op = opToken.Lexeme; // "||"
                Advance(); // Съедаем '||'
                var right = ParseLogicalAndAst();
                if (right == null)
                {
                    return null;
                }
                left = new LogicalExprAst(opToken.Line, opToken.Column, op, left, right);
            }

            return left;
        }

        // Парсит логическое И (&&) и возвращает узел AST
        private ExprAst ParseLogicalAndAst()
        {
            var left = ParseComparisonAst();
            if (left == null)
            {
                return null;
            }

            while (Match(TokenKind.AndAnd))
            {
                var opToken = Peek();
                var op = opToken.Lexeme; // "&&"
                Advance(); // Съедаем '&&'
                var right = ParseComparisonAst();
                if (right == null)
                {
                    return null;
                }
                left = new LogicalExprAst(opToken.Line, opToken.Column, op, left, right);
            }

            return left;
        }

        // Парсит сравнение (==, !=, >, <, <=, >=) и возвращает узел AST
        private ExprAst ParseComparisonAst()
        {
            var left = ParseExpressionAst();
            if (left == null)
            {
                return null;
            }

            while (Match(TokenKind.Less, TokenKind.LessEqual, TokenKind.Greater, TokenKind.GreaterEqual,
                        TokenKind.Equal, TokenKind.NotEqual))
            {
                var opToken = Peek();
                var op = opToken.Lexeme; // "<", "<=", ">", ">=", "==", "!="
                Advance(); // Съедаем оператор сравнения
                var right = ParseExpressionAst();
                if (right == null)
                {
                    return null;
                }
                left = new BinaryExprAst(opToken.Line, opToken.Column, op, left, right);
            }

            return left;
        }


    }
}