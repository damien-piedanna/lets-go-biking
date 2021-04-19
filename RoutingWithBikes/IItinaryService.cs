using System.ServiceModel;
using System.ServiceModel.Web;
using RoutingWithBikes.WebProxyService;

namespace RoutingWithBikes
{
    [ServiceContract]
    public interface IItinaryService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "/itinary")]
        Position GetItinary(Position departure, Position arrival);
    }
}
