using RoutingWithBikes.WebProxyService;
using System.ServiceModel.Web;

namespace RoutingWithBikes
{
    public class ItineraryService : IItineraryService
    {
        OpenRouteServiceTools ORSTools = new OpenRouteServiceTools();
        JCDecauxServiceClient JCDecauxClient = new JCDecauxServiceClient();
        StationsLog stationsLog = StationsLog.GetInstance();

        /**
         * Get itinerary with bikes
         */
        public Itinerary GetItinerary(string departureAddress, string arrivalAddress)
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");

            Itinerary res = new Itinerary();

            if (departureAddress == arrivalAddress)
            {
                res.errorMsg = "The departure and arrival addresses are identical";
                return res;
            }

            Position departurePosition = ORSTools.getPosition(departureAddress);
            if (departurePosition == null)
            {
                res.errorMsg = "This starting address could not be found in France";
                return res;
            }
            Position arrivalPosition = ORSTools.getPosition(arrivalAddress);
            if (arrivalPosition == null)
            {
                res.errorMsg = "This arrival address could not be found in France";
                return res;
            }
            
            //Get departure station
            Station stationDeparture = JCDecauxClient.GetNearestStationStart(departurePosition);
            stationsLog.addUsage(stationDeparture);

            //Get arrival station
            Station stationArrival = JCDecauxClient.GetNearestStationEnd(arrivalPosition);
            stationsLog.addUsage(stationArrival);

            //If start and end stations are found and they are different, then build an itinerary using bikes, otherwise do it on foot
            Position stationDeparturePosition = stationDeparture.position;
            Position stationArrivalPosition = stationArrival.position;
            if (stationDeparturePosition != null && stationArrivalPosition != null &&
               !(stationDeparturePosition.longitude == stationArrivalPosition.longitude && stationDeparturePosition.latitude == stationArrivalPosition.latitude)
            )
            {
                res.addStep(ORSTools.getFootItinerary(departurePosition, stationDeparturePosition));
                res.addStep(ORSTools.getBikeItinerary(stationDeparturePosition, stationArrivalPosition));
                res.addStep(ORSTools.getFootItinerary(stationArrivalPosition, arrivalPosition));
            }
            else
            {
                res.addStep(ORSTools.getFootItinerary(departurePosition, arrivalPosition));
            }

            res.success = true;
            return res;
        }

        /**
         * Get stations logs
         */
        public StationsLog GetStationsLog()
        {
            return stationsLog;
        }
    }
}
