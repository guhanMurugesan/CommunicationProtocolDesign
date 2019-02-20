using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace N_Server
{
    public class UDPPort
    {
        Socket _socket;
        byte[] _TransmitBuffer = new byte[512];

        public EndPoint CurrentRemoteIp
        {
            get;set;
        }

        public MessageHandler handler
        { 
            get; set;
        }


        public UDPPort()
        {
            handler = new MessageHandler(this);
        }

        public void Start()
        {
            _socket = new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);
            //reuse same socket during every connection
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            //dont wait 
            //_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, new LingerOption(true, 10));
            _socket.Bind(new IPEndPoint(IPAddress.Any,1111));
        }

        public void Stop()
        {
            _socket.Close();
        }

        public void Recieve()
        {
            if (_socket == null)
                return;
            EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any,0);
            var buffer = new byte[512];
            var len = _socket.ReceiveFrom(buffer, ref remoteEndPoint);
            //var message = Encoding.ASCII.GetString(buffer);
            CurrentRemoteIp = remoteEndPoint;
            handler.Handle(buffer);
        }

        public void Send(string message)
        {
            byte[] _buffer = Encoding.ASCII.GetBytes(message);
            _socket.SendTo(_buffer, 0, _buffer.Length, SocketFlags.None, CurrentRemoteIp);
        }

        public void Send(CGMessage message)
        {
            long dataGramLength;
            try
            {
                BinaryWriter writer = new BinaryWriter(new MemoryStream(_TransmitBuffer, 0, _TransmitBuffer.Length, true));
                message.TryPublish(writer);
                dataGramLength = writer.BaseStream.Length;
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception occured in publishing object:{0}",ex.Message);
                return;
            }

            _socket.SendTo(_TransmitBuffer, SocketFlags.None,CurrentRemoteIp);
        }

        public CGMessage GetMessage(string value)
        {
            CGMessage message = null;
            switch (Program.CurrentMessageType)
            {
                case MessageType.Type1:
                    message = new Module1(value);
                    break;
                case MessageType.Type2:
                    message = new Module2(value);
                    break;
                case MessageType.Type3:
                    message = new Module3(value);
                    break;
                default:
                    break;
            }

            return message;
        }
    }
}
