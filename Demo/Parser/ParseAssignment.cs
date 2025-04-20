namespace Demo
{
    public partial class Parser
    {
        // Парсит присваивание и возвращает узел AST
        private StmtAst ParseAssignment()
        {
            var idToken = Peek();
            Advance(); // Съедаем идентификатор

            if (!Expect(TokenKind.Assignment, "Ожидался оператор '=' после идентификатора"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            var value = ParseExpressionAst();
            if (value == null)
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            if (!Expect(TokenKind.Semicolon, "Ожидался ';' после выражения"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            return new AssignStmtAst(idToken.Line, idToken.Column, idToken.Lexeme, value);
        }
    }
}