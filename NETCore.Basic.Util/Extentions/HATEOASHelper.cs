using NETCore.Basic.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Util.Extentions
{
    public static class HATEOASHelper
    {
        public static void AddLink<T>(this HATEOASResult<T> result, string rel, Uri href, Method method) where T: class
        {
            result.Links.Add(new HATEOASLink(rel, href, method));
        }
    }
}
