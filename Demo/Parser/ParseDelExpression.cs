using System;

namespace Demo
{
    public partial class Parser
    {
        private StmtAst ParseDelExpression() 
        {
            Advance();
            var variable = Peek();
            Advance();
            if (Match(TokenKind.Period)) 
            {
                Error("Некорректный идентификатор");
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile, TokenKind.CloseBrace);
                Advance();
                return null;
            }
            if (!Expect(TokenKind.Semicolon, "Требуется ';'"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile, TokenKind.CloseBrace);
                return null;
            }
            return (new DeleteStmt(variable.Line, variable.Column, variable.Lexeme));
        }
    }
}
