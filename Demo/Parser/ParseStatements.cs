namespace Demo
{
    public partial class Parser
    {
        private StmtAst ParseStatement()
        {
            var token = Peek();
            var tokenNext = Peek(1).Kind;
            if (Match(TokenKind.Let))
            {
                return ParseVariableDeclaration();
            }
            else if (token.Kind == TokenKind.Identifier && tokenNext == TokenKind.Colon)
            {
                return ParseVariableDeclaration();
            }
            else if (token.Kind == TokenKind.Identifier && tokenNext == TokenKind.Assignment)
            {
                return ParseAssignment();
            }
            // Обработка вызова метода (например, obj.method())
            else if (token.Kind == TokenKind.Identifier && tokenNext == TokenKind.Period)
            {
                // Смотрим на токены после точки
                var tokenAfterPeriod = Peek(2).Kind;
                var tokenAfterMember = Peek(3).Kind;

                // Если после имени члена есть '(', это вызов метода (obj.method())
                if (tokenAfterPeriod == TokenKind.Identifier && tokenAfterMember == TokenKind.OpenParen)
                {
                    return ParseMemberAccessStmt();
                }
                // Если это доступ к полю (obj.field), это ошибка в контексте оператора
                else if (tokenAfterPeriod == TokenKind.Identifier)
                {
                    Error("Доступ к полю не является оператором", token);
                    SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                    Advance();
                    return null;
                }
                else
                {
                    Error("Некорректная конструкция после '.'", Peek(1));
                    SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                    Advance();
                    return null;
                }
            }
            // Обработка вызова функции (например, foo())
            else if (token.Kind == TokenKind.Identifier && tokenNext == TokenKind.OpenParen)
            {
                // TODO: Реализовать ParseCallFunction
                Error("Вызов функции пока не поддерживается", token);
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                Advance();
                return null;
            }
            else if (token.Kind == TokenKind.Print)
            {
                return ParsePrint();
            }
            else if (token.Kind == TokenKind.If)
            {
                return ParseIf();
            }
            else if (token.Kind == TokenKind.For)
            {
                return ParseFor();
            }
            else if (token.Kind == TokenKind.Class)
            {
                return ParseClassDeclaration();
            }
            else if (token.Kind == TokenKind.Return)
            {
                return ParseReturnStatement();
            }
            else if (token.Kind == TokenKind.Fn)
            {
                return ParseFunctionDeclaration();
            }
            else if (token.Kind == TokenKind.New)
            {
                return ParseNewExpression();
            }
            else if (token.Kind == TokenKind.Del)
            {
                return ParseDelExpression();
            }
            Error($"Синтаксическая ошибка - '{token.Lexeme}' неизвестная конструкция", token);
            SynchronizeTo(TokenKind.Semicolon, TokenKind.CloseBrace, TokenKind.Identifier);
            Advance();
            return null;

        }
    }
}