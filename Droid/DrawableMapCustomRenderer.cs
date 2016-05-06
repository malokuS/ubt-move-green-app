using System;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Maps.Android;

using MoveGreenApp;
using MoveGreenApp.Droid;

using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.App;

[assembly: ExportRenderer(typeof(DrawableMap), typeof(DrawableMapCustomRenderer))]

namespace MoveGreenApp.Droid
{
	public class DrawableMapCustomRenderer : MapRenderer
	{
		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			var drawableMap = e.NewElement as DrawableMap;
			if (drawableMap != null)
			{
				var mapView = Control as Android.Gms.Maps.MapView;
				if (mapView != null)
				{                   
					if (drawableMap.GeoData != null)
					{
						mapView.GetMapAsync (new MapReady (drawableMap));
					}
				}   
			}
		}

		class MapReady : Java.Lang.Object, IOnMapReadyCallback
		{
			DrawableMap _drawableMap;

			public MapReady(DrawableMap drawableMap)
			{
				_drawableMap = drawableMap;
			}

			public void OnMapReady(GoogleMap googleMap)
			{
				DrawLine (googleMap);          
			}

			// add line feature to map
			void DrawLine(GoogleMap googleMap)
			{
				if (_drawableMap.GeoData != null)
				{
					var line = new PolylineOptions();
					var lineColor = Android.Graphics.Color.Blue;
					lineColor.A = 170;

					line.InvokeColor(lineColor.ToArgb()); 
					foreach(var p in _drawableMap.GeoData)
					{
						line.Add(new LatLng(p.Latitude, p.Longitude));
					}
					googleMap.AddPolyline(line);
				}
			}
		}
	}
}