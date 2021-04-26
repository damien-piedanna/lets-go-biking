using System;
using Cache;

namespace WebProxyService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "JCDecauxService" à la fois dans le code et le fichier de configuration.
    public class JCDecauxService : IJCDecauxService
    {
        readonly Cache<ListStation> listStationCache = new Cache<ListStation>(600);
        public Position GetNearestStationStartCoordinates(Position position)
        {
            return GetNearestStationCoordinates(position, true);
        }

        public Position GetNearestStationEndCoordinates(Position position)
        {
            return GetNearestStationCoordinates(position, false);
        }

        private Position GetNearestStationCoordinates(Position position, bool isStart)
        {
            String contract = position.city.ToLower();
            ListStation listStation = listStationCache.Get(contract);
            if (listStation == null)
            {
                listStation = new ListStation(contract);
                listStationCache.Set(contract, listStation);
            }
            if (listStation.isEmpty())
            {
                return null;
            }
            Station station = isStart ? listStation.GetNearest(position, true, false) : listStation.GetNearest(position, false, true);
            if (station == null) return null;
            return station.position;
        }
    }
}
