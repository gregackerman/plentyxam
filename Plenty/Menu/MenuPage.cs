﻿using System;
using Xamarin.Forms;
using Plenty;
using System.Collections.Generic;

namespace Plenty
{
	public class MenuPage : ContentPage
	{
		public ListView Menu { get; set; }


		public MenuPage ()
		{
			Icon = "UserMenu.png";
			Title = "menu"; // The Title property must be set.
			//BackgroundColor = Color.FromHex ("333333");

			Menu = new MenuListView ();

			var menuLabel = new ContentView {
				Padding = new Thickness (10, 36, 0, 5),
				Content = new Label {
					TextColor = Color.FromHex ("AAAAAA"),
					Text = "Plenty", 
				}
			};

			var layout = new StackLayout { 
				Spacing = 0, 
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			layout.Children.Add (menuLabel);
			layout.Children.Add (Menu);
					

			/*var logoutButton = new Button { Text = "Logout",BackgroundColor=Color.Gray };
			logoutButton.Clicked += (sender, e) => {
				App.Current.Logout();
			};*/

			Content = new StackLayout {
				//BackgroundColor = Color.Gray,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {
					layout, 
					//logoutButton
				}
			};
		}


	}
}

