using System.Windows.Forms;

namespace Demo
{
    public partial class Parser
    {
        // Парсит объявление переменной и возвращает узел AST
        // Поддерживает формы: "x: int;" и "let x: int;" с опциональной инициализацией
        private StmtAst ParseVariableDeclaration()
        {
            bool hasLet = false;
            Token letToken = null;

            // Проверяем наличие ключевого слова 'let'
            if (Match(TokenKind.Let))
            {
                hasLet = true;
                letToken = Peek();
                Advance(); // Съедаем 'let'
            }

            // Ожидаем идентификатор
            var idToken = Peek();
            if (!Expect(TokenKind.Identifier, "Ожидался идентификатор переменной"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            // Ожидаем двоеточие
            if (!Expect(TokenKind.Colon, "Ожидался ':' после идентификатора"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            // Ожидаем тип (int, string, float)
            var typeToken = Peek();
            if (!Match(TokenKind.Int, TokenKind.String, TokenKind.Float, TokenKind.Identifier))
            {
                Error("Ожидался тип (int, string или float)", typeToken);
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }
            var type = typeToken.Lexeme;
            Advance(); // Съедаем тип

            ExprAst initializer = null;
            StmtAst stmtAstInitializer = null;
            // Проверяем наличие инициализации
            if (Match(TokenKind.Assignment))
            {
                Advance(); // Съедаем '='
                if (!Match(TokenKind.New))
                {
                    initializer = ParseExpressionAst();
                    if (initializer == null)
                    {
                        SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                        return null;
                    }
                }
                else
                {
                    stmtAstInitializer = ParseNewExpression();
                    if (stmtAstInitializer == null)
                    {
                        SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                        return null;
                    }
                }
            }
            // Ожидаем точку с запятой
            if (!Expect(TokenKind.Semicolon, "Ожидался ';' после объявления переменной"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            // Создаём узел AST
            if (initializer != null)
                return new VarDeclExprAst(idToken.Line, idToken.Column, idToken.Lexeme, type, initializer);
            return new VarDeclStmtAst(idToken.Line, idToken.Column, idToken.Lexeme, type, stmtAstInitializer);
        }
    }
}