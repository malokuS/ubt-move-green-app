using System;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Collections.Generic;
using Plugin.Geolocator;
using System.Threading.Tasks;
using MoveGreenApp;
using System.Diagnostics;

namespace MoveGreenApp
{
	public class DrawableMapPage : ContentPage
	{
		DrawableMap map;
		ControlLocation control = new ControlLocation();
		MapSpan mapSpan;
		double defaultLat = 42.660824, defaultLon = 21.162240;
		private Button street = new Button {Text = "Street", Opacity = 0.4};
		private Button satellite = new Button {Text = "Satellite", Opacity = 0.25};
		private Button hybrid = new Button {Text = "Hybrid", Opacity = 0.25};
		bool gotPos = false;
		RelativeLayout relL;
		StackLayout mapTypes, indicatorSL;

		public DrawableMapPage ()
		{
			Title = " Map";

			street.Clicked += MapTypesOnClick;
			satellite.Clicked += MapTypesOnClick;
			hybrid.Clicked += MapTypesOnClick;

			mapTypes = new StackLayout 
			{
				Spacing = 5,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalOptions = LayoutOptions.EndAndExpand,
				Children = {street, satellite, hybrid}
			};
					
			relL = new RelativeLayout 
			{

			};



			Content = relL;

			var setUp = SetUpMap ();
		}

		private async Task SetUpMap()
		{
			var startPin = new Pin 
			{
				Type = PinType.Place,
				Position = new Position (42.673207, 21.198152),
				Label = "Stacioni i fundit - Linja 4",
				Address = "Rr. Dr.Shpetim Robaj - Parku Germia"
			};

			var finishPin = new Pin 
			{
				Type = PinType.Place,
				Position = new Position(42.656144, 21.180569),
				Label = "Stacioni i pare - Linja 4",
				Address = "Rr. Xheladin Hana"
			};

			map = new DrawableMap (startPin,finishPin) 
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				IsShowingUser = true,
				MapType = MapType.Street,
			};

			relL.Children.Add (map, Constraint.RelativeToParent((Parent) =>{
				return Parent.X;
			}), Constraint.RelativeToParent((Parent) =>{
				return Parent.Y;
			}), Constraint.RelativeToParent((Parent) =>{
				return Parent.Width;
			}), Constraint.RelativeToParent((Parent) =>{
				return Parent.Height;
			}));

			relL.Children.Add (mapTypes, Constraint.RelativeToView(map, (Parent, sibling) =>{
				return sibling.X + 5;
			}), Constraint.RelativeToView(map, (Parent, sibling) =>{
				return sibling.Y;
			}), Constraint.RelativeToParent((Parent) =>{
				return Parent.Width;
			}), Constraint.RelativeToParent((Parent) =>{
				return Parent.Height;
			}));

			map.HasZoomEnabled = true;
			map.HasScrollEnabled = true;

			map.GeoData = BusLineFour.getLineCords ();

			MoveToPos ();

			gotPos = await control.GetCurrentPos ();

			MoveToPos ();
		}

		public void MoveToPos()
		{
			if (control.Latitude != 0 && control.Longitude != 0)
			{
				Debug.WriteLine ("Got Position ? : " + gotPos + " : " + control.Latitude + " ; " + control.Longitude);
				mapSpan = MapSpan.FromCenterAndRadius (new Position (control.Latitude, control.Longitude), Distance.FromKilometers (1));
			} else {
				Debug.WriteLine ("Didn't get Position ? : " + gotPos + " : " + control.Latitude + " ; " + control.Longitude);
				mapSpan = MapSpan.FromCenterAndRadius (new Position (defaultLat, defaultLon), Distance.FromKilometers (1));	
			}

			map.MoveToRegion (mapSpan);
		}

		void MapTypesOnClick (object sender, EventArgs e)
		{
			var b = sender as Button;
			switch (b.Text) 
			{
			case "Street":
				street.Opacity = 0.4;
				satellite.Opacity = 0.25;
				hybrid.Opacity = 0.25;
				map.MapType = MapType.Street;
				break;
			case "Satellite":
				street.Opacity = 0.5;
				satellite.Opacity = 0.75;
				hybrid.Opacity = 0.5;
				map.MapType = MapType.Satellite;
				break;
			case "Hybrid":
				street.Opacity = 0.5;
				satellite.Opacity = 0.5;
				hybrid.Opacity = 0.75;
				map.MapType = MapType.Hybrid;
				break;
			}
		}
	}
}