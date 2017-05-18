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
    class LeaderBoardRowAdapter:BaseAdapter<LeaderBoard>
    {
        /*
         * sources
         * 
         * http://blog.atavisticsoftware.com/2014/02/listview-using-activitylistitem-style.html
         * http://blog.atavisticsoftware.com/2014/01/listview-basics-for-xamarain-android.html
        */
        private List<LeaderBoard> data;
        private Activity context;
        // ----------------------------------------------------------------------------------------------------------------
        // Constructor:
        public LeaderBoardRowAdapter (Activity activity)
        {
            this.context = activity;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Constructor:
        public LeaderBoardRowAdapter(Activity activity, bool isOnline)
        {
            context = activity;

            // the boolean parameter determines if we are using the local, or online .text files.
            // Here we are working with the default which is local (false).
            PopulateLeaderBoardData(isOnline);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Defined method signature by BaseAdapter interface.
        public override LeaderBoard this[int position]
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
                if(data == null)
                {
                    // Why are we returning 1, when data is null (it would therefore have 0 Count)
                    // We are returning 1, because we can then insert a TextView called noConnection that will have the message 'No internet connection'. 
                    return 1;
                }
                else
                {
                    return data.Count;
                }
            } 
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Populates the LeaderBoardRow with Views determined by the data == null condition.
        public override View GetView(int position, View rowView, ViewGroup parent)
        {
            var view = rowView;

            if (view == null)
            {
                view = context.LayoutInflater.Inflate(SCaR_Arcade.Resource.Layout.LeaderBoardRow, null);
            }

            if (data == null)
            {
                //add text colom details down list
                TextView noConnection = view.FindViewById<TextView>(SCaR_Arcade.Resource.Id.positiontxt);
                noConnection.Text = "No Internet connection";


                noConnection.LayoutParameters = new LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MatchParent,
                    LinearLayout.LayoutParams.WrapContent
                );
            }
            else
            {
                var row = data[position];

                //add text colom details down list
                TextView leaderboardPosition = view.FindViewById<TextView>(SCaR_Arcade.Resource.Id.positiontxt);
                leaderboardPosition.Text = Convert.ToString(row.lbPosition);

                TextView name = view.FindViewById<TextView>(SCaR_Arcade.Resource.Id.nametxt);
                name.Text = row.lbName;

                TextView diff = view.FindViewById<TextView>(SCaR_Arcade.Resource.Id.difftxt);
                diff.Text = Convert.ToString(row.lbDiff);

                TextView score = view.FindViewById<TextView>(SCaR_Arcade.Resource.Id.scoretxt);
                score.Text = Convert.ToString(row.lbScore);

                TextView time = view.FindViewById<TextView>(SCaR_Arcade.Resource.Id.timetxt);
                time.Text = row.lbTime;

            }
            return view;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Populates the Leader board with data of scores that are either from the local, or online text files.
        private void PopulateLeaderBoardData(bool isOnline)
        {
            data = LeaderBoardInterface.PopulateLeaderBoardData(isOnline);
        }
    }
}