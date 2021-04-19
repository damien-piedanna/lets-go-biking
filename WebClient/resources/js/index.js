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
  });

  /**
   * Say Hi
   */
  function hi() {
    console.log("hi")
  }
})();
