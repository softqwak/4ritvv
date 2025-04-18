using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public partial class Parser
    {
        private void ParseIf()
        {
            Advance(); // пропустить 'if'

            if (Match(TokenKind.OpenParen))
            {
                Advance(); // (
                ParseCondition(); // обрабатываем условие
                Expect(TokenKind.CloseParen, "Ожидалась закрывающая скобка ')'");
            }
            else
            {
                // Если нет скобок, сразу парсим условие
                ParseCondition();
            }

            ParseBlock(); // ожидаем { ... }

            if (Match(TokenKind.Else))
            {
                Advance(); // else
                ParseBlock();
            }
        }

        // Разбор условия (логическое выражение)
        private void ParseCondition()
        {
            ParseLogicalOr(); // начинаем с логического ИЛИ
        }

        // Логическое ИЛИ (или операторы)
        private void ParseLogicalOr()
        {
            ParseLogicalAnd(); // обрабатываем логическое И сначала
            while (Match(TokenKind.OrOr))
            {
                Advance(); // если нашли '||'
                ParseLogicalAnd(); // снова обрабатываем логическое И
            }
        }

        // Логическое И (и операторы)
        private void ParseLogicalAnd()
        {
            ParseComparison(); // начинаем с операторов сравнения
            while (Match(TokenKind.AndAnd))
            {
                Advance(); // если нашли '&&'
                ParseComparison(); // снова проверяем операторы сравнения
            }
        }

        // Сравнение (например, ==, !=, >, <)
        private void ParseComparison()
        {
            ParseExpression(); // здесь используется ваш парсер выражений (сложение, вычитание и т.д.)

            while (Match(TokenKind.Equal, TokenKind.NotEqual,
                         TokenKind.Less, TokenKind.LessEqual,
                         TokenKind.Greater, TokenKind.GreaterEqual))
            {
                Advance(); // операторы сравнения
                ParseExpression(); // снова обрабатываем выражение
            }
        }

    }
}
