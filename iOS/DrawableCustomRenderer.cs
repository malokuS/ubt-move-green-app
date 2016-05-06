using System;
using System.Collections.Generic;

using MoveGreenApp;
using MoveGreenApp.iOS;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;

using MapKit;
using CoreLocation;
using UIKit;

[assembly: ExportRenderer(typeof(DrawableMap),typeof(DrawableMapCustomRenderer))]

namespace MoveGreenApp.iOS
{
	public class DrawableMapCustomRenderer : MapRenderer
	{
		MKPolylineRenderer lineRenderer;
		MKPolyline lineOverlay;

		protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<Xamarin.Forms.View> e)
		{
			base.OnElementChanged(e);

			var mapView = (Control as MKMapView);

			if (mapView != null)
			{
				mapView.OverlayRenderer = (m, o) =>
				{
					if (lineRenderer == null)
					{
						lineRenderer = new MKPolylineRenderer(o as MKPolyline);
						lineRenderer.FillColor = UIColor.Blue;
						lineRenderer.Alpha = 0.5f;
						lineRenderer.LineWidth = (nfloat)7;
						lineRenderer.StrokeColor = UIColor.Blue;
					}
					return lineRenderer;
				};
			}

			var map = e.NewElement as DrawableMap;

			if (map != null)
			{
				if (map.GeoData != null)
				{
					DrawLine(map, mapView);
				}
			}
		}

		// add line to map
		void DrawLine(DrawableMap map, MKMapView mapView)
		{
			if (map.GeoData != null)
			{
				var points = new List<CLLocationCoordinate2D>();

				foreach (var p in map.GeoData)
				{
					var coord = new CLLocationCoordinate2D(p.Latitude, p.Longitude);
					points.Add(coord);
				}

				lineOverlay = MKPolyline.FromCoordinates(points.ToArray());
				mapView.AddOverlay(lineOverlay);
			}
		}
	}
}