using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public partial class Parser
    {
        private void ParsePrint()
        {
            Advance(); // съедаем 'print'
            if (!Expect(TokenKind.OpenParen, "Ожидалась '(' после 'print'"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.CloseBrace, TokenKind.EndOfFile);
                return;
            }
            ParseArgument();
            Expect(TokenKind.Semicolon, "Ожидался символ ';'");
        }

        
    }
}
