using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Threading.Tasks;
using Plugin.Geolocator;
using System.Diagnostics;

namespace MoveGreenApp
{
	public class MapPage : ContentPage
	{
		ControlLocation control = new ControlLocation();

		public MapPage ()
		{
			Title = "Move Green";
			Icon = "map.png";

			Image iconImg = new Image{
				WidthRequest = 120,
				HeightRequest = 120,
				Aspect = Aspect.AspectFit,
				Source = ImageSource.FromFile("app_icon.png")
			};

			Button drawableMapButton = new Button {
				Text = "Navigate to drawable map"
			};

			Button directionsMapButton = new Button {
				Text = "Navigate to directions map"
			};

			Content = new StackLayout {
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					iconImg,
					drawableMapButton,
					directionsMapButton
				}
			};

			drawableMapButton.Clicked += (object sender, EventArgs e) => {
				Navigation.PushAsync(new DrawableMapPage());
			};

			directionsMapButton.Clicked += (object sender, EventArgs e) => {
				Navigation.PushAsync(new DirectionsMapPage());
			};
		}
	}
}