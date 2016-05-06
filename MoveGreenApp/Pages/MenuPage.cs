using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Diagnostics;

namespace MoveGreenApp
{
	public class MenuPage : ContentPage
	{
		public ListView Menu { get; set; }
		public String title = "UBT College", desc = "Computer Sciences and Engineering";
		public String imgSource;

		public MenuPage ()
		{
			Icon = "menu.png";

			Title = "menu"; // The Title property must be set.
			BackgroundColor = Color.FromHex ("f5f5f5");

			Menu = new MenuListView ();

			var layoutS = new StackLayout { 
				Spacing = 0, 
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			RelativeLayout navHeaderRL = new RelativeLayout {
				BackgroundColor = Color.FromHex ("0D76FF"),
				HeightRequest = 170
			};

			imgSource = "user_ico.png";

			var userImgV = new ContentView {
				Content = new Image{
					HeightRequest = 72,
					WidthRequest = 72,
					Aspect = Aspect.AspectFit,
					Source = imgSource
				}
			};

			var nameLabel = new ContentView {
				Content = new Label {
					TextColor = Color.FromHex ("FFFFFF"),
					Text = title,
					FontSize = 14,
					HorizontalOptions = LayoutOptions.StartAndExpand
				}
			};

			var descLabel = new ContentView {
				Content = new Label {
					TextColor = Color.FromHex ("FFFFFF"),
					Text = desc,
					FontSize = 10,
					HorizontalOptions = LayoutOptions.StartAndExpand
				}
			};

			navHeaderRL.Children.Add (userImgV,
				Constraint.RelativeToParent ((Parent) => 15),
				Constraint.RelativeToParent ((Parent) => 50));

			navHeaderRL.Children.Add (nameLabel,                                      
				Constraint.RelativeToParent ((Parent) => 15),    
				Constraint.RelativeToParent ((Parent) => 130)); 

			navHeaderRL.Children.Add (descLabel,                                      
				Constraint.RelativeToParent ((Parent) => 15),    
				Constraint.RelativeToParent ((Parent) => 155));

			layoutS.Children.Add (navHeaderRL); // Add the Relative Layout Navigation Header

			layoutS.Children.Add (Menu); // Add Menu

			Content = layoutS;
		}
	}
}