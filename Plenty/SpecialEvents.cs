using System;

using Xamarin.Forms;
using LoginModule;
namespace Plenty
{
	public class SpecialEvents : ContentPage
	{
		public SpecialEvents ()
		{
			Icon = "Events.png";
			Title = "Special Events";
			Content = new TableView { 
				Intent = TableIntent.Menu,
				Root = new TableRoot () { 

					new TableSection ("Meeting Times") {
						new ImageCell () {
							Text = "Monday 3:20 PM - 4:15 PM",
							Detail = "Fleming 24-106",
							ImageSource = new FileImageSource () { File = "Recommendations.png" }
						},
						new ImageCell () {
							Text = "Monday 3:20 PM - 4:15 PM",
							Detail = "Fleming 24-106",
							ImageSource = new FileImageSource () { File = "Recommendations.png" }
						},
						new ImageCell () {
							Text = "Monday 3:20 PM - 4:15 PM",
							Detail = "Fleming 24-106",
							ImageSource = new FileImageSource () { File = "Recommendations.png" }
						}
					}
				}
			};

			//To Enable Navigation on Navigation Bar
			/*ToolbarItems.Add(new ToolbarItem {
				Name = "Launch",
				Order = ToolbarItemOrder.Primary,
				Command = new Command(() => Navigation.PushAsync(new LoginPage(App.Current)))
			});*/
		}
	}
}


