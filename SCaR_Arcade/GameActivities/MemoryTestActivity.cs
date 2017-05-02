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

// Created by: Saxon Colbert-Smith 
// Student Number 3230355
// created: 28-Apr-17
// last modified: 29-Apr-17
//Code adapted from https://developer.xamarin.com/guides/android/user_interface/grid_view/

namespace SCaR_Arcade.GameActivities
{
    [Activity(
              Label = "MemoryTestActivity",
              MainLauncher = false,
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape,
              Theme = "@android:style/Theme.NoTitleBar"
            )
   ]
    public class MemoryTestActivity : Activity
    {

        // firstClicked points to the first button control 
        // that the player clicks, it must be null 
        // if the player hasn't clicked a label yet
        Button firstClicked = null;

        // secondClicked points to the second button control 
        // that the player clicks
        Button secondClicked = null;

        // Use this Random object to choose random icons for the squares
        Random random = new Random();

        // Each of these letters is an interesting icon
        // in the Symbols1 font,
        // and each icon appears twice in this list
        List<string> icons = new List<string>()
        {
            "T", "T", "I", "I", "A", "A", "S", "S",
            "J", "J", "V", "V", "N", "N", "Q", "Q"
        };

        public MemoryTestActivity()
        {
            //InitializeComponent();

            //AssignIconsToSquares();
        }

        protected override void OnCreate(Bundle bundle)
        {
        //    // Timer1 relates to the delay when an incorrect selection is made
        //    // and thereforeis only on after two non-matching 
        //    // icons have been shown to the player, 
        //    // so ignore any clicks if the timer is running
        //    if (timer1.Enabled == true)
        //        return;

        //    Button clickedButton = sender as Button;

        //    if (clickedButton != null)
        //    {

        //        //If the clicked label is black, the player clicked
        //        //an icon that's already been revealed, ignore it
        //        if (clickedButton.ForeColor == Color.Black)
        //            return;

        //        // If firstClicked is null, this is the first icon 
        //        // in the pair that the player clicked,
        //        // so set firstClicked to the label that the player 
        //        // clicked, change its color to black, and return
        //        if (firstClicked == null)
        //        {
        //            firstClicked = clickedButton;
        //            //firstClicked.ForeColor = Color.Black;

        //            return;
        //        }

        //        // If the player gets this far, the timer isn't
        //        // running and firstClicked isn't null,
        //        // so this must be the second icon the player clicked
        //        // Set its colour to match the background colour
        //        //secondClicked = clickedLabel;
        //        //secondClicked.ForeColor = Color.Black;

        //        // Check to see if the player won
        //        CheckForWinner();

        //        // If the player clicked two matching icons, keep them 
        //        // black and reset firstClicked and secondClicked 
        //        // so the player can click another icon
        //        if (firstClicked.Text == secondClicked.Text)
        //        {
        //            firstClicked = null;
        //            secondClicked = null;
        //            return;
        //        }

        //        // If the player gets this far, the player 
        //        // clicked two different icons, so start the 
        //        // timer (which will wait three quarters of 
        //        // a second, and then hide the icons)
        //       // timer1.Start();
        //    }
        //}

        ///// This timer is started when the player clicks 
        ///// two icons that don't match,
        ///// so it counts three quarters of a second 
        ///// and then turns itself off and hides both icons
        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    // Stop the timer
        //   // timer1.Stop();

        //    // Hide both icons
        //    //firstClicked.ForeColor = firstClicked.BackColor;
        //    //secondClicked.ForeColor = secondClicked.BackColor;

        //    // Reset firstClicked and secondClicked 
        //    // so the next time a label is
        //    // clicked, the program knows it's the first click
        //    firstClicked = null;
        //    secondClicked = null;
        //}

        ///// Check every icon to see if it is matched, by 
        ///// comparing its foreground color to its background color. 
        ///// If all of the icons are matched, the player wins
        //private void CheckForWinner()
        //{
        //    // Go through all of the labels in the TableLayoutPanel, 
        //    // checking each one to see if its icon is matched
        //    foreach (Control control in tableLayoutPanel1.Controls)
        //    {
        //        Label iconLabel = control as Label;

        //        if (iconLabel != null)
        //        {
        //            if (iconLabel.ForeColor == iconLabel.BackColor)
        //                return;
        //        }
        //    }

        //    // If the loop didn’t return, it didn't find
        //    // any unmatched icons
        //    // That means the user won. Show a message and close the form
        //    MessageBox.Show("You matched all the icons!", "Congratulations");
        //    Close();

            try
            {
                base.OnCreate(bundle);

                SetContentView(Resource.Layout.MemoryTest);


                var GameBoard = FindViewById<GridView>(Resource.Id.GameBoard);

                GameBoard.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
                {
                    Toast.MakeText(this, args.Position.ToString(), ToastLength.Short).Show();

                };
            }
            catch
            {
                GlobalApp.Alert(this, 0);
            }
        }

    }

}