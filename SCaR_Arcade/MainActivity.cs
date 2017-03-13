using Android.App;
using Android.Widget;
using Android.OS;

namespace SCaR_Arcade
{
    [Activity(Label = "SCaR_Arcade",
        MainLauncher = true,
        Icon = "@drawable/icon",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape,
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
                string saxy = "heyThere";
                Toast.MakeText(this, args.Position.ToString(), ToastLength.Short).Show();
            };
        }
    }
}

