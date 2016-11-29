<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Map.aspx.cs" Inherits="BingMaps" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bing Maps</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script charset="UTF-8" type="text/javascript" src="http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.3&mkt=en-us"></script>   
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="smPage" runat="server" />
        <div id='myMap' style="position:absolute; width:98%; height:95%;"></div>
    </form>
    <script type="text/javascript">
        var map = new VEMap('myMap');
        //map.SetCredentials("Your Bing Maps Key");
        map.LoadMap();
        //MapLocation("326 WOODBRIDGE CENTER DR. WOODBRIDGE NJ 07095");

        function MyHandleCredentialsError() { alert("The Bing Map credentials are invalid."); }
        function UnloadMap() { if(myMap != null) myMap.Dispose(); }
        function MapLocation(address) {
		 if(address != null) {
                var points = new Array(address);
                for (var i = 0; i < points.length; i++) {
                    map.Find(null, points[i], null, null, 0, 10, false, false, false, true, ProcessStore);
                }
		 }
		 else {
		    map.Clear();
		    map.SetZoomLevel(1); 
		    map.ZoomOut();
		 }
        }
        function ProcessStore(layer, results, places, hasmore) {
          //Create a custom pin
            if(places != null && places[0].LatLong != 'Unavailable') {
                var spec = new VECustomIconSpecification();
                spec.CustomHTML = "<div style='font-size:8px; border:solid 1px Black; background-color:red; width:8px;'>&nbsp;<div>";
                var pin = new VEShape(VEShapeType.Pushpin, places[0].LatLong);
                pin.SetCustomIcon(spec);
                map.AddShape(pin);
            }
        }
        function callWebService(url) {
            //Calls web service with url and callback function. Callback will
            //be executed when XMLHttpRequest object returns from web service call.
            var xmlDoc = new XMLHttpRequest();
            if(xmlDoc) {
                //Execute synchronous call to web service
                //asynchronous never returns a readystate > 1 with POST
                xmlDoc.onreadystatechange = function() { stateChange(xmlDoc); };
                xmlDoc.open("GET", url, true);
                //params = "name=" + document.infoForm.name.value + "&email=" + document.infoForm.email.value + "&phone=" + document.infoForm.phone.value + "&company=" + document.infoForm.company.value + "&address=" + document.infoForm.address.value + "&state=" + document.infoForm.state.value + "&options=" + document.infoForm.options.value;
                //xmlDoc.setRequestHeader("Content-length", params.length);
                xmlDoc.send(null);
            }
            else {
                alert("Unable to create XMLHttpRequest object.");
            }
        }

        function stateChange(xmlDoc) {
            //Updates readystate by callback
            if(xmlDoc.readyState == 4) {
                var text = "";
                if(xmlDoc.status == 200) {
                    //select node containing data
                    var nd = xmlDoc.responseXML.getElementsByTagName("mail");
                    if(nd && nd.length == 1) {
                        //IE use .text, others .textContent
                        text = !nd[0].text ? nd[0].textContent : nd[0].text;
                        if(text != "") 
                            alert(text);
                        else 
                            alert("Web service call failed: " + text);
                   }
                }
                else 
                    alert("Bad response: status code=" + xmlDoc.status);
            }
        }
    </script>
</body>
</html>
