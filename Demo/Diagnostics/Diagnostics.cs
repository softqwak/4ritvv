using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Diagnostics
{
    public class Diagnostics
    {
        private readonly List<string> _messages = new List<string>();

        public void Report(int position, string message)
        {
            _messages.Add(string.Format("[Error at {0}]: {1}", position, message));
        }

        public void PrintAll()
        {
            foreach (string msg in _messages)
            {
                Console.WriteLine(msg);
            }
        }
    }
}
