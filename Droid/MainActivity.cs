using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plenty;
using System.Collections.Generic;

using Worklight;
using Worklight.Xamarin.Android;
using System.Threading.Tasks;
using System.Text;
using System.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;




namespace Plenty.Droid
{
	[Activity (Label = "Plenty.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		//STEP 1: update URL_STRONGLOOP according to the VM IP by calling ipconfig command
		public static string URL_STRONGLOOP = "http://192.168.0.105:3000/api/Offers"; 
		//STEP 2: update Plenty.Droid/Assets/wlclient.properties - the field wlServerHost to point to the VM IP
		public static IWorklightClient mfpClient = null;
		public bool isInitialCall = true;

	

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			//initializing MobileFirst Foundation 
			setOffers ();

			global::Xamarin.Forms.Forms.Init (this, bundle);

			LoadApplication (new App ());
		}
	

		//mfp initialilzing environment
		public async void setOffers ()
		{
			var isConnected = false;

			//mobileFirst client
			if (mfpClient == null) {
				mfpClient = WorklightClient.CreateInstance (this);
			}

			System.Console.WriteLine ("MFP before connect");

			//getting connection
			WorklightResponse task1 = await mfpClient.Connect ();
			if (task1.Success) {
				Console.WriteLine ("connected to MFP");
				isConnected = true;
			} else {
				Console.WriteLine ("connection failed");
			} 

			//accessing protected feedAdapter
			if (isConnected) {

				//adding challengehandler
				string appRealm = "PlentyAppRealm";
				ChallengeHandler customCH = new CustomChallengeHandler (appRealm);
				mfpClient.RegisterChallengeHandler (customCH);
				System.Console.WriteLine ("MFP before adapter");

				WorklightProcedureInvocationData invocationData = 
					new WorklightProcedureInvocationData (
						"FeedAdapter", "getFeed", new object[] { "" });
				
				WorklightResponse task3 = await mfpClient.InvokeProcedure (invocationData);
				if (task3.Success) {
					isInitialCall = false;
					Console.WriteLine ("adapter connected");
					Console.WriteLine (task3.ResponseJSON.ToString ());
				} else {
					Console.WriteLine ("adapter failed");
					Console.WriteLine (task3.ResponseJSON.ToString ());
				}
			
			}
			//end of protected feedAdapter


			task1 = await mfpClient.Connect ();

			if (task1.Success) {
				Console.WriteLine ("connected to MFP");
				isConnected = true;
			} else {
				Console.WriteLine ("connection failed");
			} 


			//ACCESSING STRONGLOOP
			if (isConnected) {
				

				//adding challengehandler
				string appRealm = "PlentyAppRealm";
				ChallengeHandler customCH = new CustomChallengeHandler (appRealm);
				mfpClient.RegisterChallengeHandler (customCH);
				System.Console.WriteLine ("MFP before SL url");

				//calling protected url (strongLoop)
				WorklightResourceRequest resourceRequest = mfpClient.ResourceRequest (
					                                           new Uri (URL_STRONGLOOP), "GET");
				WorklightResponse task2 = await resourceRequest.Send ();

				if (task2.Success) {
					Console.WriteLine ("strongloop url connected");
					Console.WriteLine (task2.ResponseText.ToString ());

					Plenty.Offers.offers = new List<OffersModel> {};

					//getting offers from JSON
					JsonArray jsonArray = new JsonArray (task2.ResponseText);
					Console.WriteLine (jsonArray.Count);
					var myObjectList = 
						(List<Offer>)
						JsonConvert.DeserializeObject (task2.ResponseText, typeof(List<Offer>));
					foreach (Offer offer in myObjectList) {
						/*
						Console.WriteLine (offer.offername);
						Console.WriteLine (offer.offerdescription);
						Console.WriteLine (offer.offerpicture);
						*/

						Plenty.Offers.offers.Add (new OffersModel (
							offer.offername,
							offer.offerdescription,
							offer.offerpicture));

					}
				} else {
					Console.WriteLine ("strongloop url failed");
					//dummy offers
					Plenty.Offers.offers = new List<OffersModel> {
						new OffersModel ("potatoes", "For offers refer", "Potatoes.jpg"),
						new OffersModel (
							"Buy one get one free on salted nuts for the big game!",
							"This offer entitles you to a free can of salted nuts for every can you purchase.  Limit 4.",
							"offers/asparagus.jpg"),
					};
				}
			} else {
				Console.WriteLine ("connection for SL failed");
				//dummy offers
				Plenty.Offers.offers = new List<OffersModel> {
					new OffersModel (
						"Buy one get one free on salted nuts for the big game!",
						"This offer entitles you to a free can of salted nuts for every can you purchase.  Limit 4.",
						"NUTS.jpg"),
				};
			}
		}

	}

	//helper class for the Json deserialization automation
	public class Offer
	{
		[JsonProperty ("id")]
		public int id { get; set; }

		[JsonProperty ("loyaltyid")]
		public int loyaltyid { get; set; }

		[JsonProperty ("offerdescription")]
		public string offerdescription { get; set; }

		[JsonProperty ("offername")]
		public string offername { get; set; }

		[JsonProperty ("offerpicture")]
		public string offerpicture { get; set; }

	}
}
