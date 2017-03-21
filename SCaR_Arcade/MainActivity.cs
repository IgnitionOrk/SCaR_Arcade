using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;

namespace SCaR_Arcade
{
    [Activity(Label = "SCaR_Arcade",
        MainLauncher = true,
        Icon = "@drawable/icon",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait,
        Theme = "@android:style/Theme.NoTitleBar")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            GridView gvGameList = FindViewById<GridView>(Resource.Id.gvGameList);
            gvGameList.Adapter = new ImageAdapter(this);

            gvGameList.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {
                //beginGame(args);

                Toast.MakeText(this, args.Position.ToString(), ToastLength.Short).Show();
            };
        }

        private void beginGame(AdapterView.ItemClickEventArgs args)
        {
            /*
              var game;
              Each case will correspond to a particular game. 
              switch(args.Position){
                 case 0:
                 game = new Intent(this, typeof(TowerOfHanoi));
                 StartActivity(game);
                 break;
                 case 1:
                 game = new Intent(this, typeof(MemoryChallenge));
                 StartActivity(game);
                 break;
                 case 2:
                 game = new Intent(this, typeof(BallMaze));
                 StartActivity(game);
                 break;
              }
              */
        }
    }
}

