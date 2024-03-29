﻿using NETCore.Basic.Domain.Models.Helpers;
using System.Collections.Generic;
using System.IO.Compression;

namespace NETCore.Basic.Domain.Interfaces
{
    public interface IZipHandler
    {
        bool Read(string path, out ZipArchive file);
        bool Write(List<FileModel> files, string path, string fileName);
    }
}
