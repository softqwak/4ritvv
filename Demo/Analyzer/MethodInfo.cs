using System.Collections.Generic;

namespace Demo
{
    // Информация о методе (виртуальном или реализованном)
    public class MethodInfo
    {
        public string Name { get; }
        public string ReturnType { get; }
        public List<(string Name, string Type)> Parameters { get; }
        public bool IsVirtual { get; }
        public int Line { get; }
        public int Column { get; }

        public MethodInfo(string name, string returnType, List<(string, string)> parameters, bool isVirtual, int line, int column)
        {
            Name = name;
            ReturnType = returnType;
            Parameters = parameters;
            IsVirtual = isVirtual;
            Line = line;
            Column = column;
        }
    }
}
