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
    [Activity(Label = "GameMenuActivity", 
        MainLauncher = false,
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait,
        Theme = "@android:style/Theme.NoTitleBar")]
    public class GameMenuActivity : Activity
    {
        private LinearLayout lL1;
        private TextView txtGameTitle;
        private TextView txtDifficulty;
        private TextView txtErrorMessage;
        private Button btnStart;
        private Button btnLeaderBoard;
        private Button btnGameSelect;
        private ImageButton imgBtnIn;
        private ImageButton imgBtnDe;
        private int gameChoice;
        private int difficulty;
        private int maxDifficulty;
        private int minDifficulty;
        private Game game;


        protected override void OnCreate(Bundle savedInstanceState)
        {
          try
            {
                base.OnCreate(savedInstanceState);
                // Create your application here
                SetContentView(Resource.Layout.GameMenu);
                lL1 = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
                txtGameTitle = FindViewById<TextView>(Resource.Id.txtGameTitle);
                txtDifficulty = FindViewById<TextView>(Resource.Id.txtDifficulty);
                txtErrorMessage = FindViewById<TextView>(Resource.Id.txtErrorMessage);
                btnStart = FindViewById<Button>(Resource.Id.btnStart);
                btnLeaderBoard = FindViewById<Button>(Resource.Id.btnLeaderBoard);
                btnGameSelect = FindViewById<Button>(Resource.Id.btnGameSelect);
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
                lL1.SetBackgroundResource(game.gMenuBackground);
                //--------------------------------------------------------------------
                // Event handlers.
                btnStart.Click += ButtonClickStart;
                btnGameSelect.Click += ButtonClickSelect;
                btnLeaderBoard.Click += ButtonClickLeaderboard;
                imgBtnIn.Click += ImageButtonIncrease;
                imgBtnDe.Click += ImageButtonDecrease;


                addPlusAndMinus();
            }
            catch
            {
                GlobalApp.Alert(this, 0);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Add plus, and minus bitmap images to image buttons 
        // so players can increase, or decrease the difficulty for the selected game.
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
        // Begins the game selected.
        protected void ButtonClickStart(Object sender, EventArgs args)
        {
            try
            {
                Type type = game.activity.GetType();
                Intent intent = new Intent(this, type);
                intent.PutExtra(GlobalApp.getVariableDifficultyName(), difficulty);
                StartActivity(intent);
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
                Intent intent = new Intent(this, typeof(MainActivity));
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
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            }
            catch
            {
                GlobalApp.Alert(this, "Error 101");
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Event handler: that increases the difficulty level for the game selected.
        protected void ImageButtonIncrease(Object sender, EventArgs args)
        {
            updateDifficulty(true);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Event handler: that decreases the difficulty level for the game selected.
        protected void ImageButtonDecrease(Object sender, EventArgs args)
        {
            updateDifficulty(false);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Updates the difficulty level determined by pressing the 'plus' or 'minus' buttons.
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

        // ----------------------------------------------------------------------------------------------------------------
        // Event Handler: Will direct the user to the Leaderboard. 
        protected void ButtonClickLeaderboard(Object sender, EventArgs ev)
        {

        }
    }
}