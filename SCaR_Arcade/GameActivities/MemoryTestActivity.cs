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

// Created by: Saxon Colbert-Smith 
// Student Number 3230355
// created: 28-Apr-17
// last modified: 29-Apr-17
//Code adapted from https://developer.xamarin.com/guides/android/user_interface/grid_view/

namespace SCaR_Arcade.GameActivities
{
    [Activity(
              Label = "MemoryTestActivity",
              MainLauncher = false,
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape,
              Theme = "@android:style/Theme.NoTitleBar"
            )
   ]
    public class MemoryTestActivity : Activity
    {
        // Instances for Memory Test
        //----------------
        //----------------
        //----------------
    
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.MemoryTest);

            var gridview = FindViewById<GridView>(Resource.Id.gridview);
            gridview.Adapter = new ImageAdapter(this);

            gridview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args) {
                Toast.MakeText(this, args.Position.ToString(), ToastLength.Short).Show();

            };
        }

        public class ImageAdapter : BaseAdapter
        {
            Context context;

            public ImageAdapter(Context c)
            {
                context = c;
            }

            public override int Count
            {
                get { return thumbIds.Length; }
            }

            public override Java.Lang.Object GetItem(int position)
            {
                return null;
            }

            public override long GetItemId(int position)
            {
                return 0;
            }

            // create a new ImageView for each item referenced by the Adapter
            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                ImageView imageView;

                if (convertView == null)
                {  // if it's not recycled, initialize some attributes
                    imageView = new ImageView(context);
                    imageView.LayoutParameters = new GridView.LayoutParams(85, 85);
                    imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
                    imageView.SetPadding(8, 8, 8, 8);
                }
                else
                {
                    imageView = (ImageView)convertView;
                }

                imageView.SetImageResource(thumbIds[position]);
                return imageView;
            }

            // references to our images
            int[] thumbIds = {
        
                //Add pathways to images for the game here, eg:
        //Resource.Drawable.sample_2, Resource.Drawable.sample_3,
        //Resource.Drawable.sample_4, Resource.Drawable.sample_5,
        //Resource.Drawable.sample_6, Resource.Drawable.sample_7,
        //Resource.Drawable.sample_0, Resource.Drawable.sample_1,
        //Resource.Drawable.sample_2, Resource.Drawable.sample_3,
        //Resource.Drawable.sample_4, Resource.Drawable.sample_5,
        //Resource.Drawable.sample_6, Resource.Drawable.sample_7,
        //Resource.Drawable.sample_0, Resource.Drawable.sample_1,
        //Resource.Drawable.sample_2, Resource.Drawable.sample_3,
        //Resource.Drawable.sample_4, Resource.Drawable.sample_5,
        //Resource.Drawable.sample_6, Resource.Drawable.sample_7
    };
        }

    }
}