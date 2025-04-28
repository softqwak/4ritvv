using System.Collections.Generic;

namespace Demo.Analyzer
{
    // Информация о функции
    public class FunctionInfo
    {
        public string Name { get; }
        public string ReturnType { get; }
        public List<(string Name, string Type)> Parameters { get; }
        public int Line { get; }
        public int Column { get; }

        public FunctionInfo(string name, string returnType, List<(string, string)> parameters, int line, int column)
        {
            Name = name;
            ReturnType = returnType;
            Parameters = parameters;
            Line = line;
            Column = column;
        }
    }
}
