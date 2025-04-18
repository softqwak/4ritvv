using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public partial class Parser
    {
        // Парсинг общего выражения (ариметическое или логическое)
        private void ParseExpression()
        {
            ParseTerm();
            while (Match(TokenKind.Plus, TokenKind.Minus))
            {
                Advance(); // + или -
                ParseTerm();
            }
        }

        // Парсинг термов (умножение, деление, остаток)
        private void ParseTerm()
        {
            ParseFactor();
            while (Match(TokenKind.Asterisk, TokenKind.Slash, TokenKind.Percent))
            {
                Advance(); // * / %
                ParseFactor();
            }
        }

        // Парсинг факторов (например, скобки или переменные)
        private void ParseFactor()
        {
            if (Match(TokenKind.OpenParen))
            {
                Advance(); // (
                ParseExpression(); // рекурсивно обрабатываем выражение
                Expect(TokenKind.CloseParen, "Ожидалась ')'");
            }
            else
            {
                ParsePrimary();
            }

        }

        // Основные элементы выражений (числа, строки, идентификаторы)
        private void ParsePrimary()
        {
            if (Match(TokenKind.NumberInt, TokenKind.NumberFloat, TokenKind.StringText, TokenKind.Identifier))
            {
                Advance(); // число, строка, переменная
            }
            else
            {
                Error("Синтаксическая ошибка - ожидалось значение или выражение");
                Advance();
            }
        }

    }
}
