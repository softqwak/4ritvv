using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public partial class Parser
    {
        // Парсит оператор print и возвращает узел AST
        // Возвращает PrintStmtAst или null в случае ошибки
        private StmtAst ParsePrint()
        {
            // Сохраняем позицию текущего токена ('print') для узла AST
            var printToken = Peek();
            Advance(); // Съедаем 'print'

            // Проверяем открывающую скобку
            if (!Expect(TokenKind.OpenParen, "Ожидалась '(' после 'print'"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.CloseBrace, TokenKind.EndOfFile);
                return null;
            }

            // Парсим список аргументов
            var arguments = ParseArgumentList();

            // Закрывающая скобка уже обработана в ParseArgumentList

            // Проверяем точку с запятой
            if (!Expect(TokenKind.Semicolon, "Ожидался символ ';'"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.CloseBrace, TokenKind.EndOfFile);
                return null;
            }

            // Создаём узел AST для print
            return new PrintStmtAst(printToken.Line, printToken.Column, arguments);
        }

    }
}
