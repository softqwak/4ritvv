using Demo.Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Demo
{

    public class SemanticAnalyzer
    {
        // Стек таблиц символов для переменных (каждая таблица — область видимости)
        private readonly List<Dictionary<string, VariableInfo>> _variableScopes;
        // Таблица символов для классов (глобальная)
        private readonly Dictionary<string, ClassInfo> _classes;
        // Таблица символов для функций (глобальная)
        private readonly Dictionary<string, FunctionInfo> _functions;
        // Текущая функция или метод для проверки return
        private string _currentFunctionReturnType;
        // Текущий класс для обработки полей (в конструкторах и методах)
        private ClassInfo _currentClass;

        public SemanticAnalyzer()
        {
            _variableScopes = new List<Dictionary<string, VariableInfo>>();
            _variableScopes.Add(new Dictionary<string, VariableInfo>()); // Глобальная область
            _classes = new Dictionary<string, ClassInfo>();
            _functions = new Dictionary<string, FunctionInfo>();
            _currentFunctionReturnType = null;
            _currentClass = null;
        }

        // Анализ списка узлов AST
        public void Analyze(List<StmtAst> ast)
        {
            if (ast == null)
            {
                Diagnostics.Report(0, 0, "Список AST пуст");
                return;
            }

            foreach (var node in ast)
            {
                if (node == null)
                {
                    continue;
                }

                switch (node)
                {
                    case ClassDeclStmt classDecl:
                        RegisterClass(classDecl);
                        break;
                    case VarDeclExprAst varDeclExpr:
                        AnalyzeVarDeclExpr(varDeclExpr);
                        break;
                    case VarDeclStmtAst varDeclStmt:
                        AnalyzeVarDeclStmt(varDeclStmt);
                        break;
                    case BlockStmtAst blockStmt:
                        BeginScope();
                        Analyze(blockStmt.Statements);
                        EndScope();
                        break;
                    case IfStmtAst ifStmt:
                        AnalyzeIfStmt(ifStmt);
                        break;
                    case ForStmtAst forStmt:
                        AnalyzeForStmt(forStmt);
                        break;
                    case FunctionDeclStmt funcDecl:
                        AnalyzeFunctionDecl(funcDecl);
                        break;
                    case AssignStmtAst assignStmt:
                        AnalyzeAssignStmt(assignStmt);
                        break;
                    case AssignExprAst assignExpr:
                        AnalyzeAssignExpr(assignExpr);
                        break;
                    case PrintStmtAst printStmt:
                        AnalyzePrintStmt(printStmt);
                        break;
                    case ReturnStmt returnStmt:
                        AnalyzeReturnStmt(returnStmt);
                        break;
                    case NewStmt newStmt:
                        AnalyzeNewStmt(newStmt);
                        break;
                    case CallStmt callStmt:
                        AnalyzeCallStmt(callStmt);
                        break;
                    case DeleteStmt deleteStmt:
                        AnalyzeDeleteStmt(deleteStmt);
                        break;
                    case ConstructorDeclStmt constructorDecl:
                        AnalyzeConstructorDecl(constructorDecl);
                        break;
                    case VirtualMethodDeclStmt virtualMethod:
                        AnalyzeVirtualMethodDecl(virtualMethod);
                        break;
                    case ImplMethodDeclStmt implMethod:
                        AnalyzeImplMethodDecl(implMethod);
                        break;
                    default:
                        Diagnostics.Warning(node.Line, node.Column,
                            $"Узел {node.GetType().Name} не поддерживается в семантическом анализе");
                        break;
                }
            }
        }

        // Анализ одного оператора
        private void AnalyzeSingleStmt(StmtAst stmt)
        {
            if (stmt == null)
            {
                return;
            }

            if (stmt is BlockStmtAst blockStmt)
            {
                BeginScope();
                Analyze(blockStmt.Statements);
                EndScope();
            }
            else
            {
                Analyze(new List<StmtAst> { stmt });
            }
        }

        // Анализ выражения
        private void AnalyzeExpression(ExprAst expr)
        {
            if (expr == null)
            {
                return;
            }

            switch (expr)
            {
                case IntExprAst _:
                case FloatExprAst _:
                case StringExprAst _:
                    break;
                case IdentifierExprAst idExpr:
                    if (!IsVariableDeclared(idExpr.Name))
                    {
                        // Проверяем, является ли идентификатор полем текущего класса
                        if (_currentClass != null && IsFieldInClass(_currentClass, idExpr.Name))
                        {
                            // Поле класса найдено, ошибка не выдаётся
                            return;
                        }
                        Diagnostics.Report(idExpr.Line, idExpr.Column,
                            $"Переменная '{idExpr.Name}' не объявлена");
                    }
                    break;
                case BinaryExprAst binaryExpr:
                    AnalyzeExpression(binaryExpr.Left);
                    AnalyzeExpression(binaryExpr.Right);
                    string leftType = InferType(binaryExpr.Left);
                    string rightType = InferType(binaryExpr.Right);
                    break;
                case UnaryExprAst unaryExpr:
                    AnalyzeExpression(unaryExpr.Operand);
                    if (unaryExpr.Operator == "++" && InferType(unaryExpr.Operand) != "int")
                    {
                        Diagnostics.Report(unaryExpr.Line, unaryExpr.Column,
                            $"Оператор '++' применим только к типу 'int'");
                    }
                    break;
                case LogicalExprAst logicalExpr:
                    AnalyzeExpression(logicalExpr.Left);
                    AnalyzeExpression(logicalExpr.Right);
                    string logicalLeftType = InferType(logicalExpr.Left);
                    string logicalRightType = InferType(logicalExpr.Right);
                    break;
                case MemberAccessExprAst memberAccess:
                    AnalyzeMemberAccessExpr(memberAccess);
                    break;
                case MemberAccessStmtAst memberAccessStmt:
                    AnalyzeMemberAccessStmt(memberAccessStmt);
                    break;
                default:
                    Diagnostics.Warning(expr.Line, expr.Column,
                        $"Выражение {expr.GetType().Name} не поддерживается в семантическом анализе");
                    break;
            }
        }

        // Регистрация класса
        private void RegisterClass(ClassDeclStmt classDecl)
        {
            if (_classes.ContainsKey(classDecl.Name))
            {
                Diagnostics.Report(classDecl.Line, classDecl.Column,
                    $"Класс '{classDecl.Name}' уже объявлен в строке {_classes[classDecl.Name].Line}");
                return;
            }

            // Проверяем базовый класс
            if (classDecl.BaseClass != null && !_classes.ContainsKey(classDecl.BaseClass))
            {
                Diagnostics.Report(classDecl.Line, classDecl.Column,
                    $"Базовый класс '{classDecl.BaseClass}' не объявлен");
                return;
            }

            var classInfo = new ClassInfo(classDecl.Name, classDecl.BaseClass, classDecl.Line, classDecl.Column);
            _classes[classDecl.Name] = classInfo;

            // Сохраняем текущий класс
            var previousClass = _currentClass;
            _currentClass = classInfo;

            // Регистрируем поля, конструкторы и методы
            foreach (var member in classDecl.Members)
            {
                if (member is VarDeclExprAst varDeclExpr)
                {
                    if (classInfo.Fields.ContainsKey(varDeclExpr.Name))
                    {
                        Diagnostics.Report(varDeclExpr.Line, varDeclExpr.Column,
                            $"Поле '{varDeclExpr.Name}' уже объявлено в классе '{classDecl.Name}'");
                    }
                    else if (!IsValidType(varDeclExpr.Type, varDeclExpr.Line, varDeclExpr.Column))
                    {
                        Diagnostics.Report(varDeclExpr.Line, varDeclExpr.Column,
                            $"Недопустимый тип '{varDeclExpr.Type}' для поля '{varDeclExpr.Name}'");
                    }
                    else
                    {
                        // Регистрируем поле
                        classInfo.Fields[varDeclExpr.Name] = varDeclExpr.Type;
                        if (varDeclExpr.Initializer != null)
                        {
                            AnalyzeExpression(varDeclExpr.Initializer);
                            string initType = InferType(varDeclExpr.Initializer);
                            if (initType != null && !AreTypesCompatible(varDeclExpr.Type, initType))
                            {
                                Diagnostics.Report(varDeclExpr.Line, varDeclExpr.Column,
                                    $"Несовместимый тип инициализатора поля: ожидался '{varDeclExpr.Type}', получен '{initType}'");
                            }
                        }
                        else
                        {
                            // Предупреждение о неинициализированном поле
                            Diagnostics.Warning(varDeclExpr.Line, varDeclExpr.Column,
                                $"Переменная '{varDeclExpr.Name}' объявлена без инициализации");
                        }
                    }
                }
                else if (member is VarDeclStmtAst varDeclStmt)
                {
                    if (classInfo.Fields.ContainsKey(varDeclStmt.Name))
                    {
                        Diagnostics.Report(varDeclStmt.Line, varDeclStmt.Column,
                            $"Поле '{varDeclStmt.Name}' уже объявлено в классе '{classDecl.Name}'");
                    }
                    else if (!IsValidType(varDeclStmt.Type, varDeclStmt.Line, varDeclStmt.Column))
                    {
                        Diagnostics.Report(varDeclStmt.Line, varDeclStmt.Column,
                            $"Недопустимый тип '{varDeclStmt.Type}' для поля '{varDeclStmt.Name}'");
                    }
                    else
                    {
                        // Регистрируем поле
                        classInfo.Fields[varDeclStmt.Name] = varDeclStmt.Type;
                        if (varDeclStmt.Initializer != null)
                        {
                            AnalyzeSingleStmt(varDeclStmt.Initializer);
                            string initType = InferStmtType(varDeclStmt.Initializer);
                            if (initType != null && !AreTypesCompatible(varDeclStmt.Type, initType))
                            {
                                Diagnostics.Report(varDeclStmt.Line, varDeclStmt.Column,
                                    $"Несовместимый тип инициализатора поля: ожидался '{varDeclStmt.Type}', получен '{initType}'");
                            }
                        }
                        else
                        {
                            // Предупреждение о неинициализированном поле
                            Diagnostics.Warning(varDeclStmt.Line, varDeclStmt.Column,
                                $"Переменная '{varDeclStmt.Name}' объявлена без инициализации");
                        }
                    }
                }
                else if (member is ConstructorDeclStmt constructorDecl)
                {
                    AnalyzeConstructorDecl(constructorDecl, classInfo);
                }
                else if (member is VirtualMethodDeclStmt virtualMethod)
                {
                    AnalyzeVirtualMethodDecl(virtualMethod, classInfo);
                }
                else if (member is ImplMethodDeclStmt implMethod)
                {
                    AnalyzeImplMethodDecl(implMethod, classInfo);
                }
            }

            // Восстанавливаем предыдущий класс
            _currentClass = previousClass;
        }

        // Анализ объявления переменной с выражением
        private void AnalyzeVarDeclExpr(VarDeclExprAst varDecl)
        {
            var currentScope = _variableScopes[_variableScopes.Count - 1];
            if (currentScope.ContainsKey(varDecl.Name))
            {
                Diagnostics.Report(varDecl.Line, varDecl.Column,
                    $"Переменная '{varDecl.Name}' уже объявлена в текущей области видимости");
                return;
            }

            if (!IsValidType(varDecl.Type, varDecl.Line, varDecl.Column))
            {
                Diagnostics.Report(varDecl.Line, varDecl.Column,
                    $"Недопустимый тип '{varDecl.Type}' для переменной '{varDecl.Name}'");
                return;
            }

            if (varDecl.Initializer != null)
            {
                AnalyzeExpression(varDecl.Initializer);
                string initializerType = InferType(varDecl.Initializer);
                if (initializerType == null)
                {
                    Diagnostics.Report(varDecl.Line, varDecl.Column,
                        $"Не удалось определить тип инициализатора для переменной '{varDecl.Name}'");
                    return;
                }
                if (!AreTypesCompatible(varDecl.Type, initializerType))
                {
                    Diagnostics.Report(varDecl.Line, varDecl.Column,
                        $"Несовместимый тип инициализатора: ожидался '{varDecl.Type}', получен '{initializerType}'");
                    return;
                }
            }

            currentScope[varDecl.Name] = new VariableInfo(varDecl.Name, varDecl.Type, varDecl.Line, varDecl.Column);
        }

        // Анализ объявления переменной с new
        private void AnalyzeVarDeclStmt(VarDeclStmtAst varDecl)
        {
            var currentScope = _variableScopes[_variableScopes.Count - 1];
            if (currentScope.ContainsKey(varDecl.Name))
            {
                Diagnostics.Report(varDecl.Line, varDecl.Column,
                    $"Переменная '{varDecl.Name}' уже объявлена в текущей области видимости");
                return;
            }

            if (!IsValidType(varDecl.Type, varDecl.Line, varDecl.Column))
            {
                Diagnostics.Report(varDecl.Line, varDecl.Column,
                    $"Недопустимый тип '{varDecl.Type}' для переменной '{varDecl.Name}'");
                return;
            }

            if (varDecl.Initializer != null)
            {
                if (!(varDecl.Initializer is NewStmt newStmt))
                {
                    Diagnostics.Report(varDecl.Line, varDecl.Column,
                        $"Ожидалась конструкция 'new' для инициализации переменной '{varDecl.Name}'");
                    return;
                }
                AnalyzeNewStmt(newStmt);
                if (newStmt.ClassName != varDecl.Type)
                {
                    Diagnostics.Report(varDecl.Line, varDecl.Column,
                        $"Тип инициализатора '{newStmt.ClassName}' не соответствует типу переменной '{varDecl.Type}'");
                    return;
                }
            }

            currentScope[varDecl.Name] = new VariableInfo(varDecl.Name, varDecl.Type, varDecl.Line, varDecl.Column);
        }

        // Анализ условного оператора
        private void AnalyzeIfStmt(IfStmtAst ifStmt)
        {
            // Анализируем выражение условия
            AnalyzeExpression(ifStmt.Condition);

            // Определяем тип условия
            string conditionType = InferType(ifStmt.Condition);

            // Проверяем, что тип условия — логический (bool)
            if (conditionType != null && conditionType != "bool")
            {
                Diagnostics.Report(ifStmt.Condition.Line, ifStmt.Condition.Column,
                    $"Условие if должно иметь тип 'bool', получен '{conditionType}'");
            }

            // Анализируем тело then-ветки
            AnalyzeSingleStmt(ifStmt.ThenBody);

            // Анализируем тело else-ветки, если она есть
            if (ifStmt.ElseBody != null)
            {
                AnalyzeSingleStmt(ifStmt.ElseBody);
            }
        }

        // Анализ цикла for
        private void AnalyzeForStmt(ForStmtAst forStmt)
        {
            BeginScope();
            if (forStmt.Initializer != null)
            {
                AnalyzeSingleStmt(forStmt.Initializer);
            }
            if (forStmt.Condition != null)
            {
                AnalyzeExpression(forStmt.Condition);
                string conditionType = InferType(forStmt.Condition);
                if (conditionType != null && conditionType != "int")
                {
                    Diagnostics.Report(forStmt.Condition.Line, forStmt.Condition.Column,
                        $"Условие цикла должно иметь тип 'int', получен '{conditionType}'");
                }
            }
            if (forStmt.Increment != null)
            {
                AnalyzeExpression(forStmt.Increment);
            }
            AnalyzeSingleStmt(forStmt.Body);
            EndScope();
        }

        // Анализ объявления функции
        private void AnalyzeFunctionDecl(FunctionDeclStmt funcDecl)
        {
            if (_functions.ContainsKey(funcDecl.Name))
            {
                Diagnostics.Report(funcDecl.Line, funcDecl.Column,
                    $"Функция '{funcDecl.Name}' уже объявлена в строке {_functions[funcDecl.Name].Line}");
                return;
            }

            if (!IsValidType(funcDecl.ReturnType, funcDecl.Line, funcDecl.Column) && funcDecl.ReturnType != "void")
            {
                Diagnostics.Report(funcDecl.Line, funcDecl.Column,
                    $"Недопустимый возвращаемый тип '{funcDecl.ReturnType}' для функции '{funcDecl.Name}'");
                return;
            }

            var parameters = new List<(string, string)>();
            foreach (var param in funcDecl.Parameters)
            {
                if (!IsValidType(param.Type, param.Line, param.Column))
                {
                    Diagnostics.Report(param.Line, param.Column,
                        $"Недопустимый тип '{param.Type}' для параметра '{param.Name}'");
                    return;
                }
                parameters.Add((param.Name, param.Type));
            }

            _functions[funcDecl.Name] = new FunctionInfo(funcDecl.Name, funcDecl.ReturnType, parameters,
                funcDecl.Line, funcDecl.Column);

            BeginScope();
            foreach (var param in funcDecl.Parameters)
            {
                var currentScope = _variableScopes[_variableScopes.Count - 1];
                if (currentScope.ContainsKey(param.Name))
                {
                    Diagnostics.Report(param.Line, param.Column,
                        $"Параметр '{param.Name}' уже объявлен");
                    continue;
                }
                currentScope[param.Name] = new VariableInfo(param.Name, param.Type, param.Line, param.Column);
            }
            _currentFunctionReturnType = funcDecl.ReturnType;
            AnalyzeSingleStmt(funcDecl.Body);
            _currentFunctionReturnType = null;
            EndScope();
        }

        // Анализ присваивания (stmt)
        private void AnalyzeAssignStmt(AssignStmtAst assignStmt)
        {
            AnalyzeSingleStmt(assignStmt.Value);
            string valueType = InferStmtType(assignStmt.Value);
            if (!IsVariableDeclared(assignStmt.Name))
            {
                // Проверяем, является ли идентификатор полем текущего класса
                if (_currentClass != null && IsFieldInClass(_currentClass, assignStmt.Name))
                {
                    // Отладка: подтверждаем, что поле найдено
                    Diagnostics.Warning(assignStmt.Line, assignStmt.Column,
                        $"Найдено поле '{assignStmt.Name}' в классе '{_currentClass.Name}'");
                    string fieldType = FindField(_currentClass.Name, assignStmt.Name);
                    if (valueType != null && !AreTypesCompatible(fieldType, valueType))
                    {
                        Diagnostics.Report(assignStmt.Line, assignStmt.Column,
                            $"Несовместимый тип присваивания для поля '{assignStmt.Name}': ожидался '{fieldType}', получен '{valueType}'");
                    }
                    return;
                }
                Diagnostics.Report(assignStmt.Line, assignStmt.Column,
                    $"Переменная '{assignStmt.Name}' не объявлена");
                return;
            }
            // Отладка: подтверждаем, что переменная найдена
            Diagnostics.Warning(assignStmt.Line, assignStmt.Column,
                $"Найдена переменная '{assignStmt.Name}' в области видимости");
            string varType = GetVariableType(assignStmt.Name);
            if (valueType != null && varType != null && !AreTypesCompatible(varType, valueType))
            {
                Diagnostics.Report(assignStmt.Line, assignStmt.Column,
                    $"Несовместимый тип присваивания: ожидался '{varType}', получен '{valueType}'");
            }
        }

        // Анализ присваивания (expr)
        private void AnalyzeAssignExpr(AssignExprAst assignExpr)
        {
            AnalyzeExpression(assignExpr.Value);
            string valueType = InferType(assignExpr.Value);
            if (!IsVariableDeclared(assignExpr.Name))
            {
                // Проверяем, является ли идентификатор полем текущего класса
                if (_currentClass != null && IsFieldInClass(_currentClass, assignExpr.Name))
                {
                    string fieldType = FindField(_currentClass.Name, assignExpr.Name);
                    if (valueType != null && !AreTypesCompatible(fieldType, valueType))
                    {
                        Diagnostics.Report(assignExpr.Line, assignExpr.Column,
                            $"Несовместимый тип присваивания для поля '{assignExpr.Name}': ожидался '{fieldType}', получен '{valueType}'");
                    }
                    return;
                }
                Diagnostics.Report(assignExpr.Line, assignExpr.Column,
                    $"Переменная '{assignExpr.Name}' не объявлена");
                return;
            }
            string varType = GetVariableType(assignExpr.Name);
            if (valueType != null && varType != null && !AreTypesCompatible(varType, valueType))
            {
                Diagnostics.Report(assignExpr.Line, assignExpr.Column,
                    $"Несовместимый тип присваивания: ожидался '{varType}', получен '{valueType}'");
            }
        }

        // Анализ print
        private void AnalyzePrintStmt(PrintStmtAst printStmt)
        {
            foreach (var arg in printStmt.Arguments)
            {
                AnalyzeExpression(arg);
            }
        }

        // Анализ return
        private void AnalyzeReturnStmt(ReturnStmt returnStmt)
        {
            if (_currentFunctionReturnType == null)
            {
                Diagnostics.Report(returnStmt.Line, returnStmt.Column,
                    $"Оператор 'return' вне функции или метода");
                return;
            }

            string returnType = returnStmt.Value != null ? InferType(returnStmt.Value) : "void";
            if (returnStmt.Value != null)
            {
                AnalyzeExpression(returnStmt.Value);
            }

            if (!AreTypesCompatible(_currentFunctionReturnType, returnType))
            {
                Diagnostics.Report(returnStmt.Line, returnStmt.Column,
                    $"Несовместимый тип возвращаемого значения: ожидался '{_currentFunctionReturnType}', получен '{returnType}'");
            }
        }

        // Анализ new
        private void AnalyzeNewStmt(NewStmt newStmt)
        {
            if (!_classes.ContainsKey(newStmt.ClassName))
            {
                Diagnostics.Report(newStmt.Line, newStmt.Column,
                    $"Класс '{newStmt.ClassName}' не объявлен");
                return;
            }

            var classInfo = _classes[newStmt.ClassName];
            bool constructorFound = false;
            foreach (var constructor in classInfo.Constructors)
            {
                if (constructor.Parameters.Count == newStmt.Arguments.Count)
                {
                    bool paramsMatch = true;
                    for (int i = 0; i < newStmt.Arguments.Count; i++)
                    {
                        AnalyzeExpression(newStmt.Arguments[i]);
                        string argType = InferType(newStmt.Arguments[i]);
                        if (argType == null || !AreTypesCompatible(constructor.Parameters[i].Type, argType))
                        {
                            paramsMatch = false;
                            break;
                        }
                    }
                    if (paramsMatch)
                    {
                        constructorFound = true;
                        break;
                    }
                }
            }

            if (!constructorFound)
            {
                Diagnostics.Report(newStmt.Line, newStmt.Column,
                    $"Подходящий конструктор для класса '{newStmt.ClassName}' не найден");
            }
        }

        // Анализ вызова метода
        private void AnalyzeCallStmt(CallStmt callStmt)
        {
            AnalyzeExpression(callStmt.Receiver);
            string receiverType = InferType(callStmt.Receiver);
            if (receiverType == null || !_classes.ContainsKey(receiverType))
            {
                Diagnostics.Report(callStmt.Line, callStmt.Column,
                    $"Тип '{receiverType}' не является классом или не объявлен");
                return;
            }

            MethodInfo methodInfo = FindMethod(receiverType, callStmt.Method);
            if (methodInfo == null)
            {
                Diagnostics.Report(callStmt.Line, callStmt.Column,
                    $"Метод '{callStmt.Method}' не найден в классе '{receiverType}'");
                return;
            }

            if (callStmt.Arguments.Count != methodInfo.Parameters.Count)
            {
                Diagnostics.Report(callStmt.Line, callStmt.Column,
                    $"Неверное количество аргументов для метода '{callStmt.Method}': " +
                    $"ожидалось {methodInfo.Parameters.Count}, получено {callStmt.Arguments.Count}");
                return;
            }

            for (int i = 0; i < callStmt.Arguments.Count; i++)
            {
                AnalyzeExpression(callStmt.Arguments[i]);
                string argType = InferType(callStmt.Arguments[i]);
                string paramType = methodInfo.Parameters[i].Type;
                if (argType != null && !AreTypesCompatible(paramType, argType))
                {
                    Diagnostics.Report(callStmt.Arguments[i].Line, callStmt.Arguments[i].Column,
                        $"Несовместимый тип аргумента {i + 1}: ожидался '{paramType}', получен '{argType}'");
                }
            }
        }

        // Анализ удаления переменной
        private void AnalyzeDeleteStmt(DeleteStmt deleteStmt)
        {
            var currentScope = _variableScopes[_variableScopes.Count - 1];
            if (!currentScope.ContainsKey(deleteStmt.Variable))
            {
                Diagnostics.Report(deleteStmt.Line, deleteStmt.Column,
                    $"Переменная '{deleteStmt.Variable}' не объявлена в текущей области видимости");
                return;
            }
            currentScope.Remove(deleteStmt.Variable);
        }

        // Анализ конструктора
        private void AnalyzeConstructorDecl(ConstructorDeclStmt constructorDecl, ClassInfo classInfo = null)
        {
            var parameters = new List<(string, string)>();
            foreach (var param in constructorDecl.Parameters)
            {
                if (!IsValidType(param.Type, param.Line, param.Column))
                {
                    Diagnostics.Report(param.Line, param.Column,
                        $"Недопустимый тип '{param.Type}' для параметра '{param.Name}' конструктора");
                    return;
                }
                parameters.Add((param.Name, param.Type));
            }

            if (classInfo != null)
            {
                classInfo.Constructors.Add(new ConstructorInfo(parameters, constructorDecl.Line, constructorDecl.Column));
            }

            BeginScope();
            var currentScope = _variableScopes[_variableScopes.Count - 1];

            // Добавляем параметры конструктора в область видимости
            foreach (var param in constructorDecl.Parameters)
            {
                if (currentScope.ContainsKey(param.Name))
                {
                    Diagnostics.Report(param.Line, param.Column,
                        $"Параметр '{param.Name}' уже объявлен");
                    continue;
                }
                currentScope[param.Name] = new VariableInfo(param.Name, param.Type, param.Line, param.Column);
            }

            // Добавляем поля текущего класса и базовых классов в область видимости
            if (classInfo != null)
            {
                var allFields = GetAllFields(classInfo);
                foreach (var field in allFields)
                {
                    if (currentScope.ContainsKey(field.Key))
                    {
                        Diagnostics.Report(classInfo.Line, classInfo.Column,
                            $"Поле '{field.Key}' конфликтует с параметром или другой переменной");
                        continue;
                    }
                    currentScope[field.Key] = new VariableInfo(field.Key, field.Value, classInfo.Line, classInfo.Column);
                }
            }

            AnalyzeSingleStmt(constructorDecl.Body);
            EndScope();
        }

        // Анализ виртуального метода
        private void AnalyzeVirtualMethodDecl(VirtualMethodDeclStmt virtualMethod, ClassInfo classInfo = null)
        {
            if (!IsValidType(virtualMethod.ReturnType, virtualMethod.Line, virtualMethod.Column) && virtualMethod.ReturnType != "void")
            {
                Diagnostics.Report(virtualMethod.Line, virtualMethod.Column,
                    $"Недопустимый возвращаемый тип '{virtualMethod.ReturnType}' для метода '{virtualMethod.Name}'");
                return;
            }

            var parameters = new List<(string, string)>();
            foreach (var param in virtualMethod.Parameters)
            {
                if (!IsValidType(param.Type, param.Line, param.Column))
                {
                    Diagnostics.Report(param.Line, param.Column,
                        $"Недопустимый тип '{param.Type}' для параметра '{param.Name}'");
                    return;
                }
                parameters.Add((param.Name, param.Type));
            }

            if (classInfo != null)
            {
                if (classInfo.Methods.ContainsKey(virtualMethod.Name))
                {
                    Diagnostics.Report(virtualMethod.Line, virtualMethod.Column,
                        $"Метод '{virtualMethod.Name}' уже объявлен в классе '{classInfo.Name}'");
                    return;
                }
                classInfo.Methods[virtualMethod.Name] = new MethodInfo(virtualMethod.Name, virtualMethod.ReturnType,
                    parameters, true, virtualMethod.Line, virtualMethod.Column);
            }
        }

        // Анализ реализованного метода
        private void AnalyzeImplMethodDecl(ImplMethodDeclStmt implMethod, ClassInfo classInfo = null)
        {
            if (!IsValidType(implMethod.ReturnType, implMethod.Line, implMethod.Column) && implMethod.ReturnType != "void")
            {
                Diagnostics.Report(implMethod.Line, implMethod.Column,
                    $"Недопустимый возвращаемый тип '{implMethod.ReturnType}' для метода '{implMethod.Name}'");
                return;
            }

            var parameters = new List<(string, string)>();
            foreach (var param in implMethod.Parameters)
            {
                if (!IsValidType(param.Type, param.Line, param.Column))
                {
                    Diagnostics.Report(param.Line, param.Column,
                        $"Недопустимый тип '{param.Type}' для параметра '{param.Name}'");
                    return;
                }
                parameters.Add((param.Name, param.Type));
            }

            if (classInfo != null)
            {
                if (classInfo.Methods.ContainsKey(implMethod.Name))
                {
                    var existingMethod = classInfo.Methods[implMethod.Name];
                    if (!existingMethod.IsVirtual)
                    {
                        Diagnostics.Report(implMethod.Line, implMethod.Column,
                            $"Метод '{implMethod.Name}' уже объявлен как невиртуальный в классе '{classInfo.Name}'");
                        return;
                    }

                    if (existingMethod.ReturnType != implMethod.ReturnType ||
                        existingMethod.Parameters.Count != implMethod.Parameters.Count)
                    {
                        Diagnostics.Report(implMethod.Line, implMethod.Column,
                            $"Реализация метода '{implMethod.Name}' не соответствует объявлению: " +
                            $"разные возвращаемые типы или количество параметров");
                        return;
                    }

                    for (int i = 0; i < existingMethod.Parameters.Count; i++)
                    {
                        if (existingMethod.Parameters[i].Type != parameters[i].Item2)
                        {
                            Diagnostics.Report(implMethod.Line, implMethod.Column,
                                $"Реализация метода '{implMethod.Name}' не соответствует объявлению: " +
                                $"несовпадение типа параметра {i + 1}");
                            return;
                        }
                    }
                }

                classInfo.Methods[implMethod.Name] = new MethodInfo(implMethod.Name, implMethod.ReturnType,
                    parameters, false, implMethod.Line, implMethod.Column);
            }

            BeginScope();
            var currentScope = _variableScopes[_variableScopes.Count - 1];

            // Добавляем параметры метода в область видимости
            foreach (var param in implMethod.Parameters)
            {
                if (currentScope.ContainsKey(param.Name))
                {
                    Diagnostics.Report(param.Line, param.Column,
                        $"Параметр '{param.Name}' уже объявлен");
                    continue;
                }
                currentScope[param.Name] = new VariableInfo(param.Name, param.Type, param.Line, param.Column);
            }

            // Добавляем поля текущего класса и базовых классов в область видимости
            if (classInfo != null)
            {
                var allFields = GetAllFields(classInfo);
                foreach (var field in allFields)
                {
                    if (currentScope.ContainsKey(field.Key))
                    {
                        Diagnostics.Report(classInfo.Line, classInfo.Column,
                            $"Поле '{field.Key}' конфликтует с параметром или другой переменной");
                        continue;
                    }
                    currentScope[field.Key] = new VariableInfo(field.Key, field.Value, classInfo.Line, classInfo.Column);
                }
            }

            _currentFunctionReturnType = implMethod.ReturnType;
            AnalyzeSingleStmt(implMethod.Body);

            // Проверяем наличие return для методов с возвращаемым типом (не void)
            if (implMethod.ReturnType != "void" && !HasReturnStatement(implMethod.Body))
            {
                Diagnostics.Report(implMethod.Line, implMethod.Column,
                    $"Метод '{implMethod.Name}' с возвращаемым типом '{implMethod.ReturnType}' должен содержать оператор return");
            }

            _currentFunctionReturnType = null;
            EndScope();
        }

        // Проверка наличия return в теле метода
        private bool HasReturnStatement(StmtAst stmt)
        {
            if (stmt == null)
            {
                return false;
            }

            if (stmt is ReturnStmt)
            {
                return true;
            }

            if (stmt is BlockStmtAst blockStmt)
            {
                foreach (var s in blockStmt.Statements)
                {
                    if (HasReturnStatement(s))
                    {
                        return true;
                    }
                }
            }
            else if (stmt is IfStmtAst ifStmt)
            {
                return HasReturnStatement(ifStmt.ThenBody) || (ifStmt.ElseBody != null && HasReturnStatement(ifStmt.ElseBody));
            }

            return false;
        }

        // Анализ доступа к полю
        private void AnalyzeMemberAccessExpr(MemberAccessExprAst memberAccess)
        {
            AnalyzeExpression(memberAccess.Receiver);
            string receiverType = InferType(memberAccess.Receiver);
            if (receiverType == null || !_classes.ContainsKey(receiverType))
            {
                Diagnostics.Report(memberAccess.Line, memberAccess.Column,
                    $"Тип '{receiverType}' не является классом или не объявлен");
                return;
            }

            string fieldType = FindField(receiverType, memberAccess.Member);
            if (fieldType == null)
            {
                Diagnostics.Report(memberAccess.Line, memberAccess.Column,
                    $"Поле '{memberAccess.Member}' не найдено в классе '{receiverType}'");
            }
        }

        // Анализ доступа к методу
        private void AnalyzeMemberAccessStmt(MemberAccessStmtAst memberAccess)
        {
            AnalyzeExpression(memberAccess.Receiver);
            string receiverType = InferType(memberAccess.Receiver);
            if (receiverType == null || !_classes.ContainsKey(receiverType))
            {
                Diagnostics.Report(memberAccess.Line, memberAccess.Column,
                    $"Тип '{receiverType}' не является классом или не объявлен");
                return;
            }

            MethodInfo methodInfo = FindMethod(receiverType, memberAccess.Member);
            if (methodInfo == null)
            {
                Diagnostics.Report(memberAccess.Line, memberAccess.Column,
                    $"Метод '{memberAccess.Member}' не найден в классе '{receiverType}'");
            }
        }

        // Создание новой области видимости
        private void BeginScope()
        {
            _variableScopes.Add(new Dictionary<string, VariableInfo>());
        }

        // Удаление текущей области видимости
        private void EndScope()
        {
            if (_variableScopes.Count > 0)
            {
                _variableScopes.RemoveAt(_variableScopes.Count - 1);
            }
        }

        // Проверка, объявлена ли переменная
        private bool IsVariableDeclared(string name)
        {
            for (int i = _variableScopes.Count - 1; i >= 0; i--)
            {
                if (_variableScopes[i].ContainsKey(name))
                {
                    return true;
                }
            }
            return false;
        }

        // Получение типа переменной
        private string GetVariableType(string name)
        {
            for (int i = _variableScopes.Count - 1; i >= 0; i--)
            {
                if (_variableScopes[i].ContainsKey(name))
                {
                    return _variableScopes[i][name].Type;
                }
            }
            return null;
        }

        // Проверка допустимости типа
        private bool IsValidType(string type, int line, int column)
        {
            // Проверяем, что тип не null и соответствует одному из допустимых значений
            if (type == null)
            {
                Diagnostics.Report(line, column, "Тип не указан (null)");
                return false;
            }
            return type == "int" || type == "string" || type == "float" || _classes.ContainsKey(type) || type == "void";
        }

        // Поиск поля в классе (с учётом наследования)
        private string FindField(string className, string fieldName)
        {
            ClassInfo currentClass = _classes.ContainsKey(className) ? _classes[className] : null;
            while (currentClass != null)
            {
                if (currentClass.Fields.ContainsKey(fieldName))
                {
                    return currentClass.Fields[fieldName];
                }
                currentClass = currentClass.BaseClass != null && _classes.ContainsKey(currentClass.BaseClass)
                    ? _classes[currentClass.BaseClass]
                    : null;
            }
            return null;
        }

        // Проверка, является ли имя полем класса (с учётом наследования)
        private bool IsFieldInClass(ClassInfo classInfo, string fieldName)
        {
            return FindField(classInfo.Name, fieldName) != null;
        }

        // Получение всех полей класса, включая унаследованные
        private Dictionary<string, string> GetAllFields(ClassInfo classInfo)
        {
            var allFields = new Dictionary<string, string>();
            ClassInfo currentClass = classInfo;

            while (currentClass != null)
            {
                foreach (var field in currentClass.Fields)
                {
                    if (!allFields.ContainsKey(field.Key))
                    {
                        allFields[field.Key] = field.Value;
                    }
                }
                currentClass = currentClass.BaseClass != null && _classes.ContainsKey(currentClass.BaseClass)
                    ? _classes[currentClass.BaseClass]
                    : null;
            }

            return allFields;
        }

        // Поиск метода в классе (с учётом наследования)
        private MethodInfo FindMethod(string className, string methodName)
        {
            ClassInfo currentClass = _classes.ContainsKey(className) ? _classes[className] : null;
            while (currentClass != null)
            {
                if (currentClass.Methods.ContainsKey(methodName))
                {
                    return currentClass.Methods[methodName];
                }
                currentClass = currentClass.BaseClass != null && _classes.ContainsKey(currentClass.BaseClass)
                    ? _classes[currentClass.BaseClass]
                    : null;
            }
            return null;
        }

        // Определение типа выражения
        private string InferType(ExprAst expr)
        {
            if (expr == null)
            {
                return null;
            }

            switch (expr)
            {
                case IntExprAst _:
                    return "int";
                case FloatExprAst _:
                    return "float";
                case StringExprAst _:
                    return "string";
                case IdentifierExprAst idExpr:
                    for (int i = _variableScopes.Count - 1; i >= 0; i--)
                    {
                        if (_variableScopes[i].ContainsKey(idExpr.Name))
                        {
                            return _variableScopes[i][idExpr.Name].Type;
                        }
                    }
                    // Проверяем, является ли идентификатор полем текущего класса
                    if (_currentClass != null && IsFieldInClass(_currentClass, idExpr.Name))
                    {
                        return FindField(_currentClass.Name, idExpr.Name);
                    }
                    Diagnostics.Report(idExpr.Line, idExpr.Column,
                        $"Переменная '{idExpr.Name}' не объявлена");
                    return null;
                case BinaryExprAst binaryExpr:
                    // Анализируем левый и правый операнды
                    string leftType = InferType(binaryExpr.Left);
                    string rightType = InferType(binaryExpr.Right);
                    if (leftType == null || rightType == null)
                    {
                        return null;
                    }

                    // Операторы сравнения (<, >, <=, >=, ==, !=) возвращают bool
                    if (binaryExpr.Operator == "<" || binaryExpr.Operator == ">" ||
                        binaryExpr.Operator == "<=" || binaryExpr.Operator == ">=" ||
                        binaryExpr.Operator == "==" || binaryExpr.Operator == "!=")
                    {
                        // Проверяем, что операнды совместимы для сравнения
                        if (!AreTypesComparable(leftType, rightType))
                        {
                            Diagnostics.Report(binaryExpr.Line, binaryExpr.Column,
                                $"Нельзя сравнивать типы '{leftType}' и '{rightType}'");
                            return null;
                        }
                        return "bool"; // Сравнение возвращает bool
                    }

                    // Арифметические операторы (+, -, *, /) возвращают тот же тип, что у операндов
                    if (binaryExpr.Operator == "+" || binaryExpr.Operator == "-" ||
                        binaryExpr.Operator == "*" || binaryExpr.Operator == "/")
                    {
                        if (leftType == rightType && (leftType == "int" || leftType == "float"))
                        {
                            return leftType;
                        }
                        if (leftType == "string" && rightType == "string" && binaryExpr.Operator == "+")
                        {
                            return "string"; // Конкатенация строк
                        }
                        Diagnostics.Report(binaryExpr.Line, binaryExpr.Column,
                            $"Несовместимые типы операндов для оператора '{binaryExpr.Operator}': '{leftType}' и '{rightType}'");
                        return null;
                    }

                    Diagnostics.Report(binaryExpr.Line, binaryExpr.Column,
                        $"Неизвестный оператор '{binaryExpr.Operator}'");
                    return null;
                // Остальные случаи опущены для краткости
                default:
                    return null;
            }
        }

        // Определение типа оператора (для AssignStmtAst)
        private string InferStmtType(StmtAst stmt)
        {
            if (stmt == null)
            {
                return null;
            }

            if (stmt is NewStmt newStmt)
            {
                return newStmt.ClassName;
            }
            else if (stmt is AssignExprAst assignExpr)
            {
                return InferType(assignExpr.Value);
            }
            return null;
        }

        // Проверка, можно ли сравнивать типы
        private bool AreTypesComparable(string type1, string type2)
        {
            // Можно сравнивать int, float, string, или одинаковые классы
            if (type1 == type2)
            {
                return true;
            }
            if ((type1 == "int" && type2 == "float") || (type1 == "float" && type2 == "int"))
            {
                return true;
            }
            return false;
        }

        // Проверка совместимости типов
        private bool AreTypesCompatible(string targetType, string sourceType)
        {
            if (targetType == sourceType)
            {
                return true;
            }
            // Проверяем совместимость для наследования
            if (_classes.ContainsKey(sourceType) && _classes.ContainsKey(targetType))
            {
                ClassInfo currentClass = _classes[sourceType];
                while (currentClass != null)
                {
                    if (currentClass.Name == targetType)
                    {
                        return true;
                    }
                    currentClass = currentClass.BaseClass != null && _classes.ContainsKey(currentClass.BaseClass)
                        ? _classes[currentClass.BaseClass]
                        : null;
                }
            }
            return false;
        }
    }
}
