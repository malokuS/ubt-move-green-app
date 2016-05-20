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
		WiFiManager wifiManager;

		public MapPage ()
		{
			Title = "Move Green";
			Icon = "map.png";

			wifiManager = new WiFiManager ();

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
				wifiManager.RefreshConnectionStatus();
				if(wifiManager.getConnectionStatus())
				{
					Navigation.PushAsync(new DrawableMapPage());
				}
				else
				{
					this.DisplayAlert("No Internet Connection!", "Please check your internet connection and try again.", "OK");
				}
			};

			directionsMapButton.Clicked += (object sender, EventArgs e) => {
				wifiManager.RefreshConnectionStatus();
				if(wifiManager.getConnectionStatus())
				{
					Navigation.PushAsync(new DirectionsMapPage());
				}
				else
				{
					this.DisplayAlert("No Internet Connection!", "Please check your internet connection and try again.", "OK");
				}
			};
		}
	}
}