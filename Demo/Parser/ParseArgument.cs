using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public partial class Parser
    {
        private void ParseArgument()
        {
            if (Match(TokenKind.CloseParen))
            {
                Advance(); // пустой список
            }
            else
            {
                ParseExpression();

                while (true)
                {
                    if (Match(TokenKind.Comma))
                    {
                        Advance();
                        ParseExpression();
                    }
                    else if (Match(TokenKind.CloseParen))
                    {
                        break;
                    }
                    else
                    {
                        Error("Ожидалась ',' или ')'");
                        // Можно попытаться синхронизироваться до следующего возможного аргумента
                        SynchronizeTo(TokenKind.Comma, TokenKind.CloseParen, TokenKind.Semicolon);
                        if (Match(TokenKind.Comma))
                        {
                            Advance(); // пропускаем запятую и продолжаем
                            ParseExpression();
                            continue;
                        }
                        else if (Match(TokenKind.CloseParen))
                        {
                            break;
                        }
                        else
                        {
                            // если ни , ни ) — выходим
                            return;
                        }
                    }
                }
                if (!Expect(TokenKind.CloseParen, "Ожидалась ')'"))
                {
                    SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
                    return;
                }
            }
        }
    }
}
