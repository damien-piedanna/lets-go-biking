using System;

namespace WebProxyService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "JCDecauxService" à la fois dans le code et le fichier de configuration.
    public class JCDecauxService : IJCDecauxService
    {
        readonly ProxyCache<ListStation> listStationCache = new ProxyCache<ListStation>(600);
        public Position GetNearestStationStartCoordinates(Position position)
        {
            String contract = "nantes";
            ListStation listStation = listStationCache.Get(contract);
            if (listStation == null) {
                listStation = new ListStation(contract);
                listStationCache.Set(contract, listStation);
            }
            Station station = listStation.GetNearest(position, true, false);
            return station.position;
        }

        public Position GetNearestStationEndCoordinates(Position position)
        {
            String contract = "nantes";
            ListStation listStation = listStationCache.Get(contract);
            if (listStation == null)
            {
                listStation = new ListStation(contract);
                listStationCache.Set(contract, listStation);
            }
            Station station = listStation.GetNearest(position, false, true);
            return station.position;
        }
    }
}
