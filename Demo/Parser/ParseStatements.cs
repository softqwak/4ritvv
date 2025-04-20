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
            else
            {
                Error($"Синтаксическая ошибка - '{token.Lexeme}' неизвестная конструкция", token);
                SynchronizeTo(TokenKind.Semicolon, TokenKind.CloseBrace, TokenKind.Identifier);
                Advance();
                return null;
            }
        }
    }
}