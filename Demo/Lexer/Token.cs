using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{

    public enum TokenCategory
    {
        Type,
        Identifier,
        Keyword,
        Operator,
        OperatorComparison,
        OperatorBlock,
        Literal,
        Delimiter,
        Comment,
        Punctuation,
        Other
    }

    public enum TokenKind
    {
        // Типы данных
        Int,
        String,
        Float,
        // Идентификаторы (например: переменные, имена функций)
        Identifier,
        // Оператор присвоения
        Assignment, // =
        // Оператор сравнения
        Equal,          // ==
        NotEqual,       // !=
        Less,           // <
        LessEqual,      // <=
        Greater,        // >
        GreaterEqual,   // >=
        OrOr,           // ||
        AndAnd,         // &&
        // Оператор объявления типа
        Colon,  // :
        // Комментарии
        DoubleSlash,
        // Строки
        StringText,
        DoubleQuotes,  // "
        // Числа
        NumberInt,
        NumberFloat,
        // Операторы арифметики
        Plus,       // +
        Minus,      // -
        Asterisk,   // *
        Slash,      // /
        Percent,    // %
        Not,        // !

        // Скобки и разделители
        OpenParen,          // (
        CloseParen,         // )
        Semicolon,          // ;
        Comma,              // ,
        // Блок кода
        OpenBrace,      // {
        CloseBrace,     // }
        // Ключевые слова языка
        If,
        Else,
        While,
        For,
        Fn,         // Метод
        Return,
        Class,      // Определение класса
        Extends,    // Наследование базового класса
        New,        // Для создания объекта или инициализации его полей
        Virt,       // Объявление виртуального метода
        Impl,       // Реализация виртуального метода
        Let,        // Объявление перемемнной
        Del,        // Удаление переменной
        Print,
        // Конец входного потока
        EndOfFile,
        // Недопустимый (неизвестный) символ
        Invalid
    }

    public class Token
    {
        public TokenKind Kind { get; }
        public string Lexeme { get; }
        public int Position { get; }
        public int Line { get; }
        public int Column { get; }
        public Token(TokenKind kind, string lexeme, int pos, int line, int column)
        {
            Kind = kind;
            Lexeme = lexeme;
            Position = pos;
            Line = line;
            Column = column;
        }

        public TokenCategory Category => TokenInfo.GetCategory(Kind);
    }

    public static class TokenInfo
    {

        private static readonly Dictionary<TokenKind, TokenCategory> _categories = new Dictionary<TokenKind, TokenCategory>
        {
            // Типы
            { TokenKind.Int, TokenCategory.Type },
            { TokenKind.String, TokenCategory.Type },
            { TokenKind.Float, TokenCategory.Type },

            // Литералы
            { TokenKind.NumberInt, TokenCategory.Literal },
            { TokenKind.NumberFloat, TokenCategory.Literal },
            { TokenKind.StringText, TokenCategory.Literal },

            // Операторы
            { TokenKind.Assignment, TokenCategory.Operator },
            { TokenKind.Plus, TokenCategory.Operator },
            { TokenKind.Minus, TokenCategory.Operator },
            { TokenKind.Asterisk, TokenCategory.Operator },
            { TokenKind.Slash, TokenCategory.Operator },

            // Операторы сравнения
            { TokenKind.Equal, TokenCategory.OperatorComparison },
            { TokenKind.NotEqual, TokenCategory.OperatorComparison },
            { TokenKind.Less, TokenCategory.OperatorComparison },
            { TokenKind.LessEqual, TokenCategory.OperatorComparison },
            { TokenKind.Greater, TokenCategory.OperatorComparison },
            { TokenKind.GreaterEqual, TokenCategory.OperatorComparison },
            { TokenKind.OrOr, TokenCategory.OperatorComparison },
            { TokenKind.AndAnd, TokenCategory.OperatorComparison },

            // Разделители и пунктуация
            { TokenKind.Colon, TokenCategory.Delimiter },
            { TokenKind.Comma, TokenCategory.Delimiter },
            { TokenKind.Semicolon, TokenCategory.Delimiter },
            { TokenKind.OpenParen, TokenCategory.Punctuation },
            { TokenKind.CloseParen, TokenCategory.Punctuation },
            { TokenKind.OpenBrace, TokenCategory.OperatorBlock },
            { TokenKind.CloseBrace, TokenCategory.OperatorBlock },
            { TokenKind.DoubleQuotes, TokenCategory.Punctuation },

            // Идентификаторы
            { TokenKind.Identifier, TokenCategory.Identifier },

            // Комментарии
            { TokenKind.DoubleSlash, TokenCategory.Comment },

            // Ключевые слова
            { TokenKind.If, TokenCategory.Keyword },
            { TokenKind.Else, TokenCategory.Keyword },
            { TokenKind.While, TokenCategory.Keyword },
            { TokenKind.For, TokenCategory.Keyword },
            { TokenKind.Fn, TokenCategory.Keyword },
            { TokenKind.Return, TokenCategory.Keyword },
            { TokenKind.Class, TokenCategory.Keyword },
            { TokenKind.Extends, TokenCategory.Keyword },
            { TokenKind.New, TokenCategory.Keyword },
            { TokenKind.Virt, TokenCategory.Keyword },
            { TokenKind.Impl, TokenCategory.Keyword },
            { TokenKind.Let, TokenCategory.Keyword },
            { TokenKind.Del, TokenCategory.Keyword },
            { TokenKind.Print, TokenCategory.Keyword },

            // Остальное
            { TokenKind.EndOfFile, TokenCategory.Other },
            { TokenKind.Invalid, TokenCategory.Other },
        };

        public static TokenCategory GetCategory(TokenKind kind)
        {
            return _categories.TryGetValue(kind, out var cat) ? cat : TokenCategory.Other;
        }
    }



}
