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
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "/itinerary?start={start}&end={end}")]
        Itinerary GetItinerary(string start, string end);
    }
    [DataContract]
    public class Itinerary
    {
        [DataMember]
        public double duration { get; set; }
        [DataMember]
        public List<ItineraryStep> itineraries { get; set; }

        public Itinerary()
        {
            itineraries = new List<ItineraryStep>();
        }

        public void addStep(ItineraryStep step)
        {
            duration += step.featureCollection.features[0].properties.segments[0].duration;
            itineraries.Add(step);
        }
    }
}
