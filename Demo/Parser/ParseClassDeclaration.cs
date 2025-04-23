using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class Parser
    {
        // Парсит объявление класса
        // Формат: class Shape {} или class Square extends Shape {}
        private StmtAst ParseClassDeclaration()
        {
            var classToken = Peek(); // 'class'
            Advance();

            // Ожидаем имя класса
            if (!Expect(TokenKind.Identifier, "Ожидалось имя класса"))
            {
                SynchronizeTo(TokenKind.OpenBrace, TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }
            var name = Peek(-1).Lexeme;
            
            // Проверяем наследование
            string baseClass = null;
            if (Match(TokenKind.Extends))
            {
                Advance(); // Съедаем 'extends'
                if (!Expect(TokenKind.Identifier, "Ожидалось имя базового класса"))
                {
                    SynchronizeTo(TokenKind.OpenBrace, TokenKind.Semicolon, TokenKind.EndOfFile);
                    return null;
                }
                baseClass = Peek(-1).Lexeme;
            }

            // Ожидаем тело класса
            if (!Expect(TokenKind.OpenBrace, "Ожидался '{' для начала тела класса"))
            {
                SynchronizeTo(TokenKind.CloseBrace, TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            // Парсим члены класса (конструкторы, виртуальные методы, реализации)
            var members = new List<StmtAst>();
            while (!Match(TokenKind.CloseBrace, TokenKind.EndOfFile))
            {
                var member = ParseClassMember();
                if (member != null)
                {
                    members.Add(member);
                }
                else
                {
                    SynchronizeTo(TokenKind.CloseBrace, TokenKind.Semicolon, TokenKind.EndOfFile);
                }
            }

            // Съедаем '}'
            if (!Expect(TokenKind.CloseBrace, "Ожидался '}' для конца тела класса"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            return new ClassDeclStmt(classToken.Line, classToken.Column, name, baseClass, members);
        }

        // Парсит члены класса (конструкторы, виртуальные методы, реализации)
        private StmtAst ParseClassMember()
        {
            if (Match(TokenKind.New))
            {
                return ParseConstructorDeclaration();
            }
            if (Match(TokenKind.Virt))
            {
                return ParseVirtualMethodDeclaration();
            }
            if (Match(TokenKind.Impl))
            {
                return ParseImplMethodDeclaration();
            }
            if (Match(TokenKind.Fn))
            {
                return ParseFunctionDeclaration();
            }
            if (Match(TokenKind.Identifier) || Match(TokenKind.Let))
            {
                return ParseVariableDeclaration();
            }
            Error("Ожидался конструктор, виртуальный метод, реализация метода или функция", Peek());
            Advance();
            return null;
        }

        // Парсит конструктор
        // Формат: new(s: int) {} или new() {}
        private StmtAst ParseConstructorDeclaration()
        {
            var newToken = Peek(); // Съедено 'new'
            Advance();
            // Парсим параметры
            var parameters = ParseParameters();
            if (parameters == null)
            {
                SynchronizeTo(TokenKind.OpenBrace, TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            // Парсим тело
            var body = ParseBlock();
            if (body == null)
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            return new ConstructorDeclStmt(newToken.Line, newToken.Column, parameters, body);
        }

        // Парсит виртуальный метод
        // Формат: virt draw() {}
        private StmtAst ParseVirtualMethodDeclaration()
        {
            var virtToken = Peek(); // Съедено 'virt'
            Advance();
            // Ожидаем имя метода
            if (!Expect(TokenKind.Identifier, "Ожидалось имя виртуального метода"))
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
                if (!Match(TokenKind.Int, TokenKind.String, TokenKind.Float))
                {
                    Error("Требуется тип параметра");
                    SyncToBlockEnd();
                    return null;
                }
                returnType = Peek().Lexeme;
                Advance();
            }

            if (!Expect(TokenKind.Semicolon, "Требуется ';' в конце объявления виртуальной функции"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return null;
            }

            return new VirtualMethodDeclStmt(virtToken.Line, virtToken.Column, name, returnType, parameters);
        }

        // Парсит реализацию виртуального метода
        // Формат: impl draw() {}
        private StmtAst ParseImplMethodDeclaration()
        {
            var implToken = Peek(); // Съедено 'impl'
            Advance();
            // Ожидаем имя метода
            if (!Expect(TokenKind.Identifier, "Ожидалось имя метода для реализации"))
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
                if (!Match(TokenKind.Int, TokenKind.String, TokenKind.Float))
                {
                    Error("Требуется тип параметра");
                    SyncToBlockEnd();
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

            return new ImplMethodDeclStmt(implToken.Line, implToken.Column, name, returnType, parameters, body);
        }
    }
}
