using NETCore.Basic.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NETCore.Basic.Util.Helper
{
    public interface ILocalFileHandler : IFileHandler<Stream>
    {
        bool Delete(string path);
        bool Delete(FileInfo file);
        bool DeleteFolder(string path);

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
                //using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                //{
                //    byte[] bytes = new byte[file.Length];
                //    file.Read(bytes, 0, (int)file.Length);
                //    file.Write(bytes, 0, bytes.Length);
                //}
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

                //using (FileStream fileStream = new FileStream($@"{path}\\{fileName}", FileMode.Create))
                //{
                //    file.CopyTo(fileStream);
                //}

                return true;
            }
            catch
            {
                return false;
            }
        }

        private byte[] ByteArrayFromFile(Stream stream)
        {
            //MemoryStream ms = new MemoryStream();

            //var tempStream = stream as MemoryStream;
            //var byteArray = tempStream.ToArray();
            //ms.Write(byteArray, 0, byteArray.Length);

            //return ms.ToArray();

            using (MemoryStream file = new MemoryStream())
            {
                stream.CopyTo(file);
                return file.ToArray();
            }
        }
        private static void StreamFromBytes(byte[] bytes, Stream s)
        {
            using (var writer = new BinaryWriter(s))
            {
                writer.Write(bytes);
            }
        }
        private static Stream StreamFromBytes(byte[] bytes) => new MemoryStream(bytes);
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
