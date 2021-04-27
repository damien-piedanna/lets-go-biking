using System;
using System.ServiceModel;
using WebProxyService;

namespace RoutingWithBikesHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost serviceHost = new ServiceHost(typeof(JCDecauxService)))
            {
                // Open the ServiceHost to create listeners
                // and start listening for messages.  
                serviceHost.Open();

                // The service can now be accessed.  
                Console.WriteLine("The Web Proxy Service WCF is ready.");
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.WriteLine();
                Console.ReadLine();
            }
        }
    }
}
