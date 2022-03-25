using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _03LMS1_Briones
{
    [Activity(Label = "EndGame")]
    public class EndGame : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.EndScreen);
            string Diff = Intent.GetStringExtra("Winner");

            Button RetryButton = FindViewById<Button>(Resource.Id.Retry);
            TextView Toaster = FindViewById<TextView>(Resource.Id.Toast);

            RetryButton.Click += Retry;

            if (Diff == "win")
            {
                //Nothing
            }

            else if (Diff == "lose")
            {
                Toaster.Text = "You Lost!";
            }

            else {
                Toaster.Text = "Tie!";
            }


        }

        private void Retry(object sender, EventArgs e)
        {
            this.Finish();
        }
    }
}