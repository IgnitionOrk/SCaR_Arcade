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
        public static void setName(string name)
        {
            if (player == null)
            {
                player = new Player();
            }
            player.setName(name);
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
        public static void Alert(Context c, bool isGameError, int iMsg)
        {
            // Show an alert.
            AlertDialog.Builder adb = new AlertDialog.Builder(c);
            if (isGameError)
            {
                adb.SetMessage(getGameMessage(iMsg));
                adb.SetTitle("Move not allowed");
            }
            else
            {
                adb.SetMessage(getApplicationMessage(iMsg));
                adb.SetTitle("Application failed");
            }
            adb.Show();
        }
        // ----------------------------------------------------------------------------------------------------------------
        private static string getGameMessage(int iMsg)
        {
            string message = "";
            switch (iMsg)
            {
                case 0:
                    message = "You cannot place larger disks on top of smaller disks";
                    break;
                case 1:
                    message = "You have dropped the disk outside of the game screen.";
                    break;
                default:
                    message = "Unkown Error";
                    break;
            }
            return message;
        }
        // ----------------------------------------------------------------------------------------------------------------
        private static string getApplicationMessage(int iMsg)
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
    }
}