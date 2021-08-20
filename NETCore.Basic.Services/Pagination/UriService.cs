using Microsoft.AspNetCore.WebUtilities;
using NETCore.Basic.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Services.Pagination
{
    public interface IUriService
    {
        public Uri GetPageUri(int pageIndex, int pageSize,  string route);
    }
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        public Uri GetPageUri(int pageIndex, int pageSize, string route)
        {
            var _endpointUri = new Uri(string.Concat(_baseUri, route));
            var modifiedUri = QueryHelpers.AddQueryString(_endpointUri.ToString(), "pageIndex", pageIndex.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", pageSize.ToString());
            return new Uri(modifiedUri);
        }
    }
}
