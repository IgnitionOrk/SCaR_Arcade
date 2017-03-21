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
/// Date created: 21-Mar-2017
/// Created by: Ryan Cunneen
/// Student no: 3179234
/// Date modified: 21-Mar-2017
/// </summary>
namespace SCaR_Arcade
{
    [Activity(Label = "GameMenuActivity")]
    public class GameMenuActivity : Activity
    {
        private TextView txtGameTitle;
        private TextView txtDifficulty;
        private Button btnStart;
        private Button btnLeaderBoard;
        private Button btnGameSelect;
        private ImageButton imgBtnIncrease;
        private ImageButton imgBtnDecrease;
        private int gameChoice;
        private int difficulty;
        private const int MAXDIFFICULTY = 5;
        private const int MINDIFFICULTY = 1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.GameMenu);
            txtGameTitle = FindViewById<TextView>(Resource.Id.txtGameTitle);
            txtDifficulty = FindViewById<TextView>(Resource.Id.txtDifficulty);
            btnStart = FindViewById<Button>(Resource.Id.btnStart);
            btnLeaderBoard = FindViewById<Button>(Resource.Id.btnLeaderBoard);
            btnGameSelect = FindViewById<Button>(Resource.Id.btnGameSelect);
            imgBtnIncrease = FindViewById<ImageButton>(Resource.Id.imgBtnIncrease);
            imgBtnDecrease = FindViewById<ImageButton>(Resource.Id.imgBtnDecrease);
            gameChoice = Convert.ToInt32(Intent.GetStringExtra("gameChoice"));
            difficulty = 1;
            txtDifficulty.Text = String.Format("{0}", difficulty);
            txtGameTitle.Text = GetGameTitle();
            //--------------------------------------------------------------------
            btnStart.Click += ButtonClickStart;
            btnGameSelect.Click += ButtonClickSelect;
            imgBtnIncrease.Click += ImageButtonIncrease;
            imgBtnDecrease.Click += ImageButtonDecrease;
        }
        //--------------------------------------------------------------------
        protected void ButtonClickStart(Object sender, EventArgs args)
        {
            try
            {
                Intent intent = null;
                switch (gameChoice)
                {
                    case 0:
                        intent = new Intent(this, typeof(TowerOfHanoiActivity));
                        StartActivity(intent);
                        break;
                    case 1:
                        //implement the memory card game;
                        break;
                }
            }
            catch
            {

            }
        }
        //--------------------------------------------------------------------
        protected void ButtonClickSelect(Object sender, EventArgs args)
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
        //--------------------------------------------------------------------
        protected string GetGameTitle()
        {
            string title = "";
            switch (gameChoice)
            {
                case 0:
                    title = "Tower of Hanoi";
                    break;
            }
            return title;
        }
        //--------------------------------------------------------------------
        protected void ImageButtonIncrease(Object sender, EventArgs args)
        {
            if (difficulty < MAXDIFFICULTY) {
                difficulty++;
                txtDifficulty.Text = String.Format("{0}", difficulty);
            }
            else
            {
                txtDifficulty.Text = String.Format("{0}", difficulty);
            }
        }
        //--------------------------------------------------------------------
        protected void ImageButtonDecrease(Object sender, EventArgs args)
        {

            if (difficulty > MINDIFFICULTY)
            {
                difficulty--;
                txtDifficulty.Text = String.Format("{0}", difficulty);
            }
            else
            {
                txtDifficulty.Text = String.Format("{0}", difficulty);
            }
        }
    }
}