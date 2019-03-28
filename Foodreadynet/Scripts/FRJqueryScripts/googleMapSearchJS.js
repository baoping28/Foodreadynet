var map;
function initialize() {
    var myOptions = {
        zoom: 14,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    var mk = mapinfo = document.getElementById('markers');
    if (mk == null || mk == undefined) return false;
    var mapinfo = mk.value;
    var all;
    all = eval("(" + mapinfo + ")");
    var infoWindow = new google.maps.InfoWindow;
    map = new google.maps.Map(document.getElementById('map_canvas1'), myOptions);
    // Set the center of the map
    var mlat = all[0][5];
    var mlng = all[0][6];
    var pos = new google.maps.LatLng(mlat, mlng);
    map.setCenter(pos);
    var ic = new google.maps.MarkerImage('http://maps.google.com/mapfiles/ms/micons/man.png');
    var marker1 = new google.maps.Marker({ position: new google.maps.LatLng(mlat, mlng), map: map});

    function infoCallback(place) {
        return function () {
           window.location="#" + place;
        };
    }
    function setMarkers(map, all) {
        for (var i in all) {
            var j = parseInt(i) + 1;
            var name = all[i][0];
            var address = all[i][1];
            var city = all[i][2];
            var state = all[i][3];
            var zip = all[i][4];
            var lat = all[i][5];
            var lng = all[i][6];
            var id = all[i][7];
            var imgname = all[i][8];
            var bizindex = all[i][9];
            var latlngset;
            latlngset = new google.maps.LatLng(lat, lng);
            var ic = 'http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=' + j + '|FF0000|000000';
            var marker = new google.maps.Marker({
                map: map, title: name, position: latlngset, icon: ic
            });
            var menu = '<a href="#' + bizindex + '">Order Now</a>';
            var content = '<div class="map-content"><img class="gimg"" src="/Content/images/' + imgname + '.png"/><h3>' + name + '</h3>' + address + ', ' + city + ', ' + state + ' ' + zip + '<br /><br />' + menu + '<br/><a href="http://maps.google.com/?daddr=' + address + ' ' + city + ', ' + state + ' ' + zip + '" target="_blank">Get Directions</a></div>';
            var infowindow = new google.maps.InfoWindow();
            infowindow.setContent(content);
            google.maps.event.addListener(
                marker,
                'click',
                infoCallback(bizindex)
              );
        }
    }
    // Set all markers in the all variable
    setMarkers(map, all);
};
// Initializes the Google Map
google.maps.event.addDomListener(window, 'load', initialize);