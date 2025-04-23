namespace Demo
{
    public partial class Parser
    {
        // Парсит оператор return
        // Формат: return 0; или return; или return x; или return "3";
        private StmtAst ParseReturnStatement()
        {
            var returnToken = Peek(); // Съедено 'return'
            Advance();
            ExprAst value = null;
            if (!Match(TokenKind.Semicolon))
            {
                value = ParseExpressionAst();
                if (value == null)
                {
                    SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                    return null;
                }
            }

            if (!Expect(TokenKind.Semicolon, "Ожидался ';' после return"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            return new ReturnStmt(returnToken.Line, returnToken.Column, value);
        }
    }
}
