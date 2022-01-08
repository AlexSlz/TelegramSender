using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tgSender
{
    internal class ConsoleManipulation
    {
        string commandText = "Command Status: ";
        char[] Animation = { '|', '/', '-', '\\' };
        Func<string[], Task<string>> func;
        string[] tempCommandArgs;
        public ConsoleManipulation(Func<string[], Task<string>> _action, string[] _tempCommandArgs)
        {
            func = _action;
            tempCommandArgs = _tempCommandArgs;
        }
        public void Wait()
        {
            Thread thread = new Thread(Anim);
            thread.Start();
            var foo = func(tempCommandArgs);
            foo.Wait();
            thread.Abort();
            //Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            Console.Write($"{foo.Result}\n");
        }
        public void Anim()
        {
            for (int i = 0; i < Animation.Length + 1; i++)
            {
                if (i == Animation.Length)
                {
                    i = 0;
                }
                Console.Write($"{commandText}{Animation[i]}");
                Thread.Sleep(100);
                Console.SetCursorPosition(Console.CursorLeft - commandText.Length - 1, Console.CursorTop);
            }
        }
    }
}
