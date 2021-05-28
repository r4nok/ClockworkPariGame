using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;
using ServiceLevel;

namespace MessengerHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(ServiceMessenger)))
            {
                host.Open();
                Console.WriteLine("Host started!");
                Console.ReadKey();
            }
        }
    }
}
