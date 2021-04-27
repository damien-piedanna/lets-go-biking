using System.ServiceModel;

namespace WebProxyService
{
    [ServiceContract]
    public interface IJCDecauxService
    {
        [OperationContract]
        Station GetNearestStationStart(Position position);

        [OperationContract]
        Station GetNearestStationEnd(Position position);
    }
}
