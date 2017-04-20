using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
/// <summary>
/// Creator: Ryan Cunneen
/// Creator: Martin O'Connell
/// Student number: 3179234
/// Student number: 3279660
/// Date created: 18-Mar-17
/// Date modified: 20-Apr-17
/// </summary>
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
        // ----------------------------------------------------------------------------------------------------------------
        protected override void OnCreate(Bundle bundle)
        {
           try
            {
                base.OnCreate(bundle);
                SetContentView(Resource.Layout.Main);
                lvGameList = FindViewById<ListView>(Resource.Id.lvGameList);


                lvGameList.Adapter = new GameAdapter(this);

                //on row click begin game menu
                lvGameList.ItemClick += listViewItemClick;
            }
            catch
            {
                GlobalApp.Alert(this, false, 0);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        private void listViewItemClick(Object sender, AdapterView.ItemClickEventArgs args)
        {
            try
            {
                Intent intent = new Intent(this, typeof(GameMenuActivity));
                intent.PutExtra(GlobalApp.getVariableChoiceName(), args.Position);
                StartActivity(intent);
            }
            catch
            {
                GlobalApp.Alert(this, false, 0);
            }
        }
    }
}

