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
/// Creator: Martin O'Connor
/// Student number: 3179234
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
        private Button btnBack;
        private int gameChoice;
        private Game game;
        private ListView LeaderBoardListView;

        // ----------------------------------------------------------------------------------------------------------------
        protected override void OnCreate(Bundle savedInstanceState)
        {
          try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.Leaderboard);             
                FullScreen = FindViewById<LinearLayout>(Resource.Id.FullScreenLinLay);
                LeaderBoard = FindViewById<LinearLayout>(Resource.Id.LeaderBoardLinLay);
                LeaderBoardListView = FindViewById<ListView>(Resource.Id.LeaderBoardListView);
                btnBack = FindViewById<Button>(Resource.Id.btnGameSelect);
                LeaderBoardListView.Adapter = new LeaderBoardRowAdapter(this);

                // get the index of the item the player has chosen.
                gameChoice = Intent.GetIntExtra(GlobalApp.getVariableChoiceName(), 0);

                // Return the game from the list.
                game = GameInterface.getGameAt(gameChoice);
                LeaderBoard.SetBackgroundColor(Color.Gray);
                FullScreen.SetBackgroundResource(game.gMenuBackground);
                
                // Event handlers.
                btnBack.Click += ButtonClickSelect;           
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
            try
            {
                BeginActivity(typeof(GameMenuActivity), GlobalApp.getVariableChoiceName(), gameChoice);
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
            try
            {
                BeginActivity(typeof(GameMenuActivity), GlobalApp.getVariableChoiceName(), gameChoice);        
            }
            catch
            {
                GlobalApp.Alert(this, "Error 101");
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Begins the Activity specified by @param type.
        private void BeginActivity(Type type, string variableName, int value)
        {
            Intent intent = new Intent(this, type);
            if (type != typeof(MainActivity))
            {
                intent.PutExtra(variableName, value);
            }
            StartActivity(intent);
        }
    }
}