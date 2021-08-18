using NETCore.Basic.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NETCore.Basic.Util.Helper
{
    public interface ILocalFileHandler : IFileHandler<Stream>
    {
        bool Delete(string path);
        bool DeleteFolder(string path);

    }
    public class FileHandler : ILocalFileHandler
    {
        public bool DeleteFolder(string path)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);

                foreach (FileInfo file in directoryInfo.EnumerateFiles())
                {
                    file.Delete();
                }

                return Read(path, out Stream outFile);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(string path)
        {
            try
            {

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Read(string path, out Stream file)
        {
            file = new MemoryStream();
            try
            {

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Write(Stream file, string path)
        {
            try
            {

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private byte[] ByteArrayFromStream(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
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

    }
}
