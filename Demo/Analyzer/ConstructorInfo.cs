using System.Collections.Generic;

namespace Demo
{
    // Информация о конструкторе
    public class ConstructorInfo
    {
        public List<(string Name, string Type)> Parameters { get; }
        public int Line { get; }
        public int Column { get; }

        public ConstructorInfo(List<(string, string)> parameters, int line, int column)
        {
            Parameters = parameters;
            Line = line;
            Column = column;
        }
    }
}
