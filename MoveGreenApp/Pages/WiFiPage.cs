using System;
using Xamarin.Forms;

namespace MoveGreenApp
{
	public class WiFiPage : ContentPage
	{
		public WiFiPage ()
		{
			Title = "WiFi";
			Icon = "wifi.png";

			Content = new Layout_WiFi().Content;
		}
	}
}