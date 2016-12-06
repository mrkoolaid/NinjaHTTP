using System.Collections.Generic;
using System.Net;

namespace NinjaHTTP.Infos
{
    public sealed class ResponseInfo
    {
        public HttpStatusCode StatusCode { get; }
        public WebHeaderCollection Headers { get; }
        public List<byte> Body { get; }

        public ResponseInfo(HttpStatusCode statusCode, WebHeaderCollection headers, IEnumerable<byte> body)
        {
            StatusCode = HttpStatusCode.OK;
            Headers = new WebHeaderCollection();

            Body = new List<byte>(body);
        }
    }
}