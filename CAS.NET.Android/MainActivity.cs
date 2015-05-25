using System;
using Ast;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace CAS.NET.Android
{
	[Activity (Label = "CAS.NET.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		TextView textviewinput;
		TextView textviewoutput;
		Evaluator eval = new Evaluator();


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			textviewinput = FindViewById<TextView> (Resource.Id.input);
			textviewoutput = FindViewById<TextView> (Resource.Id.output);
			Button evalButton = FindViewById<Button> (Resource.Id.evalbutton);

			evalButton.Click += delegate {
				EvaluateInput();
			};
		}

		void EvaluateInput()
		{
			if (textviewinput.Text.Length == 0) {
				textviewoutput.Text = "No Input!";
			}

			eval.Parse (textviewinput.Text);

			var res = eval.Evaluate ();

			if(!(res == null || res.GetType() == typeof(Error)))
			{
				textviewoutput.Text = res.ToString();
			}
		}
	}
}


