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
using Android.Content.Res;
using System.IO;

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
    [Activity(Label = "GameMenuActivity", 
        MainLauncher = false,
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait,
        Theme = "@android:style/Theme.NoTitleBar")]
    public class GameMenuActivity : Activity
    {
        private LinearLayout descriptionBackground;
        private LinearLayout FullScreen;
        private TextView txtGameTitle;
        private TextView txtDifficulty;
        private TextView descriptionTitle;
        private TextView gameDescription;
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
                gameDescription = FindViewById<TextView>(Resource.Id.description);
                descriptionTitle = FindViewById<TextView>(Resource.Id.desTextView);
                descriptionBackground = FindViewById<LinearLayout>(Resource.Id.descriptionLinLay);

                // get the index of the item the player has chosen.
                gameChoice = Intent.GetIntExtra(GlobalApp.getVariableChoiceName(), 0);

                game = GameInterface.getGameAt(gameChoice);

                if (game == null)
                {
                    throw new Exception();
                }

                difficulty = game.gMinDifficulty;
                minDifficulty = game.gMinDifficulty;
                maxDifficulty = game.gMaxDifficulty;
                txtDifficulty.Text = String.Format("{0}", difficulty);
                txtGameTitle.Text = game.gTitle;
                FullScreen.SetBackgroundResource(game.gMenuBackground);
                descriptionBackground.SetBackgroundColor(Color.Gray);

                // Event handlers.
                btnStart.Click += ButtonClickStart;
                btnBack.Click += ButtonClickSelect;
                btnLeaderBoard.Click += ButtonClickLeaderboard;
                imgBtnIn.Click += ImageButtonIncrease;
                imgBtnDe.Click += ImageButtonDecrease;

                // Add the plus and minus pictures to the two image buttons, 
                // that can increase or decrease the difficulty level.
                addPlusAndMinus();

                initializeKeyComponents();           
            }
            catch
            {
                GlobalApp.Alert(this, 0);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Initializes the key components Game Menu will use. 
        // Variable game must be initialized, from the GameInterface class.
        private void initializeKeyComponents()
        {
            if (!ScarStorageSystem.hasStorage())
            {
                // Determine the storage type.
                ScarStorageSystem.determineCurrentStorage(0, Assets);
            }

            if (game.gOnlinePath == null && game.gLocalPath == null)
            {
                // Assign the current game the player is playing.
                ScarStorageSystem.assignGameFilePaths(game);
            }

            // Add the description of the game.
            gameDescription.Text = ScarStorageSystem.readDescription(game.gDescription);
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
            // Begin the game Activity specifed by type
            GlobalApp.BeginActivity(this, game.gType, GlobalApp.getVariableDifficultyName(), difficulty, GlobalApp.getVariableChoiceName(), Intent.GetIntExtra(GlobalApp.getVariableChoiceName(), 0));
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Returns to the Main Activity of the application.
        protected void ButtonClickSelect(Object sender, EventArgs args)
        {
            // Begin the Main Activity
            GlobalApp.BeginActivity(this, typeof(MainActivity), "", 0);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Event Handler: Will direct the player to the Main menu.
        public override void OnBackPressed()
        {
            // Begin the Main Activity
            GlobalApp.BeginActivity(this, typeof(MainActivity), "", 0);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Event Handler: Will direct the user to the Leaderboard. 
        protected void ButtonClickLeaderboard(Object sender, EventArgs ev)
        {
            GlobalApp.BeginActivity(this ,typeof(LeaderBoardActivity), GlobalApp.getVariableChoiceName(), gameChoice);
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
        // @param isIncrease will be either true or false. 
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