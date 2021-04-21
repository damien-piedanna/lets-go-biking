(function() {
  "use strict";

  $(document).ready(function() {
    let map = L.map('map', {
      layers: L.tileLayer('https://cartodb-basemaps-{s}.global.ssl.fastly.net/rastertiles/voyager/{z}/{x}/{y}{r}.png', {
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright" target="_blank">OSM</a> &copy; <a href="http://cartodb.com/attributions" target="_blank">CartoDB</a>',
      }),
      center: [43.5526, 7.0228], //Cannes
      zoom: 13,
    })
    $('#btnItinerary').click(function() {
      let inputStart = $('#inputStart').val();
      let inputEnd = $('#inputEnd').val();

      if (inputStart == "" || inputEnd == "") {
        alert("Input vide");
      }
      let url = "http://localhost:8733/Design_Time_Addresses/RoutingWithBikes/ItineraryService/rest/itinerary?start=" + inputStart + "&end=" + inputEnd;
      $.ajax({
          type: "GET",
          url: url,
          success: function (data) {
            let result = data.GetItineraryResult;

            console.log(data.GetItineraryResult.itineraries[0].featureCollection);
            L.geoJSON(data.GetItineraryResult.itineraries[0].featureCollection).addTo(map);
          }
      });
    });
  });

  /**
   * Say Hi
   */
  function hi() {
    console.log("hi")
  }
})();
