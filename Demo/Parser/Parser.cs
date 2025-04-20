using System.Collections.Generic;

namespace Demo
{
    public partial class Parser
    {
        private List<Token> _tokens = new List<Token>();
        public List<string> lexems = new List<string>();
        private int _position;
        private int _line;
        private int _column;
        private List<StmtAst> _programAst; // Список узлов AST для программы

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _position = 0;
            _line = 0;
            _column = 0;
            _programAst = new List<StmtAst>();
        }

        public List<StmtAst> Parse()
        {
            Diagnostics._messagesError.Clear();
            Diagnostics._messagesWarning.Clear();
            Diagnostics._markWarnings.Clear();
            Diagnostics._markErrors.Clear();
            _programAst.Clear();
            ParseProgram();
            return _programAst;
        }

        private void ParseProgram()
        {
            while (!Match(TokenKind.EndOfFile))
            {
                var stmt = ParseTopLevel();
                if (stmt != null)
                {
                    _programAst.Add(stmt);
                }
            }
        }

        private StmtAst ParseTopLevel()
        {
            // В будущем здесь будет обработка функций, классов и т.д.
            // Пока обрабатываем только операторы верхнего уровня
            return ParseStatement();
        }
    }
}