using System;
using System.Collections.Generic;

namespace Demo
{
    public partial class Parser
    {
        // Парсит список аргументов функции print и возвращает список узлов AST
        // Обрабатывает ошибки, связанные с запятыми и скобками
        private List<ExprAst> ParseArgumentList()
        {
            var arguments = new List<ExprAst>();

            // Если сразу встретили ')', возвращаем пустой список
            if (Match(TokenKind.CloseParen))
            {
                Advance(); // Съедаем ')'
                return arguments;
            }

            // Парсим первый аргумент
            var arg = ParseExpressionAst();
            if (arg != null)
            {
                arguments.Add(arg);
            }
            else
            {
                // Если первый аргумент некорректен, синхронизируемся
                SynchronizeTo(TokenKind.Comma, TokenKind.CloseParen, TokenKind.Semicolon, TokenKind.EndOfFile);
                if (Match(TokenKind.CloseParen))
                {
                    Advance(); // Съедаем ')'
                    return arguments;
                }
                else if (Match(TokenKind.Semicolon) || Match(TokenKind.EndOfFile))
                {
                    return arguments; // Выходим, если встретили ';' или конец файла
                }
            }

            // Продолжаем парсить аргументы, разделённые запятыми
            while (true)
            {
                if (Match(TokenKind.Comma))
                {
                    Advance(); // Съедаем запятую
                    arg = ParseExpressionAst();
                    if (arg != null)
                    {
                        arguments.Add(arg);
                    }
                    else
                    {
                        // Если аргумент после запятой некорректен, синхронизируемся
                        SynchronizeTo(TokenKind.Comma, TokenKind.CloseParen, TokenKind.Semicolon, TokenKind.EndOfFile);
                        if (Match(TokenKind.CloseParen))
                        {
                            break; // Выходим, если встретили ')'
                        }
                        else if (Match(TokenKind.Semicolon) || Match(TokenKind.EndOfFile))
                        {
                            return arguments; // Выходим, если встретили ';' или конец файла
                        }
                        // Если встретили запятую, продолжаем парсить следующий аргумент
                        if (Match(TokenKind.Comma))
                        {
                            Advance();
                            arg = ParseExpressionAst();
                            if (arg != null)
                            {
                                arguments.Add(arg);
                            }
                            continue;
                        }
                    }
                }
                else if (Match(TokenKind.CloseParen))
                {
                    break; // Нормальное завершение списка аргументов
                }
                else
                {
                    // Ошибка: ожидалась запятая или закрывающая скобка
                    Error("Ожидалась ',' или ')'");
                    SynchronizeTo(TokenKind.Comma, TokenKind.CloseParen, TokenKind.Semicolon, TokenKind.EndOfFile);
                    if (Match(TokenKind.CloseParen))
                    {
                        break; // Выходим, если встретили ')'
                    }
                    else if (Match(TokenKind.Semicolon) || Match(TokenKind.EndOfFile))
                    {
                        return arguments; // Выходим, если встретили ';' или конец файла
                    }
                    else if (Match(TokenKind.Comma))
                    {
                        Advance(); // Пропускаем запятую и продолжаем
                        arg = ParseExpressionAst();
                        if (arg != null)
                        {
                            arguments.Add(arg);
                        }
                        continue;
                    }
                    else
                    {
                        return arguments; // Если ничего не подходит, выходим
                    }
                }
            }

            // Проверяем и съедаем закрывающую скобку
            if (!Expect(TokenKind.CloseParen, "Ожидалась ')'"))
            {
                SynchronizeTo(TokenKind.Semicolon, TokenKind.EndOfFile);
            }

            return arguments;
        }
    }
}