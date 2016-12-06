using System;
using System.Net;

namespace NinjaHTTP
{
    public sealed class RequestInfo
    {
        public HttpMethod Method { get; }
        public Uri Uri { get; }
        public WebHeaderCollection Headers { get; }
        public int Timeout { get; } = 10000;
        public string Postdata { get; }
        
        public RequestInfo(string url, WebHeaderCollection headers, string postdata)
        {
            Uri uri = new Uri(url);
            Headers = headers;
            Postdata = postdata;
        }
    }
}