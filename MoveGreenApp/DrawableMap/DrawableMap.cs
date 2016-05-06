using System;
using System.Collections.Generic;

using Xamarin.Forms.Maps;

namespace MoveGreenApp
{
	public class DrawableMap : Map
	{
		/// <summary>
		/// Returns cross-platform map that can have Polylines added to it
		/// </summary>
		public DrawableMap() 
		{
		}
		/// <summary>
		/// Returns cross-platform map that can have Polylines added to it. 
		/// The start and end destination pins will be added to the Polyline
		/// </summary>
		public DrawableMap (Pin startDestination, Pin endDestination)
		{
			Pins.Add (startDestination);
			Pins.Add (endDestination);
		}

		/// <summary>
		/// List of positions will draw Polyline on map
		/// </summary>
		public List<Position> GeoData {
			get;
			set;
		}
	}
}