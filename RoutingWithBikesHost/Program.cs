using System;
using System.ServiceModel;
using RoutingWithBikes;

namespace RoutingWithBikesHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost serviceHost = new ServiceHost(typeof(ItineraryService)))
            {
                // Open the ServiceHost to create listeners
                // and start listening for messages.  
                serviceHost.Open();

                // The service can now be accessed.  
                Console.WriteLine("The Routing With Bike WCF is ready.");
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.WriteLine();
                Console.ReadLine();
            }
        }
    }
}
