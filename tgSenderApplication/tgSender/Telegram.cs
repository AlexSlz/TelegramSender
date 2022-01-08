using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TeleSharp.TL;
using TeleSharp.TL.Contacts;
using TeleSharp.TL.Messages;
using TLSharp.Core;
using TLSharp.Core.Utils;

namespace tgSender
{
    internal class Telegram
    {
        static int api_id = 1236222;
        static string api_hash = "949f0e81ff8d83c75e2d8d58b460ba63";
        static TelegramClient client;
        static TLContacts tLContact;
        static string hash;
        static string path = AppDomain.CurrentDomain.BaseDirectory;
        public async static Task<string> Auth(string[] param = null)
        {
            FileSessionStore store = new FileSessionStore(new DirectoryInfo(path + "\\bin"));
            
            client = new TelegramClient(api_id, api_hash, store, param[0]);
            await client.ConnectAsync();
            if (!client.IsUserAuthorized())
            {
                hash = await client.SendCodeRequestAsync(param[0]);
                Console.WriteLine("Enter Code:");
                await client.MakeAuthAsync(param[0], hash, Console.ReadLine());
            }
            return "OK";
        }
        public async static Task<string> SendFile(string[] param)
        {
            TLInputFile fileResult = null;
            string[] file = null;
            try
            {
                file = DManager.getTypeAndName();
                fileResult = (TLInputFile)await client.UploadFile(file[1], new StreamReader(path + file[0] + "\\" + file[1]));
                await client.SendUploadedDocument(new TLInputPeerUser() { UserId = int.Parse(param[0]), AccessHash = long.Parse(param[1]) }, fileResult, "", file[2], new TLVector<TLAbsDocumentAttribute>());
            }
            catch (Exception)
            {
                try
                {
                    await client.SendUploadedDocument(new TLInputPeerChannel() { ChannelId = int.Parse(param[0]), AccessHash = long.Parse(param[1]) }, fileResult, "", file[2], new TLVector<TLAbsDocumentAttribute>());
                }
                catch (Exception)
                {
                    try
                    {
                        await client.SendUploadedDocument(new TLInputPeerChat() { ChatId = int.Parse(param[0]) }, fileResult, "", file[2], new TLVector<TLAbsDocumentAttribute>());
                    }
                    catch (Exception)
                    {
                        await SendMsg(new string[] { param[0], param[1], DManager.link });
                    }
                }
            }
            return "OK";
        }

        async static Task<string> SendMsg(string[] param)
        {
            try
            {
                await client.SendMessageAsync(new TLInputPeerUser() { UserId = int.Parse(param[0]), AccessHash = long.Parse(param[1]) }, param[2]);
            }
            catch (Exception)
            {
                try
                {
                    await client.SendMessageAsync(new TLInputPeerChannel() { ChannelId = int.Parse(param[0]), AccessHash = long.Parse(param[1]) }, param[2]);
                }
                catch(Exception)
                {
                    await client.SendMessageAsync(new TLInputPeerChat() { ChatId = int.Parse(param[0]) }, param[2]);
                }
            }
            return "OK";
        }

        public async static Task<string> GetChannel(string[] param)
        {
            string jsonData = "{\"channel\":[";
            var dialogs = (TLDialogsSlice)await client.GetUserDialogsAsync();
            var channels = dialogs.Chats.ToList().OfType<TLChannel>();
            foreach (TLChannel item in channels)
            {
                bool contains = false;
                if (param != null)
                {
                    foreach (var par in param)
                    {
                        if (CheckTwin(par, item.Title, item.Username))
                        {
                            contains = true;
                            param.Where(val => val != par).ToArray();
                            break;
                        }
                    }
                }
                else { contains = true; }
                if (contains)
                    jsonData += "{\"title\":\"" + item.Title + "\", \"AccessHash\":\"" + item.AccessHash + "\", \"id\":\"" + item.Id + "\"},";
            }
            var chat = dialogs.Chats.ToList().OfType<TLChat>();
            foreach (TLChat item in chat)
            {
                bool contains = false;
                if (param != null)
                {
                    foreach (var par in param)
                    {
                        if (CheckTwin(par, item.Title))
                        {
                            contains = true;
                            param.Where(val => val != par).ToArray();
                            break;
                        }
                    }
                }
                else { contains = true; }
                if (contains)
                    jsonData += "{\"title\":\"" + item.Title + "\", \"id\":\"" + item.Id + "\"},";
            }
            jsonData = (jsonData.Contains(',') ? jsonData.Remove(jsonData.Length - 1, 1) : jsonData) + "]},";
            Socket.AddJson(jsonData);
            //await client.SendMessageAsync(new TLInputPeerChannel() { ChannelId = 1319689979, AccessHash = 8028918482939018018 }, "aboba");
            //Console.ReadLine();
            return "search";
        }
        public async static Task<string> GetContact(string[] param)
        {
            tLContact = await client.GetContactsAsync();
            string jsonData = "{\"contact\":[";
            foreach (TLUser user in tLContact.Users)
            {
                bool contains = false;
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        if(CheckTwin(item, user.FirstName, user.LastName, user.Username))
                        {
                            contains = true;
                            param.Where(val => val != item).ToArray();
                            break;
                        }
                    }
                }
                else { contains = true; }
                if(contains)
                jsonData += "{\"fullName\":\"" + user.FirstName + " " + user.LastName + "\", \"AccessHash\":\"" + user.AccessHash + "\", \"id\":\"" + user.Id + "\"},";

            }
            jsonData = (jsonData.Contains(',') ? jsonData.Remove(jsonData.Length - 1, 1) : jsonData) + "]},";
            Socket.AddJson(jsonData);
            return "OK";
        }

        static bool CheckTwin(string item, params string[] param)
        {
            foreach(string i in param)
            {
                if (NullAndContains(i, item))
                {
                    return true;
                }
            }
            return false;
        }

        static bool NullAndContains(string item, string param)
        {

            if (!string.IsNullOrEmpty(item))
            {
                if (item.Contains(param))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
