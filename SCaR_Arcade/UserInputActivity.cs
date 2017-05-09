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
using Android.Views.InputMethods;
using static Android.Views.View;

namespace SCaR_Arcade
{
    [Activity(
        Label = "",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class UserInputActivity : Activity
    {
        private Button saveBtn;
        private Button menuBtn;
        private EditText enterNameTxt;
        private TextView congratTxtView;
        private TextView scoreTxtView;
        private TextView timeTxtView;
        private CheckBox chkBoxName;
        private string score;
        private string time;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.UserInput);

            saveBtn = FindViewById<Button>(Resource.Id.saveBtn);
            menuBtn = FindViewById<Button>(Resource.Id.menuBtn);
            enterNameTxt = FindViewById<EditText>(Resource.Id.enterNameETxt);
            scoreTxtView = FindViewById<TextView>(Resource.Id.scoreTxtView);
            timeTxtView = FindViewById<TextView>(Resource.Id.timeTxtView);
            congratTxtView = FindViewById<TextView>(Resource.Id.congratulationsTxtView);
            chkBoxName = FindViewById<CheckBox>(Resource.Id.chkBoxPreviousName);
            // Event handlers:
            enterNameTxt.Click += EditTextClick;
            menuBtn.Click += MenuButtonClick;

            string content = Intent.GetStringExtra(GlobalApp.getPlayersScoreVariable());

            // As content is in the format of -Score-Time we need to remove the first '-'
            content = content.Substring(content.IndexOf("-") + 1, content.Length - 1);

            score = GlobalApp.extractValuesFromString("-",content, false);
            time = GlobalApp.extractValuesFromString("-",content, true);

            scoreTxtView.Text += " "+score;
            timeTxtView.Text += " "+time;

            checkForNewPositionToLocalAndOnline(score, time);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Overwritten method to close the soft keyboard on EditText (enterNameTxt), when the user has clicked outside of the EditText view.
        // Resource: http://stackoverflow.com/questions/39636698/how-to-hide-keyboard-in-xamarin-android-after-clicking-outside-edittext
        public override bool OnTouchEvent(MotionEvent e)
        {
            InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(enterNameTxt.WindowToken, HideSoftInputFlags.None);
            enterNameTxt.Text = "Enter name here.";
            return base.OnTouchEvent(e);
        }

        // ----------------------------------------------------------------------------------------------------------------
        protected void EditTextClick(Object sender, EventArgs args)
        {
            enterNameTxt.Text = "";
        }


        // ----------------------------------------------------------------------------------------------------------------
        protected void MenuButtonClick(Object sender, EventArgs args)
        {
            try
            {
                BeginActivity(typeof(GameMenuActivity), "", 0);
            }
            catch
            {
                GlobalApp.Alert(this, 0);
            }

        }
        // ----------------------------------------------------------------------------------------------------------------
        // Will determine if the players score, and time can be added to either local, or online. 
        private void checkForNewPositionToLocalAndOnline(string scoreStr, string timeStr)
        {
            int score = Convert.ToInt32(scoreStr);
            int hours = 0;
            int minutes = 0;
            int seconds = 0;

            // Counts the number of ":" in timeStr,
            // There will be two if timeStr is in the format of HH:MM:SS
            // Otherwise there will be only one MM:SS
            int count = findNumberOfCharacters(":", timeStr);
            if (count < 2)
            {
                // First part of the string
                minutes = Convert.ToInt32(GlobalApp.extractValuesFromString(":", timeStr, false));

                // Second part of the string
                seconds = Convert.ToInt32(GlobalApp.extractValuesFromString(":", timeStr, true));
            }
            else
            {
                // First part of the string
                hours = Convert.ToInt32(GlobalApp.extractValuesFromString(":", timeStr, false));

                // Second part of the string
                minutes = Convert.ToInt32(GlobalApp.extractValuesFromString(":", timeStr, true));

                // Third part of the string
                seconds = Convert.ToInt32(timeStr.Substring(timeStr.LastIndexOf(":"), 2));
            }

            bool ifNewHighScore = LeaderBoardInterface.checkForNewLocalHighScore(score, hours, minutes, seconds);
            saveBtn.Enabled = ifNewHighScore;
            enterNameTxt.Enabled = ifNewHighScore;

            // Now we don't need to check if it can be uploaded to the Online .txt file.
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Counts the number of characters in the content string.
        private int findNumberOfCharacters(string character, string content)
        {
            int count = 0;
            // Remove an possibility of leading, and ending whitespace.
            content.Trim();

            for(int i = 0; i < content.Length; i++)
            {
                if (String.Compare(character, content.Substring(i, 1)) == 0)
                {
                    count++;
                }
            }

            return count;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Begins the Activity specified by @param type.
        private void BeginActivity(Type type, string variableName, int value)
        {
            try
            {
                Intent intent = new Intent(this, type);
                if (type != typeof(MainActivity))
                {
                    intent.PutExtra(variableName, value);
                }
                StartActivity(intent);
            }
            catch
            {
                // because an error has happend at the Application level
                // We delegate the responsibility to the GlobalApp class.
                GlobalApp.Alert(this, 2);
            }
        }
    }
}