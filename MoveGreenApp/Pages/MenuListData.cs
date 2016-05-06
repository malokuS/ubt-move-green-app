using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace MoveGreenApp
{

	public class MenuListData : List<MenuItem>
	{
		public MenuListData ()
		{
			this.Add (new MenuItem () { 
				Title = "Map", 
				IconSource = "map.png", 
				TargetType = typeof(MapPage)
			});

			this.Add (new MenuItem () { 
				Title = "WiFi", 
				IconSource = "wifi.png", 
				TargetType = typeof(WiFiPage)
			});

			this.Add (new MenuItem () { 
				Title = "My Stats", 
				IconSource = "stats.png", 
				TargetType = typeof(MyStatsPage)
			});

			this.Add (new MenuItem () {
				Title = "Manage",
				IconSource = "settings.png",
				TargetType = typeof(ManagePage)
			});
		}
	}
}