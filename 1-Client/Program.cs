using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_Client
{
    class Program
    {
        public static MessageType CurrentMessageType;
        public static UInt32 TransactionId;
        public static string IPAddress = "10.2.108.48";

        static void Main(string[] args)
        {
            int p;
            Console.WriteLine("Enter Server IP:");
            IPAddress = Console.ReadLine();
            ClientSocket socket = new ClientSocket();
            socket.Start();

            Console.WriteLine("\t\tChoose message type\n1. Type1\n2. type2\n3. type3");
            CurrentMessageType = (MessageType)(Convert.ToUInt16(Console.ReadLine()));

            do
            {
                Console.WriteLine("send message to master");
                var message = Console.ReadLine();
                socket.Send(message);
                socket.Receive();
                Console.WriteLine("Press 1 to continue");
                p = Convert.ToInt16(Console.ReadLine());
            } while (p == 1);
        }
    }
}
