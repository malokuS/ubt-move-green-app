using System;

using Xamarin.Forms;
using System.Collections.Generic;

namespace MoveGreenApp
{
	public class DirectionsListPage : ContentPage
	{
		public class DirectionDistanceModel
		{
			public string Directions {
				get;
				set;
			}
			public string Distance {
				get;
				set;
			}
		}

		public DirectionsListPage (DirectionsModel directions)
		{
			ListView listOfDirections = new ListView {
				RowHeight = 50
			};

			List<DirectionDistanceModel> combo = new List<DirectionDistanceModel> ();
			for (var i = 0; i<directions.Directions.Length; i++)
			{
				bool inBounds = (i >= 0) && (i < directions.DistanceInM.Length);
				if(inBounds)
				{
					combo.Add (
						new DirectionDistanceModel { 
							Directions = directions.Directions [i],
							Distance = directions.DistanceInM [i]
						}
					);
				}
			}

			listOfDirections.ItemsSource = combo;
			listOfDirections.ItemTemplate = new DataTemplate (() => {
				Label directionLabel = new Label{
					HorizontalOptions = LayoutOptions.StartAndExpand,
				};
				directionLabel.SetBinding(Label.TextProperty,"Directions",BindingMode.OneWay,null,null);

				Label distanceLabel = new Label{
					HorizontalOptions = LayoutOptions.End
				};
				distanceLabel.SetBinding(Label.TextProperty,"Distance",BindingMode.OneWay,null,null);

				return new ViewCell {
					View = new StackLayout {
						Orientation = StackOrientation.Horizontal,
						Padding = new Thickness(5,0,5,0),
						Children = {
							directionLabel,
							distanceLabel
						}
					}
				};

			});

			Content = new StackLayout {
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = { listOfDirections }
			};
		}
	}
}