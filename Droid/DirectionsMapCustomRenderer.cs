using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Maps.Android;

using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.App;
using Android.Media;

using MoveGreenApp;
using MoveGreenApp.Droid;

using Newtonsoft.Json;
using Android.Locations;

[assembly: ExportRenderer(typeof(DirectionsMap), typeof(DirectionsMapCustomRenderer))]


namespace MoveGreenApp.Droid
{
	public class DirectionsMapCustomRenderer : MapRenderer
	{
		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			var directionsMap = e.NewElement as DirectionsMap;
			if (directionsMap != null)
			{
				var mapView = Control as Android.Gms.Maps.MapView;
				if (mapView != null)
				{                   
					if (directionsMap.StartDestination != null && directionsMap.EndDestination != null)
					{
						mapView.GetMapAsync (new MapReady (directionsMap));
					}
				}   
			}
		}

		class MapReady : Java.Lang.Object, IOnMapReadyCallback
		{
			DirectionsMap _directionMap;

			public MapReady(DirectionsMap directionMap)
			{
				_directionMap = directionMap;
			}

			public void OnMapReady(GoogleMap googleMap)
			{
				SetDirectionsQuery (googleMap);          
			}

			private async void SetDirectionsQuery(GoogleMap googleMap)
			{
				var sb = new System.Text.StringBuilder();
				sb.Append("http://maps.googleapis.com/maps/api/directions/json?origin=");
				sb.Append (_directionMap.StartDestination.Latitude);
				sb.Append(",");
				sb.Append (_directionMap.StartDestination.Longitude);
				sb.Append("&destination=");
				sb.Append (_directionMap.EndDestination.Latitude);
				sb.Append(",");
				sb.Append (_directionMap.EndDestination.Longitude);
				sb.Append("&sensor=true");

				//Get directions through Google Web Service
				var directionsTask = GetDirections(sb.ToString());
				var jSonData = await directionsTask;
				string turnByTurnDirections = "";
				string distance = "/";
				string duration = "/";
				int totalDuration = 0;

				//Deserialize string to object
				RootObject routes = JsonConvert.DeserializeObject<RootObject>(jSonData);
				Route route = routes.routes [0];

				PolylineDecoder decoder = new PolylineDecoder ();
				List<Location> locations = decoder.DecodePolylinePoints (route.overview_polyline.points);

				PolylineOptions polylineOptions = new PolylineOptions ();
				for (var i = 0; i < locations.Count; i++) {
					polylineOptions.Add (new LatLng (locations [i].Latitude, locations [i].Longitude));
				}

				turnByTurnDirections = route.legs [0].start_address + "/";

				foreach (var dir in route.legs[0].steps) {
					var plainText = HtmlConverter(dir.html_instructions);
					turnByTurnDirections = turnByTurnDirections + plainText + "/";

					distance = distance + dir.distance.value.ToString() + "/";
					duration = duration + dir.duration.text + "/";
					totalDuration += dir.duration.value;
				}

				turnByTurnDirections += route.legs [0].end_address;

				string[] distanceSplit = distance.Split ('/');
				string[] directionsSplit = turnByTurnDirections.Split ('/');
				string[] durationSplit = duration.Split ('/');

				DirectionsModel combo = new DirectionsModel {
					DistanceInM = distanceSplit,
					Directions = directionsSplit,
					Duration = durationSplit
				};

				_directionMap.Directions = combo;
				_directionMap.ExpectedTravelTime = totalDuration;
				googleMap.AddPolyline(polylineOptions);
			}

			private async Task<String> GetDirections(string url)
			{
				var client = new WebClient();
				var directionsTask = client.DownloadStringTaskAsync(url);
				var directions = await directionsTask;
				return directions;
			}

			private string HtmlConverter(string html)
			{
				string[] delimiterChars = new string[] { "<b>", "</b>"};
				string[] words = html.Split(delimiterChars,StringSplitOptions.None);
				string direction = "";

				foreach (var word in words) {
					direction += word;
				}
				return direction;
			}
		}
	}
}