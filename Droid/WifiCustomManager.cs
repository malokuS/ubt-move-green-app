using System;
using Android;
using Android.App;
using Android.Net;
using Android.Net.Wifi;
using Android.Content;
using System.Diagnostics;
using Xamarin.Forms;
using MoveGreenApp;
using MoveGreenApp.Droid;
using System.Collections.Generic;
using System.Threading.Tasks;

[assembly:Dependency(typeof(WifiCustomManager))]
namespace MoveGreenApp.Droid
{
	public class WifiCustomManager : IWifiCustomManager
	{
		private static WifiManager wifiManager;
		private static List<string> wifiList;
		private WifiReceiver wifiReceiver;
		private static bool connectResult = true;
		private static WiFiManager myWiFiManager;
		private int configID = -1;

		#region IWifiCustomManager implementation

		public bool IsWiFiEnabled()
		{
			wifiManager = Forms.Context.GetSystemService(Context.WifiService) as WifiManager;
			if(wifiManager == null)
				return false;

			return wifiManager.IsWifiEnabled;
		}
			
		public string GetSSID()
		{
			wifiManager = Forms.Context.GetSystemService(Context.WifiService) as WifiManager;
			if(wifiManager == null)
				return string.Empty;

			WifiInfo wifiInfo = wifiManager.ConnectionInfo;

			return wifiInfo.SSID;
		}

		public bool JoinWifi(string ssid, string password)
		{
			try
			{
				WifiConfiguration wifiConfig = new WifiConfiguration();
				wifiConfig.Ssid = String.Format("\"{0}\"", ssid);
				wifiConfig.PreSharedKey = String.Format("\"{0}\"", password);

				wifiManager = Forms.Context.GetSystemService(Context.WifiService) as WifiManager;

				if(wifiManager == null)
					return false;
				
				if(!IsConfigured(wifiConfig.Ssid))
				{
					var netId = wifiManager.AddNetwork(wifiConfig);
					Debug.WriteLine("Trying to add network : " + netId);
					if(netId != -1)
					{
						var connectResult = wifiManager.EnableNetwork(netId, true);
						Debug.WriteLine("Trying to connect : " + connectResult);
						if(!connectResult)
						{
							Debug.WriteLine("Unknown error while connecting!");
							return false;
						}
						else
						{
							var result = wifiManager.SaveConfiguration();
							Debug.WriteLine("Trying to save configuration : " + result);
							if(!result)
							{
								Debug.WriteLine("Unknown error while calling WiFiManager.SaveConfiguration();");
								return false;
							}
						}
					}
					else
					{
						Debug.WriteLine("Unknown error while calling WiFiManager.AddNetwork();");
						return false;
					}
				}else{
					var connectResult = wifiManager.EnableNetwork(configID, true);
					Debug.WriteLine("Trying to connect to known SSID : " + connectResult);
					if(!connectResult)
					{
						Debug.WriteLine("Unknown error while connecting!");
						return false;
					}
				}
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Exception raised while trying to join wifi  : " + ex);
				return false;
			}

			return true;
		}

		public bool IsConfigured(string ssid)
		{
			var wifiManager = Forms.Context.GetSystemService(Context.WifiService) as WifiManager;
			if(wifiManager == null)
				return false;

			var finalSsid = ssid;//string.Format("\"{0}\"", ssid);
			foreach (var id in wifiManager.ConfiguredNetworks)
			{
				if (id == null)
					continue;


				if (string.IsNullOrWhiteSpace (id.Ssid))
					continue;

				if (id.Ssid.Equals (finalSsid, StringComparison.InvariantCultureIgnoreCase))
				{
					configID = id.NetworkId;
					Debug.WriteLine ("Found Network : " + configID + " ; " + id.Ssid);
					return true;
				}
			}

			Debug.WriteLine ("Couldn't find Network !!! : " + finalSsid);
			return false;
		}
			
		public bool ScanAvailableWiFi()
		{
			wifiManager = Forms.Context.GetSystemService(Context.WifiService) as WifiManager;
			if(wifiManager == null)
				return false;

			wifiList = new List<string> ();
			connectResult = false;

			wifiReceiver = new WifiReceiver ();
			Forms.Context.RegisterReceiver (wifiReceiver, new IntentFilter (WifiManager.ScanResultsAvailableAction));
			wifiManager.StartScan ();

			return connectResult;
		}

		/// <summary>
		/// Consider calling ScanAvailableWiFi() before getting list, for fresh results!
		/// </summary>
		public List<string> GetAvailableWiFi()
		{
			return wifiList;	
		}

		public bool ConnectResult()
		{
			return connectResult;
		}

		#endregion

		class WifiReceiver : BroadcastReceiver
		{
			public override void OnReceive(Context context, Intent intent)
			{
				IList<ScanResult> scanwifinetworks = wifiManager.ScanResults;
				foreach(ScanResult wifinetwork in scanwifinetworks)
				{
					wifiList.Add(wifinetwork.Ssid);
				}

				myWiFiManager = new WiFiManager ();

				myWiFiManager.GetScanResults ();
			}
		}
	}
}