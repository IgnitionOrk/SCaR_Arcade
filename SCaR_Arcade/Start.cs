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
/// Creator: Martin O'Connor
/// Student number: 3179234
/// Student number: 3279660
/// Date created: 25-Mar-17
/// Date modified: 30-Apr-17
/// </summary>
namespace SCaR_Arcade
{
    [Activity(
        Label = "SCaR Arcade",
        MainLauncher = true,
        Icon = "@drawable/SCaRARCADE",
        Theme = "@android:style/Theme.NoTitleBar"
    )]
    public class Start : Activity
    {
        /*
       * sources
       * http://stackoverflow.com/questions/27196590/i-want-create-timer-thread-for-android-with-xamarin
       * https://developer.xamarin.com/guides/android/user_interface/creating_a_splash_screen/
       * https://forums.xamarin.com/discussion/58925/how-can-i-set-a-progressbar-in-android-using-cq
      */

        private ImageView appLogoImgView;
        private ProgressBar progress;
        System.Timers.Timer timer;
        int delay;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                base.
                SetContentView(Resource.Layout.Start);
                appLogoImgView = FindViewById<ImageView>(Resource.Id.ApplicationLogoImgView);
                progress = FindViewById<ProgressBar>(Resource.Id.startProgress);
                Bitmap appLogo = BitmapFactory.DecodeResource(Resources, Resource.Drawable.SCaRARCADE);
                appLogoImgView.SetImageBitmap(appLogo);
                delay = 3;      // Arbitrary number (3 seconds). 
                CountDown();
            }
            catch
            {
                GlobalApp.Alert(this, 1);
            }
        }
        // Initiates a timer, used to simulate the application loading. 
        private void CountDown()
        {
            timer = new System.Timers.Timer();            
            timer.Interval = 1000;
            timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;
        }
        // Event handler
        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            delay--;
            if (delay == 0)
            {
                timer.Dispose();
                BeginActivity(typeof(MainActivity), "", 0);
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
                // Because an error has happend at the application level
                // We delegate the responsibility to the GlobalApp class.
                GlobalApp.Alert(this, 2);
            }
        }
    }
}