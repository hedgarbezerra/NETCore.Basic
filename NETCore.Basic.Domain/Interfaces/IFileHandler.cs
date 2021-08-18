﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NETCore.Basic.Domain.Interfaces
{    
    public interface IFileHandler<T> where T: class
    {
        bool Read(string path, out T file);
        bool Write(T file, string path);
    }
}
