using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public class Diagnostics
    {
        public static List<string> _messagesError = new List<string>();
        public static List<string> _messagesWarning = new List<string>();
        public static List<(int, int)> _markErrors = new List<(int, int)>();
        public static List<(int, int)> _markWarnings = new List<(int, int)>();
        public static void Report(int line, int column, string message)
        {
            _markErrors.Add((line, column));
            _messagesError.Add(string.Format("[Ошибка на {0} строке, {1} колонке]: {2}", line, column, message));
        }

        public static void Warning(int line, int column, string message)
        {
            _markWarnings.Add((line, column));
            _messagesWarning.Add(string.Format("[Предупреждение на {0} строке, {1} колонке]: {2}", line, column, message));
        }

    }
}
