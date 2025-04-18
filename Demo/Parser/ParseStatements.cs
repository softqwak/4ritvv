using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public partial class Parser
    {
        private void ParseStatement()
        {
            var token = Peek().Kind;
            var tokenNext = Peek(1).Kind;
            if (Match(TokenKind.Let))
            {
                ParseVariableDeclaration();
            }
            else if (token == TokenKind.Identifier && tokenNext == TokenKind.Colon)
            {
                // Объявление без 'let' (например, при глобальном определении)
                ParseVariableDeclaration();
            }
            else if (tokenNext == TokenKind.Assignment)
            {
                ParseAssignment();
            }
            else if (token == TokenKind.Print)
            {
                ParsePrint();
            }
            else if (token == TokenKind.If)
            {
                ParseIf();
            }
            else
            {
                Error($"Синтаксическая ошибка - '{Peek().Lexeme}' неизвестная конструкция");
                SynchronizeTo(TokenKind.Semicolon, TokenKind.CloseBrace, TokenKind.Identifier);
                Advance();
            }
        }
    }
}
