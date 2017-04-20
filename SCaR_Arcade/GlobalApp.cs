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
/// Created by: Ryan Cunneen
/// Student number: 3179234
/// Date created: 20-Apr-2017
/// Date modified: 20-Apr-2017
/// </summary>

// This class can be somewhat confusing as to why it has been created.
// The answer is quite simple. In the original protoypes of the application,
// when we called e.g.  Intent.GetIntExtra("gameDifficulty",1) in our games, there was a small chance
// someone may have typed incorrectly the string "gameDifficulty". This could lead to many hours of debugging.
// So we have created a 'global' class with a variable name. So all we would need to do is call the method getVariableName().
// This will reduce complexity, and the application being prone to future errors. 
namespace SCaR_Arcade
{
    static class GlobalApp
    {
        private static Player player;
        private static string gDifficultyName = "gameDifficulty";
        private static string gChoice = "gameChoice";
        // ----------------------------------------------------------------------------------------------------------------
        // Sets the name of the current player;
        public static void setName(string name)
        {
            if (player == null)
            {
                player = new Player();
            }
            player.name = name;
        }

        // ----------------------------------------------------------------------------------------------------------------
        // Returns the value of the variables (value represents the name).
        public static string getVariableDifficultyName()
        {
            return gDifficultyName;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Returns the value of the variables (value represents the name).
        public static string getVariableChoiceName()
        {
            return gChoice;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Issues the appropriate alert is something has gone wrong in the application.
        public static void Alert(Context c, string iMsg)
        {
            // Show an alert.
            AlertDialog.Builder adb = new AlertDialog.Builder(c);
            adb.SetMessage(iMsg);
            adb.SetTitle("Move not allowed");
            adb.Show();
        }
        // ----------------------------------------------------------------------------------------------------------------
        public static void Alert(Context c, int iApp)
        {
            AlertDialog.Builder adb = new AlertDialog.Builder(c);
            adb.SetMessage(getApplicationErrorMessage(iApp));
            adb.SetTitle("Application failed");
            adb.Show();
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Issues an alert when the player has won the game.
        public static void Alert(Context c,int score, string time)
        {
            // Show an alert.
            AlertDialog.Builder adb = new AlertDialog.Builder(c);
            adb.SetTitle("Congratulations You Won");
            adb.SetMessage("Your score " + score + " finished in " + time);
            adb.Show();
        }
        // ----------------------------------------------------------------------------------------------------------------
        private static string getApplicationErrorMessage(int iMsg)
        {
            string message = "";
            switch (iMsg)
            {
                case 0:
                    message = "Oops something went wrong";
                    break;
                default:
                    message = "Unkown Error";
                    break;
            }
            return message;
        }
        // ----------------------------------------------------------------------------------------------------------------
        public static void endScreen(Context c, int iGame, int score, string time)
        {
            // Show an alert.
            AlertDialog.Builder adb = new AlertDialog.Builder(c);
            adb.SetTitle("You Won");

            int isPB = -1;
            if (addToLeaderBoard(score))
            {
                isPB++;
            }

            switch (isPB)
            {
                case 0:
                    adb.SetMessage("Congratulations on a new personal best of " + score);
                    break;
                default:
                    adb.SetMessage("You scored " + score);
                    break;
            }

            adb.Show();
        }
        private static bool addToLeaderBoard(int score)
        {
            bool scoreAdded = false;
            //add score to leaderBoard if(score<lbScore)
            //if added
            scoreAdded = true;
            return scoreAdded;
        }

    }
}