using Microsoft.Win32;
using System;
using System.Threading.Tasks;

namespace tgSender
{
    internal class Program
    {

        static Commands[] cmd = { 
            new Commands("auth", Telegram.Auth), 
            new Commands("getcontact", Telegram.GetContact), 
            new Commands("getchannel", Telegram.GetChannel),
            new Commands("download", DManager.Download),
            new Commands("sendFile", Telegram.SendFile),
            new Commands("delete", DeleteRegEdit),
            new Commands("checkproto", Socket.SendBack)};
        static void Main(string[] args)
        {
            Console.Title = "AlexSLZ";
            InstallRegEdit();
            if (args.Length > 0)
            {
                Socket.Server();
                Commands.Parse(args[0], cmd);
            }
        }
        static void InstallRegEdit()
        {
            Registry.ClassesRoot.CreateSubKey(@"tgSender").SetValue("", "URL:tgSender Protocol");
            Registry.ClassesRoot.CreateSubKey(@"tgSender").SetValue("URL Protocol", "");
            Registry.ClassesRoot.CreateSubKey(@"tgSender\Shell\Open\Command").SetValue("", "\"" + AppDomain.CurrentDomain.BaseDirectory + "tgSender.exe\" \"%1\"");
        }
        async static Task<string> DeleteRegEdit(string[] param)
        {
            Registry.ClassesRoot.DeleteSubKeyTree(@"tgSender");
            return "OK";
        }
    }
}
