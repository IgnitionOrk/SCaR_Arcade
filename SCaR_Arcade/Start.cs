using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
/// <summary>
/// Creator: Ryan Cunneen
/// Student number: 3179234
/// Date created: 25-Mar-17
/// Date modified: 30-Apr-17
/// </summary>
namespace SCaR_Arcade
{
    [Activity(
        Label = "Start",
        MainLauncher = true,
        Theme = "@android:style/Theme.NoTitleBar"
    )]
    public class Start : Activity
    {
        private ImageView appLogoImgView;
        private ProgressBar progress;
        private System.Timers.Timer timer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.Start);
                appLogoImgView = FindViewById<ImageView>(Resource.Id.ApplicationLogoImgView);
                progress = FindViewById<ProgressBar>(Resource.Id.startProgress);
                Bitmap appLogo = BitmapFactory.DecodeResource(Resources, Resource.Drawable.SCaRARCADE);
                appLogoImgView.SetImageBitmap(appLogo);
                timer = new System.Timers.Timer();
                timer.Interval = 3000;
                timer.Start();

                //BeginActivity(typeof(MainActivity), "", 0);
            }
            catch
            {
                GlobalApp.Alert(this, 1);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Begins the Activity specified by @param type.
        private void BeginActivity(Type type, string variableName, int value)
        {
            try
            {
                Intent intent = new Intent(this, type);
                if (type != typeof(MainActivity))
                {
                    intent.PutExtra(variableName, value);
                }
                StartActivity(intent);
            }
            catch
            {
                // because an error has happend at the Application level
                // We delegate the responsibility to the GlobalApp class.
                GlobalApp.Alert(this, 2);
            }
        }
    }
}