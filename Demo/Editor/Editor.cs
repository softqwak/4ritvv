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

            // Подсветка синтаксиса
            scintilla.TextChanged += (s, e) => HighlightSiMPLESyntax((Scintilla)s);

            // Создание индикаторов
            scintilla.Indicators[0].Style = IndicatorStyle.StraightBox;
            scintilla.Indicators[0].ForeColor = Color.FromArgb(255, 0, 0); // светло-красный
            scintilla.Indicators[0].Under = true; // чтобы индикатор не мешал тексту

            scintilla.Indicators[1].Style = IndicatorStyle.StraightBox;
            scintilla.Indicators[1].ForeColor = Color.DarkGoldenrod;
            scintilla.Indicators[1].Under = true;

            // Всегда гарантируем хотя бы одну пустую строку внизу
            AddEmptyLineIfNeeded(scintilla);

            // Подписываемся на UpdateUI — он срабатывает и при изменении позиции курсора
            scintilla.UpdateUI += (s, e) => EnsureEmptyLine(scintilla);
        }

        private static void AddEmptyLineIfNeeded(Scintilla sci)
        {
            var txt = sci.Text;
            if (txt.Length == 0 || txt[txt.Length - 1] != '\n')
                sci.AppendText("\n");
        }

        private static void EnsureEmptyLine(Scintilla sci)
        {
            // 1) Сохраняем где был курсор
            int oldPos = sci.CurrentPosition;

            // 2) Вычисляем номер последней строки и номер строки курсора
            int lastLine = sci.Lines.Count - 1;
            int curLine = sci.LineFromPosition(oldPos);

            // 3) Если курсор на последней (гарантированно пустой) строке — добавляем ещё одну
            if (curLine >= lastLine)
            {
                // позиция конца последней линии
                int insertPos = sci.Lines[lastLine].Position + sci.Lines[lastLine].Length;
                // вставляем символ новой строки
                sci.InsertText(insertPos, "\n");
                // возвращаем курсор на своё старое место
                sci.CurrentPosition = oldPos;
                // опционально: чтобы маркер каретки тоже поставился корректно
                sci.ScrollCaret();
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
