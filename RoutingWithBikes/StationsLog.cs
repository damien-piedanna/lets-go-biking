using System.Runtime.Serialization;
using System.Collections.Generic;
using RoutingWithBikes.WebProxyService;

namespace RoutingWithBikes
{
    /* 
     * Class used to log stations activities
     */
    [DataContract]
    public class StationsLog
    {
        [DataMember]
        public IDictionary<string, int> nbUsage { get; set; } //Keep station nb usage

        private static StationsLog instance = null;

        public StationsLog()
        {
            nbUsage = new Dictionary<string, int>();
        }

        public static StationsLog GetInstance()
        {
            if (instance == null) instance = new StationsLog();
            return instance;
        }

        /*
         * Add an usage to a station
         */
        public void addUsage(Station station)
        {
            if (nbUsage.ContainsKey(station.name))
            {
                nbUsage[station.name] += 1;
            }
            else
            {
                nbUsage.Add(station.name, 1);
            }
        }

        /*
         * Get station nb usage
         */
        public int getNbUsage(Station station)
        {
            return nbUsage[station.name];
        }
    }
}
