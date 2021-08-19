using HtmlAgilityPack;
using NETCore.Basic.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NETCore.Basic.Util.Helper
{
    public interface IHTMLHandler : IFileHandler<HtmlDocument>
    {

    }
    public class HTMLHandler : IHTMLHandler
    {
        public bool Read(string path, out HtmlDocument html)
        {
            bool existeArquivo = File.Exists(path);
            html = new HtmlDocument();
            if (existeArquivo)
            {
                html.Load(path, Encoding.UTF8);

                return true;
            }
            return false;
        }

        public bool Write(HtmlDocument html, string path, string fileName)
        {
            string fullPath = $@"{path}\{fileName}";
            bool existeArquivo = File.Exists(fullPath);

            if (existeArquivo) return false;

            html.Save(fullPath);

            return true;
        }
    }
}
