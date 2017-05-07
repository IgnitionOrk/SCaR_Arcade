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
/// Created by: Martin O'Connor
/// Created by: Ryan Cunneen
/// Student number: 3279660
/// Student number: 3179234
/// Date modified: 21-Mar-2017
/// /// Date created: 21-Mar-2017
/// </summary>

namespace SCaR_Arcade
{
    class LeaderBoardRowAdapter:BaseAdapter<LeaderBoard>
    {
        // ----------------------------------------------------------------------------------------------------------------
        /*
         * sources
         * http://blog.atavisticsoftware.com/2014/02/listview-using-activitylistitem-style.html
         * http://blog.atavisticsoftware.com/2014/01/listview-basics-for-xamarain-android.html
        */
        private List<LeaderBoard> data;
        private Activity context;
        private Android.Content.Res.AssetManager assets;

        // ----------------------------------------------------------------------------------------------------------------
        // Constructor:
        public LeaderBoardRowAdapter (Activity activity, Android.Content.Res.AssetManager assets)
        {
            context = activity;
            this.assets = assets; 

            // the boolean parameter determines if we are using the local, or online .text files.
            // Here we are working with the default which is local (false).
            PopulateLeaderBoardData(false);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Constructor:
        public LeaderBoardRowAdapter(Activity activity, Android.Content.Res.AssetManager assets, bool isOnline)
        {
            context = activity;
            this.assets = assets;

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
            get { return data.Count; } 
        }
        // ----------------------------------------------------------------------------------------------------------------
        public override View GetView(int position, View rowView, ViewGroup parent)
        {
            var view = rowView;

            if (view == null)
            {
                view = context.LayoutInflater.Inflate(SCaR_Arcade.Resource.Layout.LeaderBoardRow, null);
            }

            var row = data[position];

            
            //add text colom details down list
            TextView leaderboardPosition = view.FindViewById<TextView>(SCaR_Arcade.Resource.Id.positiontxt);
            leaderboardPosition.Text = "P#"+ row.lbPosition; ;

            TextView name = view.FindViewById<TextView>(SCaR_Arcade.Resource.Id.nametxt);
            name.Text = "N#"+row.lbName;

            TextView time = view.FindViewById<TextView>(SCaR_Arcade.Resource.Id.timetxt);
            time.Text = "T#" + row.lbTime;

            TextView score = view.FindViewById<TextView>(SCaR_Arcade.Resource.Id.scoretxt);
            score.Text = "S#"+row.lbScore;


            return view;
        }
        // ----------------------------------------------------------------------------------------------------------------
        public void PopulateLeaderBoardData(bool isOnline)
        {
            if (data == null)
            {   // Local file is determined by the boolean parameter.
                List<string> unsortList = FileInterface.readFromScoreFile(isOnline, assets);
                // This list will be sorted;
                List<LeaderBoard> unsortedLb = new List<LeaderBoard>();
                for (int i = 0; i < unsortList.Count; i++)
                {
                    // Return the data (string) and index i;
                    string line = unsortList[i];

                    Char delimiter = '-';
                    String[] subStrings = line.Split(delimiter);

                    unsortedLb.Add(new LeaderBoard
                    {
                        lbPosition = Convert.ToInt32(subStrings[0]),
                        lbName = subStrings[1],
                        lbTime = subStrings[2],
                        lbScore = subStrings[3]
                    });
                }
                // Return a sorted Leaderboard list. 
                data = selectionSort(unsortedLb);
            }
        }

        // ----------------------------------------------------------------------------------------------------------------
        // We will sort the entire list here. 
        private List<LeaderBoard> selectionSort(List<LeaderBoard> sortedList)
        {
            int position = 0;
            LeaderBoard temp;
            for (int x = 0; x < sortedList.Count - 1; x++)
            {
                position = x;
                for (int y = x + 1; y < sortedList.Count; y++)
                {
                    if (sortedList[y].lbPosition < sortedList[position].lbPosition)
                    {
                        position = y;
                    }
                }
                if (position != x)
                {
                    temp = sortedList[x];
                    sortedList[x] = sortedList[position];
                    sortedList[position] = temp;
                }
            }
            return sortedList;
        }
    }
}