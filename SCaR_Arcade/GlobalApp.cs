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
        private static string playersScore = "playersScore";
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
        public static bool newPlayer()
        {
            return player == null;
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
        // Returns the value of the variables (value represents the name).
        public static string getPlayersScoreVariable()
        {
            return playersScore;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Displays an Alert (most definitely an error that has occured at the application level).
        public static void Alert(Context c, int iApp)
        {
            AlertDialog.Builder adb = new AlertDialog.Builder(c);
            adb.SetMessage(getApplicationErrorMessage(iApp));
            adb.SetTitle("Application failed");
            adb.Show();
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Determines what particular error that has occured.
        private static string getApplicationErrorMessage(int iMsg)
        {
            string message = "";
            switch (iMsg)
            {
                case 0:
                    message = "Oops something went wrong, and have contacted IT support";
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
        // ----------------------------------------------------------------------------------------------------------------
        private static bool addToLeaderBoard(int score)
        {
            bool scoreAdded = false;
            //add score to leaderBoard if(score<lbScore)
            //if added
            scoreAdded = true;
            return scoreAdded;
        }
        // ----------------------------------------------------------------------------------------------------------------
        //
        public static string extractValuesFromString(string character, string content, bool isSecondPart)
        {
            int index = 0;
            string temp = "";
            if (isSecondPart)
            {
                index = content.LastIndexOf(character);
                temp = content.Substring(index + 1, (content.Length) - (index + 1));
            }
            else
            {
                index = content.LastIndexOf(character);
                temp = content.Substring(0, index);
            }
            return temp;
        }
    }
}