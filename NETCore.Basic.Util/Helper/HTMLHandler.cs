using NETCore.Basic.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NETCore.Basic.Util.Helper
{
    public interface IHTMLHandler : IFileHandler<string>
    {

    }
    public class HTMLHandler : IHTMLHandler
    {
        public bool Read(string path, out string html)
        {
            var existeArquivo = File.Exists(path);
            html = "";
            if (existeArquivo)
            {

                if (existeArquivo)
                    html = File.ReadAllText(path);

                return true;
            }
            return false;
        }

        public bool Write(string html, string path)
        {
            throw new NotImplementedException();
        }
    }
}
