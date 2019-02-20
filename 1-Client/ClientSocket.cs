using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _1_Client
{
    public class ClientSocket
    {
        UdpClient _socket;

        byte[] _TransmitBuffer = new byte[512];

        public ClientSocket()
        {

        }

        public void Start()
        {
            _socket = new UdpClient();
            _socket.Connect(Program.IPAddress, 1111);
        }

        public void Receive()
        {
            IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
            var buffer = _socket.Receive(ref remoteEndpoint);
            CGMessage cgMessage;
            CGMessage.TryParse(buffer, out cgMessage);
            var value = ((ICGMessage)cgMessage).value;
            Console.WriteLine(value);
        }

        public void Send(string message)
        {
            long dataGramLength;
            try
            {
                var cgMessage = GetMessage(message);
                BinaryWriter writer = new BinaryWriter(new MemoryStream(_TransmitBuffer,true));
                ((CGMessage)cgMessage).TryPublish(writer);
                dataGramLength = writer.BaseStream.Length;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured in publishing message:{0}",ex.Message);
                return;
            }
            _socket.Send(_TransmitBuffer, (int)dataGramLength);
        }

        public void Stop()
        {
            if(_socket != null)
                _socket.Close();
        }

        private CGMessage GetMessage(string value)
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
