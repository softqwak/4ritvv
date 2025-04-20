using System.Collections.Generic;

namespace Demo
{
    public partial class Parser
    {
        // Парсит блок кода { ... } и возвращает узел AST
        // Возвращает BlockStmtAst или null в случае ошибки
        private StmtAst ParseBlock()
        {
            var openBraceToken = Peek();
            if (!Expect(TokenKind.OpenBrace, "Ожидался '{' для начала блока"))
            {
                SynchronizeTo(TokenKind.CloseBrace, TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            var statements = new List<StmtAst>();
            while (!Match(TokenKind.CloseBrace) && !Match(TokenKind.EndOfFile))
            {
                var stmt = ParseStatement();
                if (stmt != null)
                {
                    statements.Add(stmt);
                }
            }

            if (!Expect(TokenKind.CloseBrace, "Ожидался '}' для завершения блока"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            return new BlockStmtAst(openBraceToken.Line, openBraceToken.Column, statements);
        }
    }
}