
using System.Collections.Generic;

namespace Demo
{
    public partial class Parser
    {
        // Парсит выражение доступа к полю (например, obj.field) и возвращает узел ExprAst
        private ExprAst ParseMemberAccessExpr()
        {
            // Сохраняем токен идентификатора (например, obj)
            var receiverToken = Peek();
            if (!Expect(TokenKind.Identifier, "Ожидался идентификатор объекта"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            // Создаём узел для идентификатора объекта
            var receiver = new IdentifierExprAst(receiverToken.Line, receiverToken.Column, receiverToken.Lexeme);

            // Ожидаем точку
            if (!Expect(TokenKind.Period, "Ожидалась '.' после идентификатора"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            // Ожидаем имя поля
            var memberToken = Peek();
            if (!Expect(TokenKind.Identifier, "Ожидалось имя поля после '.'"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            // Возвращаем узел выражения доступа к полю
            return new MemberAccessExprAst(receiverToken.Line, receiverToken.Column, receiver, memberToken.Lexeme);
        }


        // Парсит вызов метода (например, obj.method()) и возвращает узел StmtAst
        private StmtAst ParseMemberAccessStmt()
        {
            // Сохраняем токен идентификатора (например, obj)
            var receiverToken = Peek();
            if (!Expect(TokenKind.Identifier, "Ожидался идентификатор объекта"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            // Создаём узел для идентификатора объекта
            var receiver = new IdentifierExprAst(receiverToken.Line, receiverToken.Column, receiverToken.Lexeme);

            // Ожидаем точку
            if (!Expect(TokenKind.Period, "Ожидалась '.' после идентификатора"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            // Ожидаем имя метода
            var methodToken = Peek();
            if (!Expect(TokenKind.Identifier, "Ожидалось имя метода после '.'"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            // Ожидаем открывающую скобку
            if (!Expect(TokenKind.OpenParen, "Ожидалась '(' для вызова метода"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            // Парсим аргументы
            var arguments = new List<ExprAst>();
            if (!Match(TokenKind.CloseParen))
            {
                arguments = ParseArgumentList();
                if (arguments == null)
                {
                    SynchronizeTo(TokenKind.CloseParen, TokenKind.Semicolon, TokenKind.EndOfFile);
                    return null;
                }
            }

            // Ожидаем точку с запятой
            if (!Expect(TokenKind.Semicolon, "Ожидался ';' после вызова метода"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            // Возвращаем узел вызова метода
            return new CallStmt(receiverToken.Line, receiverToken.Column, receiver, methodToken.Lexeme, arguments);
        }

    }
}
