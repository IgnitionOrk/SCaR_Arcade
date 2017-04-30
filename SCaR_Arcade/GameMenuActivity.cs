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
    [Activity(Label = "GameMenuActivity", 
        MainLauncher = false,
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait,
        Theme = "@android:style/Theme.NoTitleBar")]
    public class GameMenuActivity : Activity
    {
        private LinearLayout FullScreen;
        private TextView txtGameTitle;
        private TextView txtDifficulty;
        private Button btnStart;
        private Button btnLeaderBoard;
        private Button btnBack;
        private ImageButton imgBtnIn;
        private ImageButton imgBtnDe;
        private int gameChoice;
        private int difficulty;
        private int maxDifficulty;
        private int minDifficulty;
        private Game game;

        // ----------------------------------------------------------------------------------------------------------------
        // Predefined method to the create to build the Activity GameMenu.axml executes. 
        protected override void OnCreate(Bundle savedInstanceState)
        {
          try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.GameMenu);
                FullScreen = FindViewById<LinearLayout>(Resource.Id.FullScreenLinLay);
                txtGameTitle = FindViewById<TextView>(Resource.Id.txtGameTitle);
                txtDifficulty = FindViewById<TextView>(Resource.Id.txtDifficulty);
                btnStart = FindViewById<Button>(Resource.Id.btnStart);
                btnLeaderBoard = FindViewById<Button>(Resource.Id.btnLeaderBoard);
                btnBack = FindViewById<Button>(Resource.Id.btnGameSelect);
                imgBtnIn = FindViewById<ImageButton>(Resource.Id.imgBtnIncrease);
                imgBtnDe = FindViewById<ImageButton>(Resource.Id.imgBtnDecrease);

                // get the index of the item the player has chosen.
                gameChoice = Intent.GetIntExtra(GlobalApp.getVariableChoiceName(), 0);

                // Return the game from the list.
                game = GameInterface.getGameAt(gameChoice);
                difficulty = game.minDifficulty;
                minDifficulty = game.minDifficulty;
                maxDifficulty = game.maxDifficulty;
                txtDifficulty.Text = String.Format("{0}", difficulty);
                txtGameTitle.Text = game.gTitle;
                FullScreen.SetBackgroundResource(game.gMenuBackground);
                
                // Event handlers.
                btnStart.Click += ButtonClickStart;
                btnBack.Click += ButtonClickSelect;
                btnLeaderBoard.Click += ButtonClickLeaderboard;
                imgBtnIn.Click += ImageButtonIncrease;
                imgBtnDe.Click += ImageButtonDecrease;

                // Add the plus and minus pictures to the two image buttons, 
                // that can increase or decrease the difficulty level.
                addPlusAndMinus();
            }
            catch
            {
                GlobalApp.Alert(this, 0);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Plus, and minus bitmap images are added to the image buttons 
        // so players can determine how to increase, or decrease the difficulty. 
        private void addPlusAndMinus()
        {
            Bitmap minus = BitmapFactory.DecodeResource(Resources, Resource.Drawable.minus);
            Bitmap plus = BitmapFactory.DecodeResource(Resources, Resource.Drawable.plus);

            Bitmap minusScaled = Bitmap.CreateScaledBitmap(minus,50, 50, true);
            Bitmap plusScaled = Bitmap.CreateScaledBitmap(plus, 50, 50, true);

            imgBtnDe.SetImageBitmap(minusScaled);
            imgBtnIn.SetImageBitmap(plusScaled);

            imgBtnDe.SetScaleType(ImageView.ScaleType.FitCenter);
            imgBtnIn.SetScaleType(ImageView.ScaleType.FitCenter);

        }
        // ---------------------------------------------------------------------------------------------------------------
        // Begins the game selected from the Main menu.
        protected void ButtonClickStart(Object sender, EventArgs args)
        {
            try
            {
                // Begin the game Activity specifed by type
                BeginActivity(game.gType, GlobalApp.getVariableDifficultyName(), difficulty);
            }
            catch
            {
                GlobalApp.Alert(this, 0);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Returns to the Main Activity of the application.
        protected void ButtonClickSelect(Object sender, EventArgs args)
        {
            try
            {
                // Begin the Main Activity
                BeginActivity(typeof(MainActivity), null, 0);
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
            //TODO: add if statement to back into leaderboardactivity if that was the last place visited, maybe
            try
            {
                // Begin the Main Activity
                BeginActivity(typeof(MainActivity), null, 0);
            }
            catch
            {
                GlobalApp.Alert(this, "Error 101");
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Event Handler: Will direct the user to the Leaderboard. 
        protected void ButtonClickLeaderboard(Object sender, EventArgs ev)
        {
            try
            {
                // Begin the Leaderboard Activity
                BeginActivity(typeof(LeaderBoardActivity), GlobalApp.getVariableChoiceName(), gameChoice);
            }
            catch
            {
                GlobalApp.Alert(this, 0);
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
        // ----------------------------------------------------------------------------------------------------------------
        // Event handler: that increases the difficulty level for the game selected.
        protected void ImageButtonIncrease(Object sender, EventArgs args)
        {
            // the bool parameter determines if the we increase or decrease the difficulty.
            updateDifficulty(true);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Event handler: that decreases the difficulty level for the game selected.
        protected void ImageButtonDecrease(Object sender, EventArgs args)
        {
            // the bool parameter determines if the we increase or decrease the difficulty.
            updateDifficulty(false);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Updates the difficulty level determined by pressing the 'plus' or 'minus' buttons.
        // Will either increases, or decreases the difficulty. 
        private void updateDifficulty(bool isIncrease)
        {
            if (isIncrease)
            {
                if (difficulty < maxDifficulty)
                {
                    difficulty++;
                }
            }
            else
            {
                if (difficulty > minDifficulty)
                {
                    difficulty--;
                }
            }
            txtDifficulty.Text = String.Format("{0}", difficulty);
        }
    }
}