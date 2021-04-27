using System;
using Cache;

namespace WebProxyService
{
    public class JCDecauxService : IJCDecauxService
    {
        readonly Cache<StationList> listStationCache = new Cache<StationList>(600);
        public Station GetNearestStationStart(Position position)
        {
            return GetNearestStation(position, true);
        }

        public Station GetNearestStationEnd(Position position)
        {
            return GetNearestStation(position, false);
        }

        /**
         * Get the nearest station to a position
         */
        private Station GetNearestStation(Position position, bool isStart)
        {
            String contract = position.city.ToLower();
               
            //Get station list
            StationList stationList = listStationCache.Get(contract);
            if (stationList == null)
            {
                stationList = new StationList(contract);
                listStationCache.Set(contract, stationList);
            }
            if (stationList.isEmpty()) //if the city is not managed by JCDecaux
            {
                return null;
            }

            return isStart ? stationList.GetNearest(position, true, false) : stationList.GetNearest(position, false, true);
        }
    }
}
