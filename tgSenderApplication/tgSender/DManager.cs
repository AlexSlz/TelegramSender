using System;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace tgSender
{
    internal class DManager
    {
        //mime type
        static FilesType[] filesTypes = {
            new FilesType("jpg", "image/jpeg"),
            new FilesType("jpeg", "image/jpeg"),
            new FilesType("png", "image/png"),
            new FilesType("mp4", "video/mp4"),
            new FilesType("mp3", "audio/mpeg"),
            new FilesType("gif", "image/gif"),
        };
        static WebClient client = new WebClient();
        static string path = AppDomain.CurrentDomain.BaseDirectory;
        static string CreateFolder(string name)
        {
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Downloads");
            Directory.CreateDirectory(path + "\\Downloads\\" + name);
            return "\\Downloads\\" + name;
        }
        static string url;
        static string folderName;
        static int countFiles;
        public static string link;
        public async static Task<string> Download(string[] param)
        {
            link = param[1];
            url = param[0];
            folderName = param[1];
            folderName = folderName.Remove(0, "https://".Length).Split('/')[0];
            folderName = CreateFolder(folderName);
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            countFiles = Directory.GetFiles(path + $"\\{folderName}", "*", SearchOption.AllDirectories).Length + 1;
            try
            {
                var foo = client.DownloadFileTaskAsync(new Uri(url), path + $"{folderName}\\{countFiles}.{getType(url).name}");
                foo.Wait();
            }
            catch (Exception ex)
            {
                return "NOT WORKiNG";
            }
            return "OK";
        }
        public static string[] getTypeAndName()
        {
            if (folderName != null)
                return new[] { folderName, countFiles + "." + getType(url).name, getType(url).MIME };
            else
                return null;
        }
        static FilesType getType(string url)
        {
            foreach (FilesType type in filesTypes)
            {
                if (url.Contains(type.name))
                {
                    return type;
                }
            }
            return null;
        }
    }
}
