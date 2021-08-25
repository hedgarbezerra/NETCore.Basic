using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Domain.Models
{
    public enum Method
    {
        GET,
        POST,
        PUT,
        DELETE
    }
    public class HATEOASLink
    {
        public HATEOASLink(string rel, Uri href, Method method)
        {
            Rel = rel;
            Href = href;
            Method = method;
        }
        public string Rel { get; set; }
        public Uri Href { get; set; }
        public Method Method { get; set; }
    }

    public class HATEOASResult<T> where T:class
    {
        public T Data { get; set; }
        public List<HATEOASLink> Links { get; set; }
        public HATEOASResult()
        {
            Links = new List<HATEOASLink>();
        }
        public HATEOASResult(T data, List<HATEOASLink> links)
        {
            Data = data;
            Links = links;
        }
        public HATEOASResult(T data) :this()
        {
            Data = data;
        }
    }
}
