using NETCore.Basic.Domain.Interfaces;
using System.IO;

namespace NETCore.Basic.Util.Helper
{
    public interface ILocalFileHandler : IFileHandler<Stream>
    {
        bool Read(string path, out Stream file);
        bool Write(Stream file, string path, string fileName);
        bool Delete(string path);
        bool Delete(FileInfo file);
        bool DeleteFolder(string path);
        byte[] ByteArrayFromFile(Stream stream);
        Stream StreamFromBytes(byte[] bytes);

    }
    public class FileHandler : ILocalFileHandler
    {
        public bool DeleteFolder(string path)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                DeleteInnerFolder(directoryInfo);

                return true;
            }
            catch
            {
                return false;
            }
        }
        private void DeleteInnerFolder(DirectoryInfo directory)
        {
            if (directory == null) return;

            DeleteFolderFiles(directory);

            foreach (var dir in directory.EnumerateDirectories())
            {
                DeleteInnerFolder(dir);
            }
            directory.Delete();
        }
        private void DeleteFolderFiles(DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.EnumerateFiles())
            {
                file.Delete();
            }
        }
        public bool Delete(string path)
        {
            try
            {
                FileInfo file = new FileInfo(path);

                file.Delete();

                return !Read(path, out Stream outFile);
            }
            catch
            {
                return false;
            }
        }
        public bool Read(string path, out Stream file)
        {
            file = null;
            bool existeArquivo = File.Exists(path);
            if (!existeArquivo) return false;

            try
            {
                var bytes = File.ReadAllBytes(path);
                file = new MemoryStream(bytes);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Write(Stream file, string path, string fileName)
        {
            try
            {
                var bytes = ByteArrayFromFile(file);
                File.WriteAllBytes($@"{path}\\{fileName}", bytes);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public byte[] ByteArrayFromFile(Stream stream)
        {
            using (MemoryStream file = new MemoryStream())
            {
                stream.CopyTo(file);
                return file.ToArray();
            }
        }
        public Stream StreamFromBytes(byte[] bytes) => new MemoryStream(bytes);
        public bool Delete(FileInfo file)
        {
            try
            {
                file.Delete();
                return !Read(file.FullName, out Stream outFile);
            }
            catch
            {
                return false;
            }
        }
    }
}
