using System;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;

namespace MoveGreenApp
{
	public class WiFiManager
	{
		private bool isConnected = false, alreadyConnected = false;
		IWifiCustomManager wifiCM;
		private const string BUS_SSID = "", BUS_PASS = "";
		private List<string> scanResults;

		public WiFiManager ()
		{
			CrossConnectivity.Current.ConnectivityChanged += (sender, e) => 
			{
				isConnected = e.IsConnected;
			};

			wifiCM = DependencyService.Get<IWifiCustomManager> ();
		}

		public delegate void ChangedConnectionEventHandler(object source, EventArgs args);

		public event ChangedConnectionEventHandler ChangedConnection;

		protected virtual void OnChangedConnection()
		{
			if (ChangedConnection != null)
				ChangedConnection (this, EventArgs.Empty);
			else
				Debug.WriteLine ("No subscribers!");
		}

		public bool getConnectionStatus()
		{
			return isConnected;
		}

		public void RefreshConnectionStatus()
		{
			isConnected = CrossConnectivity.Current.IsConnected;
		}

		public List<string> GetConnectionTypes()
		{
			List<string> conTypes = new List<string> ();

			foreach (var band in CrossConnectivity.Current.ConnectionTypes)
			{
				conTypes.Add (band.ToString ());
			}

			return conTypes;
		}

		public async Task<bool> IsReachable(string host)
		{
			bool result = false;

			RefreshConnectionStatus ();

			try
			{
				if(isConnected)
					result = await CrossConnectivity.Current.IsReachable(host);
			}
			catch (Exception ex)
			{
				
			}

			return result;
		}

		public async Task<bool> IsRemoteReachable(string host, int port)
		{
			bool result = false;

			RefreshConnectionStatus ();

			try
			{
				if(isConnected)
					result = await CrossConnectivity.Current.IsRemoteReachable(host, port);
			}
			catch (Exception ex)
			{

			}

			return result;
		}

		public string GetSSID()
		{
			string ssid = wifiCM.GetSSID ();

			return ssid;
		}

		public bool ConnectToSSID(string ssid, string password)
		{
			bool result = false;
			if (GetIsWiFiEnabled ())
			{
				result = wifiCM.JoinWifi (ssid, password);
			}

			if (result)
			{
				OnChangedConnection ();
			}

			return result;
		}

		public bool GetIsWiFiEnabled()
		{
			return wifiCM.IsWiFiEnabled ();
		}

		public void ScanAvailableWifi()
		{
			if (GetIsWiFiEnabled ())
			{
				bool scan = wifiCM.ScanAvailableWiFi ();
			}
		}

		public void GetScanResults()
		{
			scanResults = wifiCM.GetAvailableWiFi ();

			ConnectToBusSSID ();
		}

		public bool ConnectToBusSSID()
		{
			bool result = false;
			
			if (scanResults.Contains (BUS_SSID) && !AlreadyConnectedToBusSSID ())
			{
				result = ConnectToSSID (BUS_SSID, BUS_PASS);
			}

			return result;
		}

		public bool AlreadyConnectedToBusSSID()
		{
			alreadyConnected = false;

			if (isConnected)
			{
				string ssid = wifiCM.GetSSID ();
				if (ssid.Equals (BUS_SSID))
				{
					alreadyConnected = true;
				}
				else
				{
					alreadyConnected = false;
				}
			}

			return alreadyConnected;
		}
	}
}