using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class Parser
    {
        private List<Token> _tokens = new List<Token>();
        public List<string> lexems = new List<string>();

        private int _position;
        private int _line;
        private int _column;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _position = 0;
            _line = 0;
            _column = 0;
        }


        public void Parse()
        {
            Diagnostics._messagesError.Clear();
            Diagnostics._messagesWarning.Clear();
            Diagnostics._markWarnings.Clear();
            Diagnostics._markErrors.Clear();
            ParseProgram();
        }

        private void ParseProgram()
        {
            while (!Match(TokenKind.EndOfFile))
            {
                ParseTopLevel();
            }
        }

        private void ParseTopLevel()
        {
            // В будущем здесь будет обработка функций, классов, и т.д.
            // Пока обрабатываем только операторы верхнего уровня
            ParseStatement();
        }   

    }


}
