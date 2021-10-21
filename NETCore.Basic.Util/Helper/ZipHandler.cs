using NETCore.Basic.Domain.Interfaces;
using NETCore.Basic.Domain.Models.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace NETCore.Basic.Util.Helper
{

    public class ZipHandler : IZipHandler
    {
        private readonly ILocalFileHandler _fileHandler;
        public ZipHandler(ILocalFileHandler fileHandler)
        {
            _fileHandler = fileHandler;
        }

        public bool Read(string path, out ZipArchive file)
        {
            file = null;
            var foundZip = _fileHandler.Read(path, out Stream zipFile);

            if (!foundZip)
                return false;

            file = new ZipArchive(zipFile, ZipArchiveMode.Read, true);

            return true;
        }

        public bool Write(List<FileModel> files, string path, string fileName)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (ZipArchive archive = new ZipArchive(ms, ZipArchiveMode.Update))
                    {
                        foreach (var file in files)
                        {
                            ZipArchiveEntry orderEntry = archive.CreateEntry(file.Name);
                            using (BinaryWriter writer = new BinaryWriter(orderEntry.Open()))
                            {
                                writer.Write(_fileHandler.ByteArrayFromFile(file.Content));
                            }
                        }
                    }

                    _fileHandler.Write(ms, path, fileName);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
