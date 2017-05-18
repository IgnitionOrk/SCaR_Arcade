using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Graphics;
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
    [Activity(
        Label = "LeaderBoardActivity", 
        MainLauncher = false,
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait,
        Theme = "@android:style/Theme.NoTitleBar")]
    public class LeaderBoardActivity : Activity
    {
        private LinearLayout FullScreen;
        private LinearLayout LeaderBoard;
        private LinearLayout LeaderBoardHeader;
        private LinearLayout middleLayout;
        private Button btnBack;
        private int gameChoice;
        private Game game;
        private ListView LeaderBoardListView;
        private Button localBtn;
        private Button onlineBtn;

        // ----------------------------------------------------------------------------------------------------------------
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.Leaderboard);
                FullScreen = FindViewById<LinearLayout>(Resource.Id.FullScreenLinLay);
                LeaderBoard = FindViewById<LinearLayout>(Resource.Id.LeaderBoardLinLay);
                LeaderBoardHeader = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
                LeaderBoardListView = FindViewById<ListView>(Resource.Id.LeaderBoardListView);
                middleLayout = FindViewById<LinearLayout>(Resource.Id.MiddleLinLay);
                btnBack = FindViewById<Button>(Resource.Id.btnGameSelect);
                localBtn = FindViewById<Button>(Resource.Id.btnLocal);
                onlineBtn = FindViewById<Button>(Resource.Id.btnOnline);

                var metrics = Resources.DisplayMetrics;
                middleLayout.LayoutParameters = new LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MatchParent,
                    (int)(metrics.HeightPixels * 0.75)
                );

                LeaderBoardListView.Adapter = new LeaderBoardRowAdapter(this, false);

                setBackgroundColorByClick(Color.DarkGray, Color.Gray, false);

                LeaderBoard.SetBackgroundColor(Color.Gray);
                LeaderBoardHeader.SetBackgroundColor(Color.LightGray);

                gameChoice = Intent.GetIntExtra(GlobalApp.getVariableChoiceName(), 0);

                game = GameInterface.getGameAt(gameChoice);

                if (game == null)
                {
                    throw new Exception();
                }

                FullScreen.SetBackgroundResource(game.gMenuBackground);

                // Event handlers.
                btnBack.Click += ButtonClickSelect;
                localBtn.Click += ButtonLocalClick;
                onlineBtn.Click += ButtonOnlineClick;
            }
            catch
            {
                GlobalApp.Alert(this, 0);
            }
        } 
        // ----------------------------------------------------------------------------------------------------------------
        // Returns to the Game Menu Activity of the application.
        protected void ButtonClickSelect(Object sender, EventArgs args)
        {
            GlobalApp.BeginActivity(this, typeof(GameMenuActivity), GlobalApp.getVariableChoiceName(), gameChoice);        
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Initializes the data for the Local leaderboard.
        protected void ButtonLocalClick(Object sender, EventArgs args)
        {
            initializeLeaderboardData(false);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Initializes the data for the online Leaderboard.
        protected void ButtonOnlineClick(Object sender, EventArgs args)
        {
            initializeLeaderboardData(true);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Initializes the data for the Leaderboard Local, and Online.
        // @param isOnline, must be either true, or false. 
        private void initializeLeaderboardData(bool isOnline)
        {

            try
            {
                if (isOnline)
                {
                    setBackgroundColorByClick(Color.DarkGray, Color.Gray, isOnline);
                    if (LeaderBoardInterface.hasInternetConnection())
                    {
                        // Delete the current Adpater
                        LeaderBoardListView.Adapter = null;
                        // And store a new one.
                        LeaderBoardListView.Adapter = new LeaderBoardRowAdapter(this, isOnline);
                    }
                    else
                    {
                        // Delete the current Adpater
                        LeaderBoardListView.Adapter = null;
                        // And store a new one.
                        LeaderBoardListView.Adapter = new LeaderBoardRowAdapter(this);
                    }
                }
                else
                {
                    setBackgroundColorByClick(Color.DarkGray, Color.Gray, isOnline);
                    // Delete the current Adpater
                    LeaderBoardListView.Adapter = null;
                    // And store a new one.
                    LeaderBoardListView.Adapter = new LeaderBoardRowAdapter(this, isOnline);
                }
            }
            catch
            {
                GlobalApp.Alert(this, 0);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Event Handler: Will direct the player to the Main menu.
        public override void OnBackPressed()
        {
            GlobalApp.BeginActivity(this, typeof(GameMenuActivity), GlobalApp.getVariableChoiceName(), gameChoice);        
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Changes the colour of the buttons, when pressed, Dark grey for clicked, light grey for unclicked. 
        // @param clicked, and unclicked has been initialized with a color value.
        // @param isOnline is either true, or false. 
        private void setBackgroundColorByClick(Color clicked, Color unClicked, bool isOnline)
        {
            if (isOnline)
            {
                localBtn.SetBackgroundColor(unClicked);
                onlineBtn.SetBackgroundColor(clicked);
            }
            else
            {
                localBtn.SetBackgroundColor(clicked);
                onlineBtn.SetBackgroundColor(unClicked);
            }
        }
    }
}