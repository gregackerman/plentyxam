using System;

using Xamarin.Forms;

namespace Plenty
{
	public class OffersModel 
	{
		public OffersModel (string name, string description, string image)
		{
			this.Name = name;
			this.Description = description;
			this.Image = image;
		}

		public string Name { private set; get; }

		public string Description { private set; get; }

		public string Image { private set; get; }
	}
}


