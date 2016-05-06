using System;
using System.Collections.Generic;

using MoveGreenApp;
using MoveGreenApp.iOS;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;

using UIKit;
using MapKit;
using CoreLocation;

[assembly: ExportRenderer(typeof(DirectionsMap),typeof(DirectionsMapCustomRenderer))]

namespace MoveGreenApp.iOS
{
	public class DirectionsMapCustomRenderer : MapRenderer
	{
		MKPolylineRenderer lineRenderer;

		protected override void OnElementChanged (ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged (e);

			var mapView = (Control as MKMapView);
			var directionsMap = e.NewElement as DirectionsMap;

			if (mapView != null) {
				mapView.OverlayRenderer = (m, o) => {
					if (lineRenderer == null) {
						lineRenderer = new MKPolylineRenderer (o as MKPolyline);
						lineRenderer.FillColor = UIColor.Blue;
						lineRenderer.Alpha = 1.0f;
						lineRenderer.LineWidth = (nfloat)10;
						lineRenderer.StrokeColor = UIColor.Blue;
					}
					return lineRenderer;
				};
				if(directionsMap.StartDestination != null && directionsMap.EndDestination != null)
					GetDirections (directionsMap, mapView);
			}
		}

		void GetDirections(DirectionsMap directionsMap, MKMapView mapView)
		{
			var orignPlaceMark = new MKPlacemark(
				new CLLocationCoordinate2D(
					directionsMap.StartDestination.Latitude, 
					directionsMap.StartDestination.Longitude), 
				new MKPlacemarkAddress()
			);
			var destPlaceMark = new MKPlacemark (
				new CLLocationCoordinate2D (
					directionsMap.EndDestination.Latitude, 
					directionsMap.EndDestination.Longitude), 
				new MKPlacemarkAddress()
			);

			var sourceItem = new MKMapItem(orignPlaceMark);
			var destItem = new MKMapItem(destPlaceMark);

			var request = new MKDirectionsRequest
			{
				Source = sourceItem,
				Destination = destItem,
				RequestsAlternateRoutes = true,
			};

			var directions = new MKDirections(request);

			string turnByTurnDirections = "Start Route";
			string distance = "";

			directions.CalculateDirections (
				(response, error) => {
					if (error != null) {
						Console.WriteLine (error.LocalizedDescription);
					} else {
						for(var b = 0;b<response.Routes[0].Steps.Length;b++){
							turnByTurnDirections = turnByTurnDirections + "/" + response.Routes [0].Steps [b].Instructions;
							distance = distance + response.Routes [0].Steps [b].Distance + "/";
						}
						distance = distance + "/";
						mapView.AddOverlay (response.Routes[0].Polyline);
					}

					string[] distanceSplit = distance.Split ('/');
					string[] directionsSplit = turnByTurnDirections.Split ('/');

					DirectionsModel combo = new DirectionsModel {
						DistanceInM = distanceSplit,
						Directions = directionsSplit
					};
					directionsMap.Directions = combo;
					directionsMap.ExpectedTravelTime = response.Routes[0].ExpectedTravelTime;
				}
			);
		}
	}
}