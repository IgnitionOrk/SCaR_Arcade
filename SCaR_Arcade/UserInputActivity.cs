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
        
            // Event handlers:
            enterNameTxt.Click += EditTextClick;
            menuBtn.Click += MenuButtonClick;

            string content = Intent.GetStringExtra(GlobalApp.getPlayersScoreVariable());

            score = extractValuesFromString(content, false);
            time = extractValuesFromString(content, true);

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
        private string extractValuesFromString(string content, bool isTimeValue)
        {
            // Because the format of the content will be '-Score-Time'; 
            // We need to find Score, and Time that are inbetween the '-';
            int index = 0;
            string temp = "";
            if (isTimeValue)
            {
                index = content.LastIndexOf("-");
                temp = content.Substring(index + 1, (content.Length) - (index + 1));
            }
            else
            {

                index = content.LastIndexOf("-");
                temp = content.Substring(content.IndexOf("-") + 1 , index - 1);
            }
            return temp;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Will determine if the players score, and time can be added to either local, or online. 
        private void checkForNewPositionToLocalAndOnline(string scoreStr, string timeStr)
        {
            int score = Convert.ToInt32(scoreStr);

            if (false)
            {
                enterNameTxt.Enabled = true;
                saveBtn.Enabled = true;
            }
            else
            {
                enterNameTxt.Enabled = false;
                saveBtn.Enabled = false;
            }
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