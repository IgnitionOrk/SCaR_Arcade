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
    [Activity(Label = "GameMenuActivity")]
    public class GameMenuActivity : Activity
    {
        private Button btnStart;
        private Button btnLeaderBoard;
        private Button btnGameSelect;
        private int gameChoice;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.GameMenu);
            btnStart = FindViewById<Button>(Resource.Id.btnStart);
            btnLeaderBoard = FindViewById<Button>(Resource.Id.btnLeaderBoard);
            btnGameSelect = FindViewById<Button>(Resource.Id.btnGameSelect);
            gameChoice = Convert.ToInt32(Intent.GetStringExtra("gameChoice"));

            //--------------------------------------------------------------------
            btnStart.Click += Button_Click_Start;
            btnGameSelect.Click += Button_Click_Select;
        }
        //--------------------------------------------------------------------
        protected void Button_Click_Start(Object sender, EventArgs args)
        {
            try
            {
                Intent intent = null;
                switch (gameChoice)
                {
                    case 1:
                        intent = new Intent(this, typeof(TowerOfHanoiActivity));
                        StartActivity(intent);
                        break;
                    case 2:
                        //implement the memory card game;
                        break;
                }
            }
            catch
            {

            }
        }
        //--------------------------------------------------------------------
        protected void Button_Click_Select(Object sender, EventArgs args)
        {
            try
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            }
            catch
            {

            }
        }

    }
}