using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NETCore.Basic.Domain.Models.Mailing
{
    public class MailFile
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Stream Content { get; set; }
    }
}
