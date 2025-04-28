using System.Collections.Generic;

namespace Demo
{
    // Информация о классе
    public class ClassInfo
    {
        public string Name { get; }
        public string BaseClass { get; } // null, если нет базового класса
        public int Line { get; }
        public int Column { get; }
        public Dictionary<string, string> Fields { get; } // Поля класса (имя -> тип)
        public List<ConstructorInfo> Constructors { get; } // Список конструкторов
        public Dictionary<string, MethodInfo> Methods { get; } // Методы (имя -> MethodInfo)

        public ClassInfo(string name, string baseClass, int line, int column)
        {
            Name = name;
            BaseClass = baseClass;
            Line = line;
            Column = column;
            Fields = new Dictionary<string, string>();
            Constructors = new List<ConstructorInfo>();
            Methods = new Dictionary<string, MethodInfo>();
        }
    }
}
