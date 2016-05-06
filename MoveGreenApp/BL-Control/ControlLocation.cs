using System;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Diagnostics;
using System.IO;

namespace MoveGreenApp
{
	public class ControlLocation
	{
		public bool gotPosition = false;
		IOStorageControler ioSC = new IOStorageControler();
		private const String LOC_FILE = "loc.txt";
		private IGeolocator locator;

		public ControlLocation ()
		{

		}

		private void GetLastSavedPos()
		{
			string file, tempLat = "", tempLon = "";
			var previousL = ioSC.ReadFile <String>(LOC_FILE);
			file = previousL.ToString ();
			Debug.WriteLine ("Previous saved LOC : " + file);

			bool gotLat = false;
			for (int i = 0; i < file.Length; i++) 
			{
				if (file [i].Equals (":") && !gotLat)
				{
					for (int j = i; j < file.Length ; j++) 
					{
						if (!file [j].Equals (",") && !gotLat)
						{
							tempLat += file [++j];
						} else if (file [j].Equals (",") && !gotLat) 
						{
							gotLat = true;
						}
					}
				}else if(file [i].Equals (":") && gotLat)
				{
					for (int z = i; z < file.Length ; z++) 
					{
						tempLon += file [++z];
					}
				}
			}

			Debug.WriteLine ("LAT : " + tempLat + " ; LON : " + tempLon);
		}

		private async Task WriteCurrentPos()
		{
			string data = "Lat:" + Latitude + ", Lon:" + Longitude;

			bool write = await ioSC.WriteFile(LOC_FILE, data);
		}

		public async Task<bool> GetCurrentPos()
		{
			locator = CrossGeolocator.Current;

			if (locator.IsGeolocationAvailable) 
			{
				gotPosition = await GetPosition ();	
			} 
			else if(!locator.IsGeolocationEnabled)
			{
				//Prompt to turn on geoLoc.
			}

			return gotPosition;
		}

		private async Task<bool> GetPosition()
		{
			Position position = null;

			try
			{ 
				locator.DesiredAccuracy = 5;

				position = await locator.GetPositionAsync (5000);

				Debug.WriteLine ("Position Status: {0}", position.Timestamp);
				Debug.WriteLine ("Position Latitude: {0}", position.Latitude);
				Debug.WriteLine ("Position Longitude: {0}", position.Longitude);
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
				gotPosition = false;
			}

			if (position != null) 
			{
				Latitude = position.Latitude;
				Longitude = position.Longitude;
				Timestamp = position.Timestamp.DateTime;
				gotPosition = true;
			} else {
				gotPosition = false;
			}
			return gotPosition;
		}

		public DateTime Timestamp 
		{
			get;
			set;
		}

		public double Latitude 
		{
			get;
			set;
		}

		public double Longitude 
		{
			get;
			set;
		}
	}
}