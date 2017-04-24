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
/// Created by: Ryan Cunneen
/// Student no: 3179234
/// Date modified: 21-Mar-2017
/// /// Date created: 21-Mar-2017
/// </summary>
namespace SCaR_Arcade
{
    [Activity(Label = "LeaderBoardActivity", 
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


        protected override void OnCreate(Bundle savedInstanceState)
        {
          try
            {
                base.OnCreate(savedInstanceState);
                // Create your application here
                SetContentView(Resource.Layout.Leaderboard);
                btnBack = FindViewById<Button>(Resource.Id.btnGameSelect);
                FullScreen = FindViewById<LinearLayout>(Resource.Id.FullScreenLinLay);
                LeaderBoard = FindViewById<LinearLayout>(Resource.Id.LeaderBoardLinLay);

                // get the index of the item the player has chosen.
                gameChoice = Intent.GetIntExtra(GlobalApp.getVariableChoiceName(), 0);

                // Return the game from the list.
                game = GameInterface.getGameAt(gameChoice);
                LeaderBoard.SetBackgroundColor(Color.Gray);
                //FullScreen.SetBackgroundResource(game.gMenuBackground);
                

                //--------------------------------------------------------------------
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
                Intent intent = new Intent(this, typeof(GameMenuActivity));
                StartActivity(intent);
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
                Intent intent = new Intent(this, typeof(GameMenuActivity));
                StartActivity(intent);
            }
            catch
            {
                GlobalApp.Alert(this, "Error 101");
            }
        }
        
    }
}