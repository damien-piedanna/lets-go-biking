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
                        System.Diagnostics.Debug.WriteLine(responseBody);
                        res = new Position
                        {
                            longitude = positionGEOJSON.features[0].geometry.coordinates[0],
                            latitude = positionGEOJSON.features[0].geometry.coordinates[1],
                        };
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
            string url = baseURL + "v2/directions/" + type + " ? api_key=" + apiKey + "&start=" + start.longitude + "," + start.latitude + "&end=" + end.longitude + "," + start.latitude;
            ItineraryStep res = ItineraryStepCache.Get(url);

            if (res == null)
            {
                try
                {
                    //HttpResponseMessage response = client.GetAsync(url).GetAwaiter().GetResult();
                    //if (response.IsSuccessStatusCode)
                    if (true)
                    {
                        //string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        string responseBody = "{\"type\":\"FeatureCollection\",\"features\":[{\"bbox\":[-1.54957,47.205903,-1.535372,47.234522],\"type\":\"Feature\",\"properties\":{\"segments\":[{\"distance\":3928.0,\"duration\":2828.1,\"steps\":[{\"distance\":7.4,\"duration\":5.3,\"type\":11,\"instruction\":\"HeadwestonBoulevardVincentGâche\",\"name\":\"BoulevardVincentGâche\",\"way_points\":[0,1]},{\"distance\":23.1,\"duration\":16.6,\"type\":1,\"instruction\":\"Turnright\",\"name\":\"-\",\"way_points\":[1,4]},{\"distance\":20.2,\"duration\":14.5,\"type\":0,\"instruction\":\"Turnleft\",\"name\":\"-\",\"way_points\":[4,8]},{\"distance\":318.5,\"duration\":229.3,\"type\":1,\"instruction\":\"TurnrightontoBoulevarddesMartyrsNantaisdelaRésistance\",\"name\":\"BoulevarddesMartyrsNantaisdelaRésistance\",\"way_points\":[8,20]},{\"distance\":11.3,\"duration\":8.1,\"type\":1,\"instruction\":\"Turnright\",\"name\":\"-\",\"way_points\":[20,22]},{\"distance\":6.3,\"duration\":4.5,\"type\":12,\"instruction\":\"Keepleft\",\"name\":\"-\",\"way_points\":[22,23]},{\"distance\":86.8,\"duration\":62.5,\"type\":1,\"instruction\":\"TurnrightontoQuaiMagellan\",\"name\":\"QuaiMagellan\",\"way_points\":[23,29]},{\"distance\":81.7,\"duration\":58.8,\"type\":12,\"instruction\":\"KeepleftontoQuaiMagellan\",\"name\":\"QuaiMagellan\",\"way_points\":[29,32]},{\"distance\":13.3,\"duration\":9.6,\"type\":12,\"instruction\":\"KeepleftontoRueFouré\",\"name\":\"RueFouré\",\"way_points\":[32,34]},{\"distance\":429.3,\"duration\":309.1,\"type\":6,\"instruction\":\"ContinuestraightontoRueFouré\",\"name\":\"RueFouré\",\"way_points\":[34,50]},{\"distance\":134.0,\"duration\":96.5,\"type\":6,\"instruction\":\"ContinuestraightontoRueFouré\",\"name\":\"RueFouré\",\"way_points\":[50,56]},{\"distance\":19.9,\"duration\":14.3,\"type\":13,\"instruction\":\"KeeprightontoRueFouré\",\"name\":\"RueFouré\",\"way_points\":[56,61]},{\"distance\":6.0,\"duration\":4.3,\"type\":1,\"instruction\":\"TurnrightontoAvenueCarnot\",\"name\":\"AvenueCarnot\",\"way_points\":[61,62]},{\"distance\":156.0,\"duration\":112.3,\"type\":0,\"instruction\":\"Turnleft\",\"name\":\"-\",\"way_points\":[62,69]},{\"distance\":49.2,\"duration\":35.5,\"type\":4,\"instruction\":\"Turnslightleft\",\"name\":\"-\",\"way_points\":[69,73]},{\"distance\":49.3,\"duration\":35.5,\"type\":1,\"instruction\":\"Turnright\",\"name\":\"-\",\"way_points\":[73,77]},{\"distance\":72.1,\"duration\":51.9,\"type\":0,\"instruction\":\"TurnleftontoPlacedelaDuchesseAnne\",\"name\":\"PlacedelaDuchesseAnne\",\"way_points\":[77,85]},{\"distance\":289.4,\"duration\":208.3,\"type\":1,\"instruction\":\"TurnrightontoRueHenriIV\",\"name\":\"RueHenriIV\",\"way_points\":[85,93]},{\"distance\":23.8,\"duration\":17.1,\"type\":6,\"instruction\":\"ContinuestraightontoRueHenriIV\",\"name\":\"RueHenriIV\",\"way_points\":[93,95]},{\"distance\":53.7,\"duration\":38.7,\"type\":1,\"instruction\":\"TurnrightontoPlaceduMaréchalFoch\",\"name\":\"PlaceduMaréchalFoch\",\"way_points\":[95,98]},{\"distance\":307.9,\"duration\":221.7,\"type\":3,\"instruction\":\"TurnsharprightontoRueMaréchalJoffre\",\"name\":\"RueMaréchalJoffre\",\"way_points\":[98,112]},{\"distance\":250.1,\"duration\":180.1,\"type\":13,\"instruction\":\"KeeprightontoRueMaréchalJoffre\",\"name\":\"RueMaréchalJoffre\",\"way_points\":[112,124]},{\"distance\":7.7,\"duration\":5.6,\"type\":0,\"instruction\":\"TurnleftontoRueDufour\",\"name\":\"RueDufour\",\"way_points\":[124,125]},{\"distance\":550.5,\"duration\":396.3,\"type\":13,\"instruction\":\"KeeprightontoRueDufour\",\"name\":\"RueDufour\",\"way_points\":[125,139]},{\"distance\":41.3,\"duration\":29.7,\"type\":6,\"instruction\":\"ContinuestraightontoPlacedesEnfantsNantais\",\"name\":\"PlacedesEnfantsNantais\",\"way_points\":[139,142]},{\"distance\":96.3,\"duration\":69.3,\"type\":1,\"instruction\":\"TurnrightontoRuedel\'ÉvêqueÉmilien\",\"name\":\"Ruedel\'ÉvêqueÉmilien\",\"way_points\":[142,145]},{\"distance\":823.0,\"duration\":592.6,\"type\":0,\"instruction\":\"TurnleftontoRueGénéralBuat\",\"name\":\"RueGénéralBuat\",\"way_points\":[145,172]},{\"distance\":0.0,\"duration\":0.0,\"type\":10,\"instruction\":\"ArriveatRueGénéralBuat,ontheright\",\"name\":\"-\",\"way_points\":[172,172]}]}],\"summary\":{\"distance\":3928.0,\"duration\":2828.1},\"way_points\":[0,172]},\"geometry\":{\"coordinates\":[[-1.547353,47.205912],[-1.54745,47.205903],[-1.547478,47.205983],[-1.547496,47.206045],[-1.547513,47.206106],[-1.547609,47.206103],[-1.547681,47.206126],[-1.547704,47.206137],[-1.547764,47.206124],[-1.547988,47.206382],[-1.548333,47.206843],[-1.548381,47.206916],[-1.548449,47.207008],[-1.548489,47.207067],[-1.548536,47.207138],[-1.548556,47.207171],[-1.54943,47.208521],[-1.549484,47.208599],[-1.549507,47.20863],[-1.549553,47.208692],[-1.54957,47.20871],[-1.549476,47.208741],[-1.549454,47.208768],[-1.549473,47.208823],[-1.549459,47.208825],[-1.549065,47.208887],[-1.548922,47.20893],[-1.548736,47.208967],[-1.548567,47.209011],[-1.548387,47.209068],[-1.548159,47.209166],[-1.547802,47.209289],[-1.54743,47.209409],[-1.547407,47.209453],[-1.547358,47.209518],[-1.547307,47.209605],[-1.547187,47.21024],[-1.547172,47.210327],[-1.546923,47.211314],[-1.546904,47.211397],[-1.546883,47.211486],[-1.546762,47.212001],[-1.546746,47.212067],[-1.546733,47.212121],[-1.54671,47.212222],[-1.546669,47.212394],[-1.546625,47.212577],[-1.546607,47.212651],[-1.546587,47.212734],[-1.54649,47.213137],[-1.546435,47.213324],[-1.546413,47.213399],[-1.546251,47.214071],[-1.546242,47.214105],[-1.54623,47.214147],[-1.546199,47.214292],[-1.546153,47.214514],[-1.546124,47.214557],[-1.546089,47.214625],[-1.546077,47.214635],[-1.546059,47.214642],[-1.546014,47.214651],[-1.54597,47.214606],[-1.54594,47.214626],[-1.54605,47.214745],[-1.546071,47.214795],[-1.546182,47.214939],[-1.546724,47.215601],[-1.546859,47.215754],[-1.546917,47.215828],[-1.546962,47.215842],[-1.546978,47.215852],[-1.547158,47.216057],[-1.547276,47.216191],[-1.546937,47.216331],[-1.546873,47.216357],[-1.546769,47.216409],[-1.546726,47.216429],[-1.546764,47.216464],[-1.546787,47.216484],[-1.546813,47.216517],[-1.546849,47.21655],[-1.546904,47.216594],[-1.547064,47.216709],[-1.547232,47.216896],[-1.547315,47.21693],[-1.54734,47.217007],[-1.547642,47.217379],[-1.54772,47.217461],[-1.54776,47.217543],[-1.547889,47.217731],[-1.548621,47.218624],[-1.549082,47.219188],[-1.549111,47.219223],[-1.549243,47.219375],[-1.549271,47.219407],[-1.549224,47.219425],[-1.549299,47.219519],[-1.549539,47.219816],[-1.549505,47.219817],[-1.549456,47.219858],[-1.549254,47.220045],[-1.549226,47.220071],[-1.548752,47.22051],[-1.54871,47.220548],[-1.548624,47.220627],[-1.548192,47.221032],[-1.547946,47.22125],[-1.547698,47.22148],[-1.547652,47.221523],[-1.547316,47.221812],[-1.547123,47.221946],[-1.547055,47.221999],[-1.546979,47.222019],[-1.546947,47.222045],[-1.546762,47.222191],[-1.546641,47.222288],[-1.546555,47.222372],[-1.546181,47.222631],[-1.546108,47.22269],[-1.54604,47.222765],[-1.546028,47.22278],[-1.545308,47.223716],[-1.545288,47.223744],[-1.545216,47.223837],[-1.545283,47.22389],[-1.545306,47.223925],[-1.545304,47.223952],[-1.545193,47.224253],[-1.545039,47.224585],[-1.544863,47.22485],[-1.54468,47.225178],[-1.544621,47.225283],[-1.543927,47.226502],[-1.543885,47.226575],[-1.543489,47.227215],[-1.543475,47.227238],[-1.543454,47.22727],[-1.543416,47.227337],[-1.542733,47.22851],[-1.542694,47.228573],[-1.542516,47.228755],[-1.542447,47.228826],[-1.542227,47.228778],[-1.541434,47.228376],[-1.54139,47.228354],[-1.541302,47.228439],[-1.541149,47.228591],[-1.540763,47.228971],[-1.540652,47.229084],[-1.540591,47.229146],[-1.540498,47.229237],[-1.540042,47.229684],[-1.53972,47.230008],[-1.538802,47.230931],[-1.538465,47.23127],[-1.538418,47.231317],[-1.538328,47.231405],[-1.538274,47.231458],[-1.538248,47.231484],[-1.538168,47.231562],[-1.537365,47.232358],[-1.537224,47.232498],[-1.537165,47.232559],[-1.537005,47.232723],[-1.536576,47.233148],[-1.536429,47.233306],[-1.536041,47.233719],[-1.535926,47.233869],[-1.535783,47.23404],[-1.535702,47.234136],[-1.535492,47.234382],[-1.535372,47.234522]],\"type\":\"LineString\"}}],\"bbox\":[-1.54957,47.205903,-1.535372,47.234522],\"metadata\":{\"attribution\":\"openrouteservice.org|OpenStreetMapcontributors\",\"service\":\"routing\",\"timestamp\":1619016070027,\"query\":{\"coordinates\":[[-1.547355651855469,47.205925233120546],[-1.534996032714844,47.23437307718069]],\"profile\":\"foot-walking\",\"format\":\"json\"},\"engine\":{\"version\":\"6.4.1\",\"build_date\":\"2021-04-12T11:21:00Z\",\"graph_date\":\"1970-01-01T00:00:00Z\"}}}";
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
