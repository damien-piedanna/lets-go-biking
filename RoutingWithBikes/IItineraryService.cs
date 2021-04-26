using System.ServiceModel;
using System.ServiceModel.Web;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace RoutingWithBikes
{
    [ServiceContract]
    public interface IItineraryService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "/itinerary?departureAddress={departureAddress}&arrivalAddress={arrivalAddress}")]
        Itinerary GetItinerary(string departureAddress, string arrivalAddress);
    }
    [DataContract]
    public class Itinerary
    {
        [DataMember]
        public bool success { get; set; }
        [DataMember]
        public string errorMsg { get; set; }
        [DataMember]
        public double duration { get; set; }
        [DataMember]
        public double distance { get; set; }
        [DataMember]
        public List<ItineraryStep> itineraries { get; set; }

        public Itinerary()
        {
            itineraries = new List<ItineraryStep>();
        }

        public void addStep(ItineraryStep step)
        {
            duration += step.featureCollection.features[0].properties.segments[0].duration;
            distance += step.featureCollection.features[0].properties.segments[0].distance;
            itineraries.Add(step);
        }
    }
}
