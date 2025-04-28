using System.Collections.Generic;
using System.Windows.Forms;

namespace Demo
{
    public partial class Parser
    {
        // Вспомогательный метод для парсинга параметров
        // Формат: (s1: string, s2: string) или ()
        private List<ParameterAst> ParseParameters()
        {
            var parameters = new List<ParameterAst>();

            if (!Expect(TokenKind.OpenParen, "Ожидалась '(' для начала списка параметров"))
            {
                return null;
            }

            if (!Match(TokenKind.CloseParen))
            {
                do
                {
                    //MessageBox.Show($"{Peek().Lexeme}");
                    // Ожидаем имя параметра
                    if (!Expect(TokenKind.Identifier, "Ожидалось имя параметра"))
                    {
                        return null;
                    }
                    var paramName = Peek(-1).Lexeme;

                    // Ожидаем тип параметра
                    if (!Expect(TokenKind.Colon, "Ожидалась ':' для указания типа параметра"))
                    {
                        SyncToBlockEnd();
                        return null;
                    }
                    if (!Match(TokenKind.Int, TokenKind.String, TokenKind.Float))
                    {
                        MessageBox.Show($"{Peek().Lexeme}");
                        Error("Требуется тип параметра");
                        SyncToBlockEnd();
                        return null;
                    }
                    var paramType = Peek().Lexeme;
                    parameters.Add(new ParameterAst(Peek().Line, Peek().Column, paramName, paramType));
                    Advance();
                } while (Expect(TokenKind.Comma));
            }

            if (!Expect(TokenKind.CloseParen, "Ожидалась ')' или ',' для списка параметров"))
            {
                SyncToBlockEnd();
                return null;
            }

            return parameters;
        }
    }
}
