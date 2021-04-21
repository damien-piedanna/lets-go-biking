using RoutingWithBikes.WebProxyService;
using System.ServiceModel.Web;
using System.Text.Json;

namespace RoutingWithBikes
{
    public class ItineraryService : IItineraryService
    {
        public object HttpContext { get; private set; }

        public Itinerary GetItinerary(string startAddress, string endAddress)
        {
            OpenRouteServiceTools ORSTools = new OpenRouteServiceTools();
            JCDecauxServiceClient JCDecauxClient = new JCDecauxServiceClient();

            Position startPosition = ORSTools.getPosition(startAddress);
            Position endPosition = ORSTools.getPosition(endAddress);

            Position stationStartPos = JCDecauxClient.GetNearestStationStartCoordinates(startPosition);
            Position stationEndPos = JCDecauxClient.GetNearestStationStartCoordinates(endPosition);

            Itinerary res = new Itinerary();

            if (stationStartPos != null && stationEndPos != null)
            {
                //res.addStep(ORSTools.getFootItinary(startPosition, endPosition));
            }

            res.addStep(ORSTools.getFootItinary(startPosition, endPosition));

            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");

            return res;
        }
    }
}
