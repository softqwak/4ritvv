#region Using Directives

using System;
using ScintillaNET;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Data.Common;

#endregion Using Directives

namespace Demo
{
    public partial class TestForm : Form
    {
        public Diagnostics diagnostics = new Diagnostics();

        public TestForm()
        {
            InitializeComponent();

            // Настройка Scintilla
            Editor.SetupEditor(scintilla1);
            Editor.SetupEditor(scintilla2);

            // Переключение табов
            tabConsol.SelectedIndexChanged += Form_TabChanged;

            // Настройка транслятора
            Translator translator = new Translator();



        }


        private void Form_TabChanged(object sender, EventArgs e)
        {
            switch (tabConsol.SelectedIndex)
            {
                case 0:
                    scintilla1.Focus();
                    break;
                case 1:
                    scintilla2.Focus();
                    break;
            }
        }

        private void TestForm_Load(object sender, EventArgs e) { }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Scintilla activeEditor = null;
            if (tabExs.SelectedTab == tabEx1)
                activeEditor = scintilla1;
            else if (tabExs.SelectedTab == tabEx2)
                activeEditor = scintilla2;

            // Сначала очищаем предыдущие индикаторы этого типа (по всей длине, если надо)
            ClearAllIndicators(activeEditor);

            string code = activeEditor.Text;
            SimpleLexer lexer = new SimpleLexer(code);
            List<Token> tokens = lexer.Tokenize();
            Parser parser = new Parser(tokens);
            var ast = parser.Parse();

            //rtbxAnalysis.Clear();
            //for (int i = 0; i < tokens.Count; i++)
            //{
            //    rtbxAnalysis.AppendText(tokens[i].Lexeme + "\r\n");
            //}

            //for (int i = 0; i < parser.lexems.Count; i++)
            //{
            //    rtbxAnalysis.AppendText(parser.lexems[i] + "\r\n");
            //}

            // Вывод AST в RichTextBox
            rtbxAnalysis.Clear();
            foreach (var stmt in ast)
            {
                rtbxAnalysis.AppendText(stmt.ToString(0) + "\n");
            }

            rtbxOutput.Clear();
            for (int i = 0; i < Diagnostics._messagesError.Count; i++)
            {
                rtbxOutput.SelectionColor = Color.DarkRed;
                rtbxOutput.AppendText(Diagnostics._messagesError[i] + "\r\n");
            }
            activeEditor.IndicatorCurrent = 0;

            foreach (var tmp in Diagnostics._markErrors)
            {
                int pos = activeEditor.Lines[tmp.Item1 - 1].Position;
                int posEnd = activeEditor.Lines[tmp.Item1].Position;

                // Применяем индикатор на один символ
                activeEditor.IndicatorCurrent = 0;
                activeEditor.IndicatorFillRange(pos, posEnd - pos); // 1 символ
            }


            for (int i = 0; i < Diagnostics._messagesWarning.Count; i++)
            {
                rtbxOutput.SelectionColor = Color.DarkGoldenrod;
                rtbxOutput.AppendText(Diagnostics._messagesWarning[i] + "\r\n");
            }
            activeEditor.IndicatorCurrent = 1;

            foreach (var tmp in Diagnostics._markWarnings)
            {
                int pos = activeEditor.Lines[tmp.Item1 - 1].Position;
                int posEnd = activeEditor.Lines[tmp.Item1].Position;
                // Применяем индикатор на один символ
                activeEditor.IndicatorCurrent = 1;
                activeEditor.IndicatorFillRange(pos, posEnd - pos); // 1 символ
            }
        }

        private void TestForm_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void scintilla1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                toolStripButton1_Click(sender, e);
            }
        }

        private void rtbxOutput_TextChanged(object sender, EventArgs e)
        {

        }

        void ClearAllIndicators(Scintilla editor)
        {
            for (int i = 0; i <= 1; i++) // если используешь только 0 и 1
            {
                editor.IndicatorCurrent = i;
                editor.IndicatorClearRange(0, editor.TextLength);
            }
        }
    }
}
