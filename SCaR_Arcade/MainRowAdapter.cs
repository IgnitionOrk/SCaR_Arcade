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

/// <summary>
/// Creator: Ryan Cunneen
/// Student number: 3179234
/// Creator: Martin O'Connor
/// Student number: 3279660
/// Date modified: 21-Mar-2017
/// /// Date created: 21-Mar-2017
/// </summary>

namespace SCaR_Arcade
{
    class MainRowAdapter:BaseAdapter<Game>
    {
        /*
         * sources
         * http://blog.atavisticsoftware.com/2014/02/listview-using-activitylistitem-style.html
         * http://blog.atavisticsoftware.com/2014/01/listview-basics-for-xamarain-android.html
         */  
        private List<Game> data;
        private Activity context;

        // ----------------------------------------------------------------------------------------------------------------
        // Constructor:
        public MainRowAdapter (Activity activity)
        {
            context = activity;
            populateGameData();
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Defined method signature by BaseAdapter interface.
        public override Game this[int position]
        {
            get { return data[position]; } 
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Defined method signature by BaseAdapter interface.
        public override long GetItemId(int position)
        {
            return position; 
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Defined method signature by BaseAdapter interface.
        public override int Count
        {
            get
            {
                if (data == null)
                {
                    // Why are we returning 1, when data is null (it would therefore have 0 Count)
                    // We are returning 1, because we can then insert a TextView called noConnection that will have the message 'No games in list'. 
                    return 1;
                }
                else
                {
                    return data.Count;
                }
            } 
        }
        // ----------------------------------------------------------------------------------------------------------------
        public override View GetView(int position, View rowView, ViewGroup parent)
        {
            var view = rowView;

            if (view == null)
            {
                view = context.LayoutInflater.Inflate(SCaR_Arcade.Resource.Layout.MainRow, null);
            }

            if(data == null)
            {
                // Essentially this would have been used if the user had the ability to add or remove Games.
                // add text colom details down list
                TextView noConnection = view.FindViewById<TextView>(SCaR_Arcade.Resource.Id.titletxt);
                noConnection.Text = "No games in list.";

                noConnection.LayoutParameters = new LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MatchParent,
                    LinearLayout.LayoutParams.WrapContent
                );
            }
            else
            {
                var game = data[position];

                // add text and images down list
                TextView txt = view.FindViewById<TextView>(SCaR_Arcade.Resource.Id.titletxt);
                txt.Text = game.gTitle;

                ImageView img = view.FindViewById<ImageView>(SCaR_Arcade.Resource.Id.logo);
                img.SetImageResource(game.gLogo);
            }
            return view;
        }
        // ----------------------------------------------------------------------------------------------------------------
        private void populateGameData()
        {

            if (data == null)
            {
                data = GameInterface.getGames();
            }
        }
    }
}