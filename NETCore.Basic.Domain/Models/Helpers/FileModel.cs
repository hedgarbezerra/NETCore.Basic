using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NETCore.Basic.Domain.Models.Helpers
{
    public class FileModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public Stream Content { get; set; }
    }
}
