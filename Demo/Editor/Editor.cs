using ScintillaNET;
using System;
using System.Drawing;
using System.Text.RegularExpressions;


namespace Demo
{
    internal class Editor
    {
        public static void SetupEditor(Scintilla scintilla)
        {
            // Шрифт и базовые стили
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 10;
            scintilla.StyleClearAll();
            scintilla.Lexer = Lexer.Null;

            // Стили
            scintilla.Styles[Style.Default].ForeColor = Color.Black;
            scintilla.Styles[1].ForeColor = Color.Green;  // Комментарии
            scintilla.Styles[1].Italic = true;
            scintilla.Styles[2].ForeColor = Color.Blue;   // Ключевые слова
            scintilla.Styles[3].ForeColor = Color.ForestGreen;

            // Подсветка текущей строки
            scintilla.CaretLineVisible = true;
            scintilla.CaretLineBackColor = Color.FromArgb(240, 240, 255);

            // Нумерация строк
            scintilla.Margins[0].Type = MarginType.Number;
            scintilla.Margins[0].Width = 40;

            // Подсветка скобок
            scintilla.IndentationGuides = IndentView.LookBoth;
            scintilla.UpdateUI += (s, e) => HighlightBraces(scintilla);

            // Подсветка синтаксиса
            scintilla.TextChanged += (s, e) => HighlightSiMPLESyntax((Scintilla)s);

            HighlightSiMPLESyntax(scintilla); // первая подсветка
        }

        public static void HighlightBraces(Scintilla editor)
        {
            int pos = editor.CurrentPosition;
            char curChar = (char)editor.GetCharAt(pos);
            int bracePos1 = -1;

            if ("()[]{}".IndexOf(curChar) >= 0)
                bracePos1 = pos;
            else if (pos > 0)
            {
                char prevChar = (char)editor.GetCharAt(pos - 1);
                if ("()[]{}".IndexOf(prevChar) >= 0)
                    bracePos1 = pos - 1;
            }

            if (bracePos1 >= 0)
            {
                int bracePos2 = editor.BraceMatch(bracePos1);
                if (bracePos2 != Scintilla.InvalidPosition)
                {
                    editor.BraceHighlight(bracePos1, bracePos2);
                }
                else
                {
                    editor.BraceBadLight(bracePos1);
                }
            }
            else
            {
                editor.BraceHighlight(Scintilla.InvalidPosition, Scintilla.InvalidPosition);
            }
        }

        public static void HighlightSiMPLESyntax(Scintilla editor)
        {
            string text = editor.Text;

            // Сброс всех стилей
            editor.StartStyling(0);
            editor.SetStyling(text.Length, Style.Default);

            // Подсветка ключевых слов
            foreach (string keyword in simple.keywords)
            {
                string pattern = $@"\b{Regex.Escape(keyword)}\b";
                foreach (Match match in Regex.Matches(text, pattern, RegexOptions.IgnoreCase))
                {
                    editor.StartStyling(match.Index);
                    editor.SetStyling(match.Length, 2);
                }
            }

            // Подсветка строк
            foreach (Match match in Regex.Matches(text, "\"(?:\\\\.|[^\"\\\\])*\""))
            {
                editor.StartStyling(match.Index);
                editor.SetStyling(match.Length, 3);
            }

            // Подсветка комментариев
            for (int i = 0; i < editor.Lines.Count; i++)
            {
                var line = editor.Lines[i];
                string fullText = line.Text;

                int commentIndex = fullText.IndexOf("//");

                if (commentIndex >= 0)
                {
                    int commentStart = line.Position + commentIndex;
                    int commentLength = fullText.Length - commentIndex;

                    editor.StartStyling(commentStart);
                    editor.SetStyling(commentLength, 1); // Стиль 1 — комментарий
                }
            }
        }

    }
}
