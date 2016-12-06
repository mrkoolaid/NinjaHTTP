using NinjaHTTP.Infos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaHTTP
{
    public class Client
    {
        public bool Connected { get { return _socket.Connected; } }

        private BaseSocket _socket;
        private RequestInfo _request;

        public Client(RequestInfo request)
        {
            _request = request;
        }

        public void Connect()
        {
            _socket = new BaseSocket(_request.Uri.Host, _request.Uri.Port);
            _socket.Timeout = _request.Timeout;

            _socket.Connect();
        }

        public ResponseInfo GetResponse()
        {
            SendRequest();

            string response = string.Empty;
            if (!ReadResponse(out response))
                return null;

            return null;
        }

        private string BuildRequest()
        {
            StringBuilder builder = new StringBuilder();

            try
            {
                builder.AppendLine($"{_request.Method.ToString()} {_request.Uri.PathAndQuery} HTTP/1.1");
                builder.AppendLine($"Host: {_request.Uri.Host}");

                foreach (string name in _request.Headers)
                {
                    if (name.ToLower() == "host")
                        continue;

                    string value = _request.Headers[name];
                    builder.AppendLine($"{name}: {value}");
                }

                builder.AppendLine();

                if (_request.Method == HttpMethod.POST && !string.IsNullOrEmpty(_request.Postdata))
                {
                    builder.AppendLine(_request.Postdata);
                    builder.AppendLine();
                }

                return builder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                builder.Clear();
                builder = null;
            }
        }

        private bool SendRequest()
        {
            string request = BuildRequest();
            byte[] buffer = Encoding.UTF8.GetBytes(request);

            return _socket.Send(buffer);
        }

        private string ReadResponse()
        {
            StringBuilder builder = new StringBuilder();
            byte[] buffer = new byte[1024];

            try
            {
                while (true)
                {
                    if (!_socket.Receive(out buffer))
                        return string.Empty;
                    
                    if (buffer.Length < 1024)
                        break;

                    string chunk = Encoding.ASCII.GetString(buffer);
                    if (chunk.EndsWith("\r\n\r\n"))
                        break;

                    builder.Append(chunk);

                    buffer = null;
                }

                return builder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                buffer = null;
                builder.Clear();
                builder = null;
            }

            return string.Empty;
        }
    }
}
