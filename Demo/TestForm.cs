#region Using Directives

using System;
using ScintillaNET;
using System.Windows.Forms;
using System.Collections.Generic;

#endregion Using Directives

namespace Demo
{
    public partial class TestForm : Form
    {
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

            string code = activeEditor.Text;
            SimpleLexer lexer = new SimpleLexer(code);
            List<Token> tokens = lexer.Tokenize();
            tbxAnalysis.Text += tokens.Count;
            Parser parser = new Parser(tokens);
            parser.Parse();
        }
    }
}
