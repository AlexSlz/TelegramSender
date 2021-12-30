using System;
using System.Linq;
using System.Threading.Tasks;

namespace tgSender
{
    internal class Commands
    {
        string Name;
        Func<string[], Task<string>> make;

        public Commands(string _name, Func<string[], Task<string>> _action)
        {
            Name = _name;
            make = _action;
        }
        static string stringParse(string data)
        {
            string[] dataArray = data.Replace('/', '$').Split('$');
            string result = "";
            foreach (string item in dataArray)
            {
                if (int.TryParse(item, out int value))
                    result += (char)value;
                else
                    result += item;
            }
            return result;
        }
        public static void Parse(string text, Commands[] _commands)
        {
            string[] commandFromServer = text.Split(new string[] { "//" }, StringSplitOptions.None);
            commandFromServer = commandFromServer[1].Split('/');
            foreach (string temp in commandFromServer)
            {
                if (!string.IsNullOrEmpty(temp))
                {
                    string[] tmpText = temp.Split('=');
                    string tempCommandName = tmpText[0];
                    string[] tempCommandArgs = null;
                    if (tmpText.Length > 1)
                    tempCommandArgs = tmpText[1].Contains(',') ? tmpText[1].Split(',') : new string[] { tmpText[1] };
                    Console.WriteLine(tempCommandName);
                    for (int i = 0; i < tempCommandArgs.Length; i++)
                    {
                        tempCommandArgs[i] = stringParse(tempCommandArgs[i]);
                        Console.WriteLine("- " + tempCommandArgs[i]);
                    }
                    foreach (Commands command in _commands)
                    {
                        if (command.Name == tempCommandName)
                        {
                            var foo = command.make(tempCommandArgs);
                                foo.Wait();
                        }
                    }
                }
            }
            Socket.SendData();
        }
    }
}
