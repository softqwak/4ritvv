

namespace Demo.Analyzer
{
    // Информация о переменной
    public class VariableInfo
    {
        public string Name { get; } // Имя переменной
        public string Type { get; } // Тип переменной
        public int Line { get; }   // Строка объявления
        public int Column { get; } // Столбец объявления

        public VariableInfo(string name, string type, int line, int column)
        {
            Name = name;
            Type = type;
            Line = line;
            Column = column;
        }
    }
}
