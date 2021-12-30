using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tgSender
{
    internal class FilesType
    {
        public string name;
        public string MIME;
        public FilesType(string _name, string _mime)
        {
            name = _name;
            MIME = _mime;
        }
    }
}
