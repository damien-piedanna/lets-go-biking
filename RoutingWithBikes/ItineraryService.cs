using RoutingWithBikes.WebProxyService;
using System.ServiceModel.Web;
using System.Text.Json;

namespace RoutingWithBikes
{
    public class ItineraryService : IItineraryService
    {
        public object HttpContext { get; private set; }

        public Itinerary GetItinerary(string departureAddress, string arrivalAddress)
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");

            Itinerary res = new Itinerary();

            if (departureAddress == arrivalAddress)
            {
                res.errorMsg = "The departure and arrival addresses are identical";
                return res;
            }

            OpenRouteServiceTools ORSTools = new OpenRouteServiceTools();
            JCDecauxServiceClient JCDecauxClient = new JCDecauxServiceClient();

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

            Position stationDeparturePosition = JCDecauxClient.GetNearestStationStartCoordinates(departurePosition);
            Position stationArrivalPosition = JCDecauxClient.GetNearestStationEndCoordinates(arrivalPosition);

            if (stationDeparturePosition != null && stationArrivalPosition != null &&
               !(stationDeparturePosition.longitude == stationArrivalPosition.longitude && stationDeparturePosition.latitude == stationArrivalPosition.latitude)
            )
            {
                res.addStep(ORSTools.getFootItinary(departurePosition, stationDeparturePosition));
                res.addStep(ORSTools.getBikeItinary(stationDeparturePosition, stationArrivalPosition));
                res.addStep(ORSTools.getFootItinary(stationArrivalPosition, arrivalPosition));
            }
            else
            {
                res.addStep(ORSTools.getFootItinary(departurePosition, arrivalPosition));
                System.Diagnostics.Debug.WriteLine(">>>> arrivalPosition = A PIED FDP");
            }

            System.Diagnostics.Debug.WriteLine(">>>> stationDeparturePosition = " + stationDeparturePosition);
            System.Diagnostics.Debug.WriteLine(">>>> stationArrivalPosition = " + stationArrivalPosition);
            System.Diagnostics.Debug.WriteLine(">>>> departurePosition = " + departurePosition.latitude + " " + departurePosition.longitude);
            System.Diagnostics.Debug.WriteLine(">>>> arrivalPosition = " + arrivalPosition.latitude + " " + arrivalPosition.longitude);

            res.success = true;
            return res;
        }
    }
}
