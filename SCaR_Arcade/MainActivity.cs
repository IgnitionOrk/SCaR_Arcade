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
        /*sources
        * http://blog.atavisticsoftware.com/2014/02/listview-using-activitylistitem-style.html
        * http://blog.atavisticsoftware.com/2014/01/listview-basics-for-xamarain-android.html
       */
        private ListView lvGameList;

        protected override void OnCreate(Bundle bundle)
        {
           base.OnCreate(bundle);
            

            SetContentView (Resource.Layout.Main);
            lvGameList = FindViewById<ListView>(Resource.Id.lvGameList);
            lvGameList.Adapter = new GameAdapter(this);
            
            
            
        }

        private void MoveToGameMenu(AdapterView.ItemClickEventArgs args)
        {
            Intent intent = new Intent(this,typeof(GameMenuActivity));
            intent.PutExtra("gameChoice", args.Position);
            StartActivity(intent);
        }
    }
}

