
namespace Demo
{
    public partial class Parser
    {
        // Парсит объявление функции
        // Формат: fn main(): int { return 0; } или fn printLn(s1: string, s2: string) {}
        private StmtAst ParseFunctionDeclaration()
        {
            var fnToken = Peek(); // Съедено 'fn'
            Advance();
            // Ожидаем имя функции
            if (!Expect(TokenKind.Identifier, "Ожидалось имя функции"))
            {
                SynchronizeTo(TokenKind.OpenBrace, TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }
            var name = Peek(-1).Lexeme;

            // Парсим параметры
            var parameters = ParseParameters();
            if (parameters == null)
            {
                SynchronizeTo(TokenKind.OpenBrace, TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }
            // Парсим возвращаемый тип (если есть)
            string returnType = null;
            if (Match(TokenKind.Colon))
            {
                Advance(); // Съедаем ':'
                if (!Match(TokenKind.Int, TokenKind.String, TokenKind.Float, TokenKind.Identifier))
                {
                    Error("Ожидался тип возвращаемого значения");
                    SynchronizeTo(TokenKind.OpenBrace, TokenKind.Semicolon, TokenKind.EndOfFile);
                    return null;
                }
                returnType = Peek().Lexeme;
                Advance();
            }

            // Парсим тело
            var body = ParseBlock();
            if (body == null)
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            return new FunctionDeclStmt(fnToken.Line, fnToken.Column, name, returnType, parameters, body);
        }
    }
}
