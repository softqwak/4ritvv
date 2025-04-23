using System.Windows.Forms;

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
            ExprAst value = null;
            StmtAst stmt = null;
            if (!Match(TokenKind.New))
            {
                value = ParseExpressionAst();
                if (value == null)
                {
                    SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                    return null;
                }
            }
            else
            {
                stmt = ParseNewExpression();
                if (stmt == null)
                {
                    SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                    return null;
                }
                Advance();
            }

            if (!Expect(TokenKind.Semicolon, "Ожидался ';' после выражения"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            if (value != null)
            {
                return new AssignExprAst(idToken.Line, idToken.Column, idToken.Lexeme, value);
            }
            return new AssignStmtAst(idToken.Line, idToken.Column, idToken.Lexeme, stmt);
        }
    }
}