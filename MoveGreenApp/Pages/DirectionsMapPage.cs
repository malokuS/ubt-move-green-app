using System;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MoveGreenApp
{
	public class DirectionsMapPage : ContentPage
	{
		public DirectionsMapPage ()
		{
			MapSpan mapSpan = MapSpan.FromCenterAndRadius (new Position (42.673207, 21.198152), Distance.FromKilometers (1.0));

			Pin startPin = new Pin 
			{
				Type = PinType.Place,
				Position = new Position (42.673207, 21.198152),
				Label = "Stacioni i fundit - Linja 4",
				Address = "Rr. Dr.Shpetim Robaj - Parku Germia"
			};

			Pin finishPin = new Pin 
			{
				Type = PinType.Place,
				Position = new Position(42.656144, 21.180569),
				Label = "Stacioni i pare - Linja 4",
				Address = "Rr. Xheladin Hana"
			};

			DirectionsMap directionsMap = new DirectionsMap (startPin, finishPin) {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			directionsMap.MoveToRegion (mapSpan);

			StackLayout stack = new StackLayout {
				Children = { directionsMap }
			};

			Content = stack;

			ToolbarItem getDirections = new ToolbarItem ("Get Directions", null, () => {
				Navigation.PushAsync (new DirectionsListPage (directionsMap.Directions));
			});

			ToolbarItems.Add (getDirections);
		}
	}
}