using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{

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
        Equal,  // ==

        // Оператор объявления типа
        Colon,  // :

        // Комментарии
        DoubleSlash,

        // Строки
        StringText,
        DoubleQuotes,  // "

        // Операторы арифметики
        Plus,       // +
        Minus,      // -
        Asterisk,   // *
        Slash,      // /

        // Скобки и разделители
        OpenParen,          // (
        CloseParen,         // )
        Semicolon,          // ;
        Comma,              // ,
        ExclamationMark,    // !

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
    }

}
