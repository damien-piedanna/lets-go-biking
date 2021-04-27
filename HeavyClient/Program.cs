using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeavyClient.RoutingWithBikes;

namespace HeavyClient
{
    class Program
    {
        static ItineraryServiceClient client = new ItineraryServiceClient();
        static void Main(string[] args)
        {
            int choice = -1;
            while (choice != 3)
            {
                Console.WriteLine("Let's Go Biking Heavy Client");
                Console.WriteLine("\nChoose an action : ");
                Console.WriteLine("1 -> Display an itinerary with bikes");
                Console.WriteLine("2 -> Get stations logs");
                Console.WriteLine("3 -> Quit");

                choice = Int16.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        getBikeItinerary();
                        break;
                    case 2:
                        getStationsLogs();
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }

        /**
         * Get itinerary with bikes
         */
        static void getBikeItinerary()
        {
            Console.WriteLine("Calcul an itinerary between two addresses.");

            Console.WriteLine("Departure address :");
            string departureAddress = Console.ReadLine();

            Console.WriteLine("Arrival address :");
            string arrivalAddress = Console.ReadLine();

            Console.WriteLine("\nLoading...");

            Itinerary itinerary = client.GetItinerary(departureAddress, arrivalAddress);

            if (!itinerary.success)
            {
                Console.WriteLine("\nError : " + itinerary.errorMsg);
                return;
            }

            //Global infos
            Console.WriteLine("\nDistance : " + itinerary.distance + "s");
            Console.WriteLine("Duration : " + itinerary.duration + "m\n");

            //Display steps
            Console.WriteLine("==== Steps ==== ");
            int i = 0;
            foreach (ItineraryStep itineraryStep in itinerary.itineraries)
            {
                if (i == 1)
                {
                    Console.WriteLine("Take a bike");
                }

                if (i == itinerary.itineraries.Length-1 && itinerary.itineraries.Length > 1)
                {
                    Console.WriteLine("Leave your bike");
                }

                foreach (Step step in itineraryStep.featureCollection.features[0].properties.segments[0].steps)
                {
                    Console.WriteLine(step.instruction);
                }
                i++;
            }
        }

        /**
         * Get stations logs
         */
        static void getStationsLogs()
        {
            StationsLog stationsLog = client.GetStationsLog();

            Console.WriteLine("Usages:");
            foreach (var item in stationsLog.nbUsage)
            {
                Console.WriteLine(item.Key + " => " + item.Value);
            }
        }
    }
}
