using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public partial class Parser
    {
        private void ParseAssignment()
        {
            Advance();

            if (!Expect(TokenKind.Assignment, "Ожидался оператор '=' после идентификатора"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                return;
            }

            ParseExpression(); // Пока просто литералы

            Expect(TokenKind.Semicolon, "Ожидался ';' после выражения");
            // TODO: добавить в AST узел Assignment
        }
    }
}
