using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Server
{
    class Program
    {
        public static MessageType CurrentMessageType;
        public static UInt32 TransactionId;
        public static string IPAddress = "10.2.108.48";

        static void Main(string[] args)
        {
            ComDriver comDriver = new ComDriver();

            comDriver.Start();

            Console.WriteLine("Press any key to stop");
            Console.ReadLine();
            comDriver.Stop();
        }
    }
}
