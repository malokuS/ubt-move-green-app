using System;

namespace MoveGreenApp
{
	public class DirectionsModel
	{
		/// <summary>
		/// Returns directions in a string array with start and end destinations
		/// </summary>
		public string[] Directions {
			get;
			set;
		}
		/// <summary>
		/// Returns distance in a string array. Distance is in Meters.
		/// </summary>
		public string[] DistanceInM {
			get;
			set;
		}
		/// <summary>
		/// Returns turn by turn directions in a string array. This is only available on Android through the Google API
		/// </summary>
		public string[] Duration {
			get;
			set;
		}
	}
}