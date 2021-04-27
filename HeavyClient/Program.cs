using System;
using System.IO;
using System.Text;
using HeavyClient.RoutingWithBikes;

namespace HeavyClient
{
    class Program
    {
        static ItineraryServiceClient client = new ItineraryServiceClient();
        static void Main(string[] args)
        {
            int choice = -1;
            while (choice != 4)
            {
                Console.WriteLine("\nLet's Go Biking Heavy Client");
                Console.WriteLine("\nChoose an action : ");
                Console.WriteLine("1 -> Display an itinerary with bikes");
                Console.WriteLine("2 -> Visualize stations logs");
                Console.WriteLine("3 -> Save stations logs");
                Console.WriteLine("4 -> Quit");

                choice = Int16.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        getBikeItinerary();
                        break;
                    case 2:
                        getStationsLogs();
                        break;
                    case 3:
                        saveStationsLogs();
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

            Console.WriteLine("\nUsages:");
            foreach (var item in stationsLog.nbUsage)
            {
                Console.WriteLine(item.Key + " => " + item.Value);
            }
        }

        /**
         * Get and save stations logs in a file
         */
        static void saveStationsLogs()
        {
            StationsLog stationsLog = client.GetStationsLog();

            if (stationsLog.nbUsage.Count != 0)
            {
                var builder = new StringBuilder();
                builder.AppendLine("Station;Nb Usage");
                foreach (var item in stationsLog.nbUsage)
                {
                    builder.AppendLine($"{item.Key};{item.Value}");
                }

                Console.WriteLine("\nSaving directory :");
                string excelPath = Console.ReadLine();
                excelPath += "/StationLogs_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".csv";

                File.WriteAllText(excelPath, builder.ToString());
                Console.WriteLine("\nData saved!");
            } else
            {
                Console.WriteLine("\nThere is no log for the moment.");
            }


        }
    }
}
