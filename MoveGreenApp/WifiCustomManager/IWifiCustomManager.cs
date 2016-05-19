using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoveGreenApp
{
	public interface IWifiCustomManager
	{
		bool IsWiFiEnabled();
		string GetSSID();
		bool JoinWifi(string ssid, string password);
		bool IsConfigured(string ssid);
		bool ScanAvailableWiFi();
		List<string> GetAvailableWiFi();
		bool ConnectResult ();
	}
}