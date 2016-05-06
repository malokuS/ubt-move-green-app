using System;

using Xamarin.Forms.Maps;

namespace MoveGreenApp
{
	public class DirectionsMap : Map
	{
		Position _startDestination;
		Position _endDestination;

		/// <summary>
		/// Returns cross-platform map. Start Destination and End Destination must be set to determine directions
		/// </summary>
		public DirectionsMap () {}

		/// <summary>
		/// Returns cross-platform map with directions from Start Desitination to End Destination as Positions
		/// </summary>
		public DirectionsMap (Position startDestination, Position endDestination)
		{
			_startDestination = startDestination;
			_endDestination = endDestination;
		}

		/// <summary>
		/// Returns cross-platform map with directions from Start Desitination to End Destination as Pins
		/// </summary>
		public DirectionsMap (Pin startDestination, Pin endDestination)
		{
			_startDestination = startDestination.Position;
			_endDestination = endDestination.Position;

			Pins.Add (startDestination);
			Pins.Add (endDestination);
		}

		/// <summary>
		/// Start destination generated from latitude and longitude.
		/// </summary>
		public Position StartDestination {
			get {
				return _startDestination;
			}
			set {
				_startDestination = value;
			}
		}
		/// <summary>
		/// End destination generated from latitude and longitude.
		/// </summary>
		public Position EndDestination {
			get {
				return _endDestination;
			}
			set { 
				_endDestination = value;
			}
		}
		/// <summary>
		/// Returns directions and distance as a array of strings. The distance is in feet. 
		/// Only Android will have access to turn by turn durations
		/// </summary>
		public DirectionsModel Directions {
			get;
			set;
		}
		/// <summary>
		/// Returns expected travel time in seconds
		/// </summary>
		public double ExpectedTravelTime {
			get;
			set;
		}
	}
}