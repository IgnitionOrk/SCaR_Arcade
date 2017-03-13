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

namespace SCaR_Arcade
{
    public class ImageAdapter : BaseAdapter
    {
        private Context context;

        // Image references:
        int[] thumbIds =
        {
            Resource.Drawable.game1, Resource.Drawable.game2, Resource.Drawable.game3
        };

        public ImageAdapter (Context c)
        {
            context = c;
        }

        public override int Count
        {
            get
            {
                return thumbIds.Length;
            }
        }

        public override Java.Lang.Object GetItem (int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        // Create a new ImageView for each item referenced by the Adapter:
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ImageView imageView;

            if(convertView == null) // If the view isn't recycled, initialize some attributes:
            {
                imageView = new ImageView(context);
                imageView.LayoutParameters = new GridView.LayoutParams(300, 300);
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
    }
}