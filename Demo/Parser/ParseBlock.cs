using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public partial class Parser
    {
        private void ParseBlock()
        {
            if (!Expect(TokenKind.OpenBrace, "Ожидался '{' для начала блока"))
                return;

            while (!Match(TokenKind.CloseBrace) && !Match(TokenKind.EndOfFile))
            {
                ParseStatement();
            }

            Expect(TokenKind.CloseBrace, "Ожидался '}' для завершения блока");
            // TODO: здесь можно будет возвращать AST-узел BlockNode
        }

    }
}
