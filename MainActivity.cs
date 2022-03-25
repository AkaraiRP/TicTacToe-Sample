using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;

namespace _03LMS1_Briones
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Button EasyButton = FindViewById<Button>(Resource.Id.Easy);
            Button MediumButton = FindViewById<Button>(Resource.Id.Medium);
            Button HardButton = FindViewById<Button>(Resource.Id.Hard);

            EasyButton.Click += Easy;
            MediumButton.Click += Medium;
            HardButton.Click += Hard;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void Medium(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Game));
            intent.PutExtra("Difficulty", "medium");
            StartActivity(intent);
        }

        private void Easy(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Game));
            intent.PutExtra("Difficulty", "easy");
            StartActivity(intent);
        }

        private void Hard(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Game));
            intent.PutExtra("Difficulty", "hard");
            StartActivity(intent);
        }
    }
}