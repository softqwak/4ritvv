using System;
using System.Windows.Forms;

namespace Demo
{
    public partial class Parser
    {
        // Парсит цикл for и возвращает узел AST
        // Формат: for let i: int = 0 ; i < 10 ; ++i { }
        private StmtAst ParseFor()
        {
            var forToken = Peek();
            Advance(); // Съедаем 'for'

            // Отладочный вывод текущего токена
            Console.WriteLine($"After 'for': Token = {Peek().Kind}, Lexeme = {Peek().Lexeme}, Line = {Peek().Line}, Col = {Peek().Column}");

            // Парсим инициализацию (например, let i: int = 0)
            StmtAst initializer = null;
            if (Match(TokenKind.Let) || Match(TokenKind.Identifier))
            {
                initializer = ParseVariableDeclaration();
                if (initializer == null)
                {
                    SynchronizeTo(TokenKind.Semicolon, TokenKind.OpenBrace, TokenKind.EndOfFile);
                    return null;
                }
            }

            // Отладочный вывод до условия
            MessageBox.Show($"before if: Token = {Peek().Kind}, Lexeme = {Peek().Lexeme}, Line = {Peek().Line}, Col = {Peek().Column}");

            // Парсим условие (например, i < 10)
            ExprAst condition = null;
            if (!Match(TokenKind.Semicolon))
            {
                condition = ParseComparisonAst();
                if (condition == null)
                {
                    SynchronizeTo(TokenKind.Semicolon, TokenKind.OpenBrace, TokenKind.EndOfFile);
                    return null;
                }
            }

            // Отладочный вывод перед проверкой ';'
            MessageBox.Show($"Before condition semicolon: Token = {Peek().Kind}, Lexeme = {Peek().Lexeme}, Line = {Peek().Line}, Col = {Peek().Column}");

            // Проверяем точку с запятой после условия
            if (!Expect(TokenKind.Semicolon, "Ожидался ';' после условия"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.OpenBrace, TokenKind.EndOfFile);
                return null;
            }

            // Отладочный вывод после условия
            Console.WriteLine($"After condition semicolon: Token = {Peek().Kind}, Lexeme = {Peek().Lexeme}, Line = {Peek().Line}, Col = {Peek().Column}");

            // Парсим итерацию (например, ++i)
            ExprAst increment = null;
            if (!Match(TokenKind.OpenBrace))
            {
                increment = ParseExpressionAst();
                if (increment == null)
                {
                    SynchronizeTo(TokenKind.OpenBrace, TokenKind.EndOfFile);
                    return null;
                }
            }

            // Отладочный вывод перед телом
            Console.WriteLine($"Before body: Token = {Peek().Kind}, Lexeme = {Peek().Lexeme}, Line = {Peek().Line}, Col = {Peek().Column}");

            // Парсим тело цикла
            var body = ParseBlock();
            if (body == null)
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            // Создаём узел AST
            return new ForStmtAst(forToken.Line, forToken.Column, initializer, condition, increment, body);
        }
        
    }
}