using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NETCore.Basic.Domain.Models
{
    public class TransaferenceFile
    {
        public TransaferenceFile()
        {

        }
        public TransaferenceFile(string name, string type, Stream content)
        {
            Name = name;
            Type = type;
            Content = content;
        }
        public string Name { get; set; }
        public string Type { get; set; }
        public Stream Content { get; set; }
    }
}
