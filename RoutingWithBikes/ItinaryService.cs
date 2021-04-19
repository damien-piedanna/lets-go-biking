using RoutingWithBikes.WebProxyService;

namespace RoutingWithBikes
{
    public class ItinaryService : IItinaryService
    {
        public Position GetItinary(Position departure, Position arrival)
        {
            JCDecauxServiceClient client = new JCDecauxServiceClient();
            Position stationStartPos = client.GetNearestStationStartCoordinates(departure);
            return stationStartPos;
        }
    }
}
