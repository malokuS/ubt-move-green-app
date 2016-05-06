using System;

using Xamarin.Forms;

namespace MoveGreenApp
{
	public class App : Xamarin.Forms.Application
	{	

		public App()
		{
			MainPage = GetMainPage ();
		}

		public static Page GetMainPage ()
		{	
			return new RootPage ();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}