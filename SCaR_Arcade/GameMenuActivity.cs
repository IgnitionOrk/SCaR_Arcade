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
/// Date created: 21-Mar-2017
/// Created by: Ryan Cunneen
/// Student no: 3179234
/// Date modified: 21-Mar-2017
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
        private ImageButton imgBtnIncrease;
        private ImageButton imgBtnDecrease;
        private int gameChoice;
        private int difficulty;
        private int maxDifficulty = 5;
        private int minDifficulty = 1;
        private Game game;


        protected override void OnCreate(Bundle savedInstanceState)
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
            imgBtnIncrease = FindViewById<ImageButton>(Resource.Id.imgBtnIncrease);
            imgBtnDecrease = FindViewById<ImageButton>(Resource.Id.imgBtnDecrease);

            lL1.SetBackgroundColor(Color.Red);

            gameChoice = Intent.GetIntExtra("gameChoice",0);

            if (game == null)
            {
                game = GameInterface.getGameAt(gameChoice);
            }


            difficulty = game.minDifficulty;
            minDifficulty = game.minDifficulty;
            maxDifficulty = game.maxDifficulty;
            txtDifficulty.Text = String.Format("{0}", difficulty);
            txtGameTitle.Text = GetGameTitle();
            

            //--------------------------------------------------------------------
            // Event handlers.
            btnStart.Click += ButtonClickStart;
            btnGameSelect.Click += ButtonClickSelect;
            btnLeaderBoard.Click += ButtonClickLeaderboard;
            imgBtnIncrease.Click += ImageButtonIncrease;
            imgBtnDecrease.Click += ImageButtonDecrease;
        }

        //--------------------------------------------------------------------
        //Start button
        protected void ButtonClickStart(Object sender, EventArgs args)
        {
            try
            {
                Type type = game.activity.GetType();
                Intent intent = new Intent(this, type);
                intent.PutExtra(GlobalGame.getVariableName(), difficulty);
                StartActivity(intent);
            }
            catch
            {
                txtErrorMessage.Text = "Oops game wouldn't start. Try again later.";
            }
        }

        //--------------------------------------------------------------------
        //Back button
        public override void OnBackPressed()
        {
            try
            {
                returnToMainActivity();
            }
            catch
            {
                txtErrorMessage.Text = "Oops something went wrong with trying to go back.";

            }
        }

        //--------------------------------------------------------------------
        //Back button
        protected void ButtonClickSelect(Object sender, EventArgs args)
        {
            try
            {
                returnToMainActivity();
            }
            catch
            {
                txtErrorMessage.Text = "Oops something went wrong with trying to go back.";

            }
        }
        private void returnToMainActivity()
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }
        //--------------------------------------------------------------------
        protected string GetGameTitle()
        {
            return game.gTitle;
        }

        //--------------------------------------------------------------------
        protected void ImageButtonIncrease(Object sender, EventArgs args)
        {
            updateDifficulty(true);
        }

        //--------------------------------------------------------------------
        protected void ImageButtonDecrease(Object sender, EventArgs args)
        {
            updateDifficulty(false);
        }
        //--------------------------------------------------------------------
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

        //--------------------------------------------------------------------
        protected void ButtonClickLeaderboard(Object sender, EventArgs ev)
        {

        }
    }
}