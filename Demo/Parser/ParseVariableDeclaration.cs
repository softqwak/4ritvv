using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public partial class Parser
    {
        private string variableName = "";
        private void ParseVariableDeclaration()
        {
            Expect(TokenKind.Let); // обязательно съедаем 'let' при наличии

            if (!ExpectIdentifier()) return;

            ParseOptionalTypeAnnotation();
            ParseOptionalInitializer();

            if (!Expect(TokenKind.Semicolon))
            {
                Error("Ожидался символ ';' в конце объявления переменной");
                //SynchronizeTo(TokenKind.Semicolon);
            }

            // TODO: добавить узел в AST
        }

        private bool ExpectIdentifier()
        {
            var token = Peek();
            if (token.Kind == TokenKind.Invalid)
            {
                Error("Некорректная переменная", token);
                SynchronizeTo(TokenKind.Semicolon);
                return false;
            }

            if (!Expect(TokenKind.Identifier, "Ожидался идентификатор переменной"))
            {
                SynchronizeTo(TokenKind.Semicolon);
                return false;
            }
            variableName = token.Lexeme;
            return true;
        }

        private void ParseOptionalTypeAnnotation()
        {
            if (Match(TokenKind.Colon))
            {
                Advance(); // съедаем ':'

                if (!(Expect(TokenKind.Int) || Expect(TokenKind.String) || Expect(TokenKind.Float)))
                {
                    Error("Ожидался тип после ':'");
                    SynchronizeTo(TokenKind.Semicolon);
                }
            }
        }

        private void ParseOptionalInitializer()
        {
            if (Match(TokenKind.Assignment))
            {
                Advance(); // пропускаем =
                ParseExpression();
            }
            else
            {

                Warning($"Переменная '{variableName}' объявлена, но не инициализирована");
            }
        }

    }
}
