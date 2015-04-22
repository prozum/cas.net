using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Ast;

namespace Android
{
	[Activity (Label = "Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button1 = FindViewById<Button>(Resource.Id.Button1);
			EditText field = FindViewById<EditText>(Resource.Id.Field);

			button1.Click += (object sender, EventArgs e) =>
			{
				var Dialog = new AlertDialog.Builder(this);

				Dialog.SetMessage(field.Text);

				Dialog.Show();
			};
		}
	}
}


