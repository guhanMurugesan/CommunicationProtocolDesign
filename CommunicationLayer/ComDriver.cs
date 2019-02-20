using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace N_Server
{
    public class ComDriver
    {
        Thread _Driver;
        UDPPort _Socket;
        public ComDriver()
        {

            _Socket = new UDPPort();
            _Socket.Start();
            _Driver = new Thread(() => Start());
            _Driver.Start();


        }

        public void Start()
        {
            while (true)
            {
                _Socket.Recieve();
            }
        }

        public void Stop()
        {
            _Driver.Join();
            if(_Socket != null)
                _Socket.Stop();
        }
    }
}
