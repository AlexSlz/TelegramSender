using Microsoft.Win32;
using System;

namespace tgSender
{
    internal class Program
    {

        static Commands[] cmd = { 
            new Commands("auth", Telegram.Auth), 
            new Commands("getcontact", Telegram.GetContact), 
            new Commands("getchannel", Telegram.GetChannel),
            new Commands("downloadImg", DManager.Download),
            new Commands("sendFile", Telegram.SendFile)};
        static void Main(string[] args)
        {
            Console.Title = "AlexSLZ";
            Registry.ClassesRoot.CreateSubKey(@"tgSender").SetValue("", "URL:tgSender Protocol");
            Registry.ClassesRoot.CreateSubKey(@"tgSender").SetValue("URL Protocol", "");
            Registry.ClassesRoot.CreateSubKey(@"tgSender\Shell\Open\Command").SetValue("", "\"" + AppDomain.CurrentDomain.BaseDirectory + "tgSender.exe\" \"%1\"");
            if (args.Length > 0)
            {
                Socket.Server();
                Commands.Parse(args[0], cmd);
            }
        }
    }
}
