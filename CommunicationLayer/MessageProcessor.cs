using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Server
{
    public class MessageProcessor
    {
        UDPPort Socket;
        public MessageProcessor(UDPPort socket)
        {
            Socket = socket;
        }

        public void Process(CGMessage cgMessage)
        {
            switch (cgMessage.messageType)
            {
                case MessageType.Type1:
                    Console.WriteLine(((Module1)cgMessage).value);
                    break;
                case MessageType.Type2:
                    Console.WriteLine(((Module2)cgMessage).value);
                    break;
                case MessageType.Type3:
                    Console.WriteLine(((Module3)cgMessage).value);
                    break;
                default:
                    break;
            }
        }
    }
}
