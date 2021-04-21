using System.ServiceModel;

namespace WebProxyService
{
    [ServiceContract]
    public interface IJCDecauxService
    {
        [OperationContract]
        Position GetNearestStationStartCoordinates(Position position);

        [OperationContract]
        Position GetNearestStationEndCoordinates(Position position);
    }
}
