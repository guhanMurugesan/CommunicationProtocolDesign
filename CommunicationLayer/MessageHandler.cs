using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Server
{
    public class MessageHandler
    {
        UDPPort Socket;
        MessageProcessor processor;
        public MessageHandler(UDPPort socket)
        {
            Socket = socket;
            processor = new MessageProcessor(socket);
        }

        public void Handle(string message)
        {
            Console.WriteLine("Message recieved From:{0}", Socket.CurrentRemoteIp);
            Console.WriteLine("{0}",message);
            Console.WriteLine("type your reply....");
            string replyMessage = Console.ReadLine();
            Socket.Send(replyMessage);
        }

        public void Handle(byte[] buffer)
        {
            CGMessage cgMessage;
            CGMessage.TryParse(buffer, out cgMessage);
            processor.Process(cgMessage);
            Console.WriteLine("type your reply....");
            string replyMessage = Console.ReadLine();
            Socket.Send(Socket.GetMessage(replyMessage));
        }
    }
}
