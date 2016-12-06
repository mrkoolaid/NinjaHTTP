using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace NinjaHTTP
{
    internal class BaseSocket
    {
        public bool Connected { get { return _socket.Connected; } }
        public int Timeout { get; set; } = 10000;

        protected string _targetHost;
        protected int _targetPort;

        private Socket _socket;

        public BaseSocket(string host, int port)
        {
            _targetHost = host;
            _targetPort = port;
            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect()
        {
            IPAddress address;
            if (!IPAddress.TryParse(_targetHost, out address))
                address = Dns.GetHostAddresses(_targetHost).FirstOrDefault();

            IPEndPoint endPoint = new IPEndPoint(address, _targetPort);
            IAsyncResult result = _socket.BeginConnect(endPoint, null, null);
            result.AsyncWaitHandle.WaitOne(Timeout);
            _socket.EndConnect(result);
        }

        public bool Send(IEnumerable<byte> bytes)
        {
            if (_socket.Connected)
            {
                byte[] buffer = bytes.ToArray();
                IAsyncResult result = _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, null, null);
                result.AsyncWaitHandle.WaitOne(Timeout);
                _socket.EndSend(result);

                return result.IsCompleted;
            }
            else
                return false;
        }

        public bool Receive(out byte[] buffer)
        {
            if (_socket.Connected)
            {
                buffer = new byte[1024];
                IAsyncResult result = _socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, null, null);
                result.AsyncWaitHandle.WaitOne(Timeout);
                _socket.EndReceive(result);

                return result.IsCompleted;
            }
            else
            {
                buffer = null;
                return false;
            }
        }
    }
}