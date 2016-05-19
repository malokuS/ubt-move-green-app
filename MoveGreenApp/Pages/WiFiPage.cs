using System;
using Xamarin.Forms;
using System.Diagnostics;

namespace MoveGreenApp
{
	public class WiFiPage : ContentPage
	{
		WiFiManager wifiManager;
		Layout_WiFi wifiLayout;

		public WiFiPage ()
		{
			Title = "WiFi";
			Icon = "wifi.png";
			
			Content = new Layout_WiFi().Content;

			wifiManager = new WiFiManager ();
			wifiLayout = new Layout_WiFi ();

			wifiManager.ChangedConnection += OnChangedConnection;
		}

		private void RefreshView()
		{
			wifiLayout.OnRefreshBClicked (this, EventArgs.Empty);
		}

		public void OnChangedConnection(object source, EventArgs e)
		{
			RefreshView ();
		}
	}
}