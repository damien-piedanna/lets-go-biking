using System;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Device.Location;
using System.Runtime.Serialization;
using Cache;

namespace WebProxyService
{
    [DataContract]
    public class Position
    {
        [DataMember]
        public double latitude { get; set; }

        [DataMember]
        public double longitude { get; set; }

        [DataMember]
        public string city { get; set; }
    }
    [DataContract]
    public class Totalstands
    {
        [DataMember]
        public Availabilities availabilities { get; set; }
    }
    [DataContract]
    public class Availabilities
    {
        [DataMember]
        public int bikes { get; set; }

        [DataMember]
        public int stands { get; set; }

        [DataMember]
        public int electricalBikes { get; set; }
    }
    [DataContract]
    public class Station
    {
        [DataMember]
        public int number { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public Position position { get; set; }

        [DataMember]
        public string status { get; set; }

        [DataMember]
        public Totalstands totalStands { get; set; }

        public string contractName { get; set; }

        public bool isRefresh { get; set; }

        public override string ToString()
        {
            return "{ number : " + number + ","
                 + " contractName : " + contractName + ","
                 + " name : " + name + ","
                 + " position :" + position + " }";
        }

    }

    /**
     * Represent a list of JCDecaux station
     */
    public class StationList
    {
        protected static readonly HttpClient client = new HttpClient();
        protected static readonly string apiKey = "a0dbd529c92077bf289fd7bee14fd1b6f0002eae";
        protected static readonly string baseURL = "https://api.jcdecaux.com/vls/v3";

        private readonly string contract;
        private List<Station> stations = new List<Station>();
        private readonly Cache<Station> stationsDetailsCache = new Cache<Station>(60);

        public StationList(String contract) {
            this.contract = contract;
            LoadFromAPI();
        }

        /**
         * Load station list from JCDecaux API
         */
        public void LoadFromAPI()
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(baseURL + "/stations?contract=" + contract + "&apiKey=" + apiKey).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    stations = JsonSerializer.Deserialize<List<Station>>(responseBody);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("\nException Caught!");
                System.Diagnostics.Debug.WriteLine("Message :{0} ", e.Message);
            }
        }

        public bool isEmpty()
        {
            return stations.Count == 0;
        }

        /**
         * Refresh a specific station from JCDecaux API with is number
         */
        public Station RefreshStation(int number)
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(baseURL + "/stations/" + number + "?contract=" + contract + "&apiKey=" + apiKey).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return JsonSerializer.Deserialize<Station>(responseBody);
                }
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine("\nException Caught!");
                System.Diagnostics.Debug.WriteLine("Message :{0} ", e.Message);
            }
            return null;
        }

        /**
         * Get the nearest station from position, with need bike or slot option
         */
        public Station GetNearest(Position position, bool needBike, bool needSlot)
        {
            GeoCoordinate coord = new GeoCoordinate(position.latitude, position.longitude);

            Station nearestStation = null;
            double bestDistance = 0;
            var remainingStations = this.stations;

            while (true)
            {
                foreach (var station in remainingStations)
                {
                    if (
                        nearestStation == null
                        || coord.GetDistanceTo(new GeoCoordinate(station.position.latitude, station.position.longitude)) < bestDistance
                    )
                    {
                        nearestStation = station;
                        bestDistance = coord.GetDistanceTo(new GeoCoordinate(station.position.latitude, station.position.longitude));
                    }
                }

                //Need to refresh station to verify status, need bike and need slot
                Station refreshedStation = stationsDetailsCache.Get(nearestStation.number.ToString());
                if (refreshedStation == null) {
                    refreshedStation = RefreshStation(nearestStation.number);
                    stationsDetailsCache.Set(nearestStation.number.ToString(), refreshedStation);
                }
                nearestStation = refreshedStation;

                //Verify opening, need bike and need slot
                if (
                    nearestStation.status != "OPEN" ||
                    (
                        needBike && (nearestStation.totalStands.availabilities.bikes == 0 && nearestStation.totalStands.availabilities.electricalBikes == 0)
                        || 
                        needSlot && (nearestStation.totalStands.availabilities.stands == 0)
                    )
                )
                {
                    //if nearest station unusable, remove it from list an rerun
                    remainingStations.Remove(nearestStation);
                    if (remainingStations.Count == 0) {
                        return null;
                    }
                }
                else
                {
                    return nearestStation;
                }
            }
        }

        public override string ToString()
        {
            return string.Join(",", stations);
        }

    }
}
