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
    class GameAdapter:BaseAdapter<Game>
    {

        /*sources
         * http://blog.atavisticsoftware.com/2014/02/listview-using-activitylistitem-style.html
         * http://blog.atavisticsoftware.com/2014/01/listview-basics-for-xamarain-android.html
        */
        private List<Game> data;
        private Activity context;

        public GameAdapter (Activity activity)
        {
            context = activity;

            data = PopulateGameData();
        }

        public override Game this[int position]
        {
            get { return data[position]; } 
        }

        public override long GetItemId(int position)
        {
            return position; 
        }

        public override int Count
        {
            get { return data.Count; } 
        }

        public override View GetView(int position, View rowView, ViewGroup parent)
        {


            var view = rowView;
            if (view == null)
            {

                view = context.LayoutInflater.Inflate(Android.Resource.Layout.ActivityListItem, null);
            }

            var game = data[position];

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = game.gTitle;
            ImageView img = view.FindViewById<ImageView>(Android.Resource.Id.Icon);
            img.SetImageResource(game.gLogo);
            return view;
        }

        //TODO: list into and fill with proper data
        private List<Game> PopulateGameData()
        {
            return new List<Game>()
            {   

                new Game { gTitle = "Game 1", gLogo = Resource.Drawable.game1 },
                new Game { gTitle = "Game 2", gLogo = Resource.Drawable.game2 },
                new Game { gTitle = "Game 3", gLogo = Resource.Drawable.game3 },
            };

        }

        

    }

}