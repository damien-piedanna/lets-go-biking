using System;
using System.Net.Http;
using System.Text.Json;
using Cache;
using RoutingWithBikes.WebProxyService;

namespace RoutingWithBikes
{
    class OpenRouteServiceTools
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string apiKey = "5b3ce3597851110001cf6248dd808ce2e89f440ab9cfca8763980c8b";
        private static readonly string baseURL = "https://api.openrouteservice.org/";

        private readonly Cache<Position> positionCache = new Cache<Position>(604800); //1 week cache
        private readonly Cache<ItineraryStep> ItineraryStepCache = new Cache<ItineraryStep>(604800); //1 week cache

        public Position getPosition(string address)
        {
            string url = baseURL + "geocode/search?api_key=" + apiKey + "&text=" + address;
            Position res = positionCache.Get(url);

            if (res == null)
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(url).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        PositionGeoJSON positionGEOJSON = JsonSerializer.Deserialize<PositionGeoJSON>(responseBody);
                        if (positionGEOJSON.features[0].properties.country == "France") //Ignore position if not in France
                        {
                            res = new Position
                            {
                                longitude = positionGEOJSON.features[0].geometry.coordinates[0],
                                latitude = positionGEOJSON.features[0].geometry.coordinates[1],
                                city = positionGEOJSON.features[0].properties.locality,
                            };
                        }
     
                        positionCache.Set(url, res);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("\nException Caught!");
                    System.Diagnostics.Debug.WriteLine("Message :{0} ", e.Message);
                }
            }

            return res;
        }

        public ItineraryStep getFootItinary(Position start, Position end)
        {
            return getItinary(start, end, "foot-walking");
        }

        public ItineraryStep getBikeItinary(Position start, Position end)
        {
            return getItinary(start, end, "cycling-regular");
        }

        private ItineraryStep getItinary(Position start, Position end, string type)
        {
            string url = baseURL + "v2/directions/" + type + "?api_key=" + apiKey 
                + "&start=" + start.longitude.ToString().Replace(",", ".")
                + "," + start.latitude.ToString().Replace(",", ".")
                + "&end=" + end.longitude.ToString().Replace(",", ".")
                + "," + end.latitude.ToString().Replace(",", ".");

            ItineraryStep res = ItineraryStepCache.Get(url);

            if (res == null)
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(url).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        res = new ItineraryStep
                        {
                            type = type,
                            featureCollection = JsonSerializer.Deserialize<ItineraryGeoJSON>(responseBody),
                        };
                        ItineraryStepCache.Set(url, res);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("\nException Caught!");
                    System.Diagnostics.Debug.WriteLine("Message :{0} ", e.Message);
                }
            }

            return res;
        }
    }
}
