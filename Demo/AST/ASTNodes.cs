using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    // Базовый класс для всех узлов AST
    public abstract class AstNode
    {
        public int Line { get; }
        public int Column { get; }

        protected AstNode(int line, int column)
        {
            Line = line;
            Column = column;
        }

        // Метод для вывода узла в строковом формате
        public abstract string ToString(int indent);
    }

    // Базовый класс для выражений
    public abstract class ExprAst : AstNode
    {
        protected ExprAst(int line, int column) : base(line, column) { }
    }

    // Целочисленная константа
    public class IntExprAst : ExprAst
    {
        public int Value { get; }

        public IntExprAst(int line, int column, int value)
            : base(line, column)
        {
            Value = value;
        }

        public override string ToString(int indent)
        {
            return new string(' ', indent) + $"IntExpr({Value}) [Line: {Line}, Col: {Column}]";
        }
    }

    // Вещественная константа
    public class FloatExprAst : ExprAst
    {
        public double Value { get; }

        public FloatExprAst(int line, int column, double value)
            : base(line, column)
        {
            Value = value;
        }

        public override string ToString(int indent)
        {
            return new string(' ', indent) + $"FloatExpr({Value}) [Line: {Line}, Col: {Column}]";
        }
    }

    // Строковая константа
    public class StringExprAst : ExprAst
    {
        public string Value { get; }

        public StringExprAst(int line, int column, string value)
            : base(line, column)
        {
            Value = value;
        }

        public override string ToString(int indent)
        {
            return new string(' ', indent) + $"StringExpr(\"{Value}\") [Line: {Line}, Col: {Column}]";
        }
    }

    // Идентификатор (переменная)
    public class IdentifierExprAst : ExprAst
    {
        public string Name { get; }

        public IdentifierExprAst(int line, int column, string name)
            : base(line, column)
        {
            Name = name;
        }

        public override string ToString(int indent)
        {
            return new string(' ', indent) + $"Identifier({Name}) [Line: {Line}, Col: {Column}]";
        }
    }

    // Бинарная операция (например, a + b, x > y)
    public class BinaryExprAst : ExprAst
    {
        public string Operator { get; }
        public ExprAst Left { get; }
        public ExprAst Right { get; }

        public BinaryExprAst(int line, int column, string op, ExprAst left, ExprAst right)
            : base(line, column)
        {
            Operator = op;
            Left = left;
            Right = right;
        }

        public override string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine(new string(' ', indent) + $"BinaryExpr({Operator}) [Line: {Line}, Col: {Column}]");
            sb.AppendLine(Left.ToString(indent + 2));
            sb.AppendLine(Right.ToString(indent + 2));
            return sb.ToString();
        }
    }

    // Логическая операция (&&, ||)
    public class LogicalExprAst : ExprAst
    {
        public string Operator { get; }
        public ExprAst Left { get; }
        public ExprAst Right { get; }

        public LogicalExprAst(int line, int column, string op, ExprAst left, ExprAst right)
            : base(line, column)
        {
            Operator = op;
            Left = left;
            Right = right;
        }

        public override string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine(new string(' ', indent) + $"LogicalExpr({Operator}) [Line: {Line}, Col: {Column}]");
            sb.AppendLine(Left.ToString(indent + 2));
            sb.AppendLine(Right.ToString(indent + 2));
            return sb.ToString();
        }
    }

    // Базовый класс для операторов
    public abstract class StmtAst : AstNode
    {
        protected StmtAst(int line, int column) : base(line, column) { }
    }

    // Оператор print
    public class PrintStmtAst : StmtAst
    {
        public List<ExprAst> Arguments { get; }

        public PrintStmtAst(int line, int column, List<ExprAst> arguments)
            : base(line, column)
        {
            Arguments = arguments;
        }

        public override string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine(new string(' ', indent) + $"PrintStmt [Line: {Line}, Col: {Column}]");
            foreach (var arg in Arguments)
            {
                sb.AppendLine(arg.ToString(indent + 2));
            }
            return sb.ToString();
        }
    }

    // Объявление переменной
    public class VarDeclExprAst : StmtAst
    {
        public string Name { get; }
        public string Type { get; }
        public ExprAst Initializer { get; } // Может быть null

        public VarDeclExprAst(int line, int column, string name, string type, ExprAst initializer)
            : base(line, column)
        {
            Name = name;
            Type = type;
            Initializer = initializer;
        }

        public override string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine(new string(' ', indent) + $"VarDeclExpr({Name}: {Type}) [Line: {Line}, Col: {Column}]");
            if (Initializer != null)
            {
                sb.AppendLine(new string(' ', indent + 2) + "Initializer:");
                sb.AppendLine(Initializer.ToString(indent + 4));
            }
            return sb.ToString();
        }
    }

    public class VarDeclStmtAst : StmtAst
    {
        public string Name { get; }
        public string Type { get; }
        public StmtAst Initializer { get; } // Не может быть null

        public VarDeclStmtAst(int line, int column, string name, string type, StmtAst initializer)
            : base(line, column)
        {
            Name = name;
            Type = type;
            Initializer = initializer;
        }

        public override string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine(new string(' ', indent) + $"VarDeclStmt({Name}: {Type}) [Line: {Line}, Col: {Column}]");
            if (Initializer != null)
            {
                sb.AppendLine(new string(' ', indent + 2) + "Initializer:");
                sb.AppendLine(Initializer.ToString(indent + 4));
            }
            return sb.ToString();
        }
    }


    // Присваивание
    public class AssignExprAst : StmtAst
    {
        public string Name { get; }
        public ExprAst Value { get; }

        public AssignExprAst(int line, int column, string name, ExprAst value)
            : base(line, column)
        {
            Name = name;
            Value = value;
        }

        public override string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine(new string(' ', indent) + $"AssignExpr({Name}) [Line: {Line}, Col: {Column}]");
            sb.AppendLine(new string(' ', indent + 2) + "Value:");
            sb.AppendLine(Value.ToString(indent + 4));
            return sb.ToString();
        }
    }

    // Присваивание
    public class AssignStmtAst : StmtAst
    {
        public string Name { get; }
        public StmtAst Value { get; }

        public AssignStmtAst(int line, int column, string name, StmtAst value)
            : base(line, column)
        {
            Name = name;
            Value = value;
        }

        public override string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine(new string(' ', indent) + $"AssignStmt({Name}) [Line: {Line}, Col: {Column}]");
            sb.AppendLine(new string(' ', indent + 2) + "Value:");
            sb.AppendLine(Value.ToString(indent + 4));
            return sb.ToString();
        }
    }

    // Блок кода
    public class BlockStmtAst : StmtAst
    {
        public List<StmtAst> Statements { get; }

        public BlockStmtAst(int line, int column, List<StmtAst> statements)
            : base(line, column)
        {
            Statements = statements;
        }

        public override string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine(new string(' ', indent) + $"BlockStmt [Line: {Line}, Col: {Column}]");
            foreach (var stmt in Statements)
            {
                sb.AppendLine(stmt.ToString(indent + 2));
            }
            return sb.ToString();
        }
    }

    // Конструкция if-else
    public class IfStmtAst : StmtAst
    {
        public ExprAst Condition { get; }
        public StmtAst ThenBody { get; }
        public StmtAst ElseBody { get; } // Может быть null

        public IfStmtAst(int line, int column, ExprAst condition, StmtAst thenBody, StmtAst elseBody)
            : base(line, column)
        {
            Condition = condition;
            ThenBody = thenBody;
            ElseBody = elseBody;
        }

        public override string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine(new string(' ', indent) + $"IfStmt [Line: {Line}, Col: {Column}]");
            sb.AppendLine(new string(' ', indent + 2) + "Condition:");
            sb.AppendLine(Condition.ToString(indent + 4));
            sb.AppendLine(new string(' ', indent + 2) + "Then:");
            sb.AppendLine(ThenBody.ToString(indent + 4));
            if (ElseBody != null)
            {
                sb.AppendLine(new string(' ', indent + 2) + "Else:");
                sb.AppendLine(ElseBody.ToString(indent + 4));
            }
            return sb.ToString();
        }
    }
    // Цикл for
    public class ForStmtAst : StmtAst
    {
        public StmtAst Initializer { get; } // Инициализация (например, let i: int = 0)
        public ExprAst Condition { get; } // Условие (например, i < 10)
        public ExprAst Increment { get; } // Итерация (например, ++i)
        public StmtAst Body { get; } // Тело цикла (блок)

        public ForStmtAst(int line, int column, StmtAst initializer, ExprAst condition, ExprAst increment, StmtAst body)
            : base(line, column)
        {
            Initializer = initializer;
            Condition = condition;
            Increment = increment;
            Body = body;
        }

        public override string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine(new string(' ', indent) + $"ForStmt [Line: {Line}, Col: {Column}]");
            if (Initializer != null)
            {
                sb.AppendLine(new string(' ', indent + 2) + "Initializer:");
                sb.AppendLine(Initializer.ToString(indent + 4));
            }
            if (Condition != null)
            {
                sb.AppendLine(new string(' ', indent + 2) + "Condition:");
                sb.AppendLine(Condition.ToString(indent + 4));
            }
            if (Increment != null)
            {
                sb.AppendLine(new string(' ', indent + 2) + "Increment:");
                sb.AppendLine(Increment.ToString(indent + 4));
            }
            sb.AppendLine(new string(' ', indent + 2) + "Body:");
            sb.AppendLine(Body.ToString(indent + 4));
            return sb.ToString();
        }
    }

    // Унарное выражение (например, ++i)
    public class UnaryExprAst : ExprAst
    {
        public string Operator { get; } // Оператор (например, "++")
        public ExprAst Operand { get; } // Операнд (например, i)

        public UnaryExprAst(int line, int column, string op, ExprAst operand)
            : base(line, column)
        {
            Operator = op;
            Operand = operand;
        }

        public override string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine(new string(' ', indent) + $"UnaryExpr({Operator}) [Line: {Line}, Col: {Column}]");
            sb.AppendLine(Operand.ToString(indent + 2));
            return sb.ToString();
        }
    }


    // Параметр функции или конструктора
    public class ParameterAst : AstNode
    {
        public string Name { get; }
        public string Type { get; }

        public ParameterAst(int line, int column, string name, string type)
            : base(line, column)
        {
            Name = name;
            Type = type;
        }

        public override string ToString(int indent)
        {
            return $"{new string(' ', indent)}Param({Name}: {Type}) [Line: {Line}, Col: {Column}]";
        }
    }

    // Объявление класса
    public class ClassDeclStmt : StmtAst
    {
        public string Name { get; }
        public string BaseClass { get; } // null, если нет наследования
        public List<StmtAst> Members { get; } // Конструкторы, методы, виртуальные методы

        public ClassDeclStmt(int line, int column, string name, string baseClass, List<StmtAst> members)
            : base(line, column)
        {
            Name = name;
            BaseClass = baseClass;
            Members = members;
        }

        public override string ToString(int indent)
        {
            var result = $"{new string(' ', indent)}ClassDeclStmt [Line: {Line}, Col: {Column}]\n" +
                         $"{new string(' ', indent + 2)}Name: {Name}\n" +
                         $"{new string(' ', indent + 2)}BaseClass: {(BaseClass ?? "null")}";
            if (Members.Any())
            {
                result += $"\n{new string(' ', indent + 2)}Members:";
                foreach (var member in Members)
                {
                    result += $"\n{member.ToString(indent + 4)}";
                }
            }
            return result;
        }
    }

    // Объявление конструктора
    public class ConstructorDeclStmt : StmtAst
    {
        public List<ParameterAst> Parameters { get; }
        public StmtAst Body { get; }

        public ConstructorDeclStmt(int line, int column, List<ParameterAst> parameters, StmtAst body)
            : base(line, column)
        {
            Parameters = parameters;
            Body = body;
        }

        public override string ToString(int indent)
        {
            var result = $"{new string(' ', indent)}ConstructorDeclStmt [Line: {Line}, Col: {Column}]";
            if (Parameters.Any())
            {
                result += $"\n{new string(' ', indent + 2)}Parameters:";
                foreach (var param in Parameters)
                {
                    result += $"\n{param.ToString(indent + 4)}";
                }
            }
            result += $"\n{new string(' ', indent + 2)}Body:\n{Body.ToString(indent + 4)}";
            return result;
        }
    }

    // Объявление виртуального метода
    public class VirtualMethodDeclStmt : StmtAst
    {
        public string Name { get; }
        public string ReturnType { get; } // null, если void
        public List<ParameterAst> Parameters { get; }

        public VirtualMethodDeclStmt(int line, int column, string name, string returnType, List<ParameterAst> parameters)
            : base(line, column)
        {
            Name = name;
            ReturnType = returnType;
            Parameters = parameters;
        }

        public override string ToString(int indent)
        {
            var result = $"{new string(' ', indent)}VirtualMethodDeclStmt [Line: {Line}, Col: {Column}]\n" +
                         $"{new string(' ', indent + 2)}Name: {Name}\n" +
                         $"{new string(' ', indent + 2)}ReturnType: {(ReturnType ?? "null")}";
            if (Parameters.Any())
            {
                result += $"\n{new string(' ', indent + 2)}Parameters:";
                foreach (var param in Parameters)
                {
                    result += $"\n{param.ToString(indent + 4)}";
                }
            }
            return result;
        }
    }

    // Реализация виртуального метода
    public class ImplMethodDeclStmt : StmtAst
    {
        public string Name { get; }
        public string ReturnType { get; } // null, если void
        public List<ParameterAst> Parameters { get; }
        public StmtAst Body { get; }

        public ImplMethodDeclStmt(int line, int column, string name, string returnType, List<ParameterAst> parameters, StmtAst body)
            : base(line, column)
        {
            Name = name;
            ReturnType = returnType;
            Parameters = parameters;
            Body = body;
        }

        public override string ToString(int indent)
        {
            var result = $"{new string(' ', indent)}ImplMethodDeclStmt [Line: {Line}, Col: {Column}]\n" +
                         $"{new string(' ', indent + 2)}Name: {Name}\n" +
                         $"{new string(' ', indent + 2)}ReturnType: {(ReturnType ?? "null")}";
            if (Parameters.Any())
            {
                result += $"\n{new string(' ', indent + 2)}Parameters:";
                foreach (var param in Parameters)
                {
                    result += $"\n{param.ToString(indent + 4)}";
                }
            }
            result += $"\n{new string(' ', indent + 2)}Body:\n{Body.ToString(indent + 4)}";
            return result;
        }
    }

    // Объявление функции
    public class FunctionDeclStmt : StmtAst
    {
        public string Name { get; }
        public string ReturnType { get; } // null, если void
        public List<ParameterAst> Parameters { get; }
        public StmtAst Body { get; }

        public FunctionDeclStmt(int line, int column, string name, string returnType, List<ParameterAst> parameters, StmtAst body)
            : base(line, column)
        {
            Name = name;
            ReturnType = returnType;
            Parameters = parameters;
            Body = body;
        }

        public override string ToString(int indent)
        {
            var result = $"{new string(' ', indent)}FunctionDeclStmt [Line: {Line}, Col: {Column}]\n" +
                         $"{new string(' ', indent + 2)}Name: {Name}\n" +
                         $"{new string(' ', indent + 2)}ReturnType: {(ReturnType ?? "null")}";
            if (Parameters.Any())
            {
                result += $"\n{new string(' ', indent + 2)}Parameters:";
                foreach (var param in Parameters)
                {
                    result += $"\n{param.ToString(indent + 4)}";
                }
            }
            result += $"\n{new string(' ', indent + 2)}Body:\n{Body.ToString(indent + 4)}";
            return result;
        }
    }

    // Оператор return
    public class ReturnStmt : StmtAst
    {
        public ExprAst Value { get; } // null, если return без значения

        public ReturnStmt(int line, int column, ExprAst value)
            : base(line, column)
        {
            Value = value;
        }

        public override string ToString(int indent)
        {
            var result = $"{new string(' ', indent)}ReturnStmt [Line: {Line}, Col: {Column}]";
            if (Value != null)
            {
                result += $"\n{new string(' ', indent + 2)}Value:\n{Value.ToString(indent + 4)}";
            }
            return result;
        }
    }

    // Выражение создания экземпляра (new)
    public class NewStmt : StmtAst
    {
        public string ClassName { get; }
        public List<ExprAst> Arguments { get; }

        public NewStmt(int line, int column, string className, List<ExprAst> arguments)
            : base(line, column)
        {
            ClassName = className;
            Arguments = arguments;
        }

        public override string ToString(int indent)
        {
            var result = $"{new string(' ', indent)}NewExpr({ClassName}) [Line: {Line}, Col: {Column}]";
            if (Arguments.Any())
            {
                result += $"\n{new string(' ', indent + 2)}Arguments:";
                foreach (var arg in Arguments)
                {
                    result += $"\n{arg.ToString(indent + 4)}";
                }
            }
            return result;
        }
    }

    // Вызов метода
    public class CallStmt : StmtAst
    {
        public ExprAst Receiver { get; } // Объект (например, p)
        public string Method { get; }
        public List<ExprAst> Arguments { get; }

        public CallStmt(int line, int column, ExprAst receiver, string method, List<ExprAst> arguments)
            : base(line, column)
        {
            Receiver = receiver;
            Method = method;
            Arguments = arguments;
        }

        public override string ToString(int indent)
        {
            var result = $"{new string(' ', indent)}CallStmt [Line: {Line}, Col: {Column}]\n" +
                         $"{new string(' ', indent + 2)}Receiver:\n{Receiver.ToString(indent + 4)}\n" +
                         $"{new string(' ', indent + 2)}Method: {Method}";
            if (Arguments.Any())
            {
                result += $"\n{new string(' ', indent + 2)}Arguments:";
                foreach (var arg in Arguments)
                {
                    result += $"\n{arg.ToString(indent + 4)}";
                }
            }
            return result;
        }
    }

    // Оператор удаления переменной
    public class DeleteStmt : StmtAst
    {
        public string Variable { get; }

        public DeleteStmt(int line, int column, string variable)
            : base(line, column)
        {
            Variable = variable;
        }

        public override string ToString(int indent)
        {
            return $"{new string(' ', indent)}DeleteStmt({Variable}) [Line: {Line}, Col: {Column}]";
        }
    }

    // Выражение доступа к члену (например, obj.field)
    public class MemberAccessExprAst : ExprAst
    {
        public ExprAst Receiver { get; } // Объект (например, obj)
        public string Member { get; } // Имя поля (например, field)

        public MemberAccessExprAst(int line, int column, ExprAst receiver, string member)
            : base(line, column)
        {
            Receiver = receiver;
            Member = member;
        }

        public override string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine(new string(' ', indent) + $"MemberAccessExpr({Member}) [Line: {Line}, Col: {Column}]");
            sb.AppendLine(new string(' ', indent + 2) + "Receiver:");
            sb.AppendLine(Receiver.ToString(indent + 4));
            return sb.ToString();
        }
    }

    // Выражение доступа (например, obj.method())
    public class MemberAccessStmtAst : ExprAst
    {
        public ExprAst Receiver { get; } // Объект (например, obj)
        public string Member { get; } // Имя поля (например, field)

        public MemberAccessStmtAst(int line, int column, ExprAst receiver, string member)
            : base(line, column)
        {
            Receiver = receiver;
            Member = member;
        }

        public override string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine(new string(' ', indent) + $"MemberAccessStmt({Member}) [Line: {Line}, Col: {Column}]");
            sb.AppendLine(new string(' ', indent + 2) + "Receiver:");
            sb.AppendLine(Receiver.ToString(indent + 4));
            return sb.ToString();
        }
    }

}