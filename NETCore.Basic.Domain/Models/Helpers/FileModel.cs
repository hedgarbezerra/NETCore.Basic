using System.IO;

namespace NETCore.Basic.Domain.Models.Helpers
{
    public class FileModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public Stream Content { get; set; }
    }
}
