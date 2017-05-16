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
/// Creator: Ryan Cunneen
/// Creator: Martin O'Connor
/// Student number: 3179234
/// Student number: 3279660
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
        private static string playerName; 
        private static string gDifficultyName = "gameDifficulty";
        private static string gChoice = "gameChoice";
        private static string playersScore = "playersScore";
        // ----------------------------------------------------------------------------------------------------------------
        // Sets the name of the current player;
        public static void setName(string name)
        {
            playerName = name;
        }
        // ----------------------------------------------------------------------------------------------------------------
        public static string getName()
        {
            return playerName;
        }
        // ----------------------------------------------------------------------------------------------------------------
        public static bool isNewPlayer()
        {
            return playerName == null;
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
        // Begins the Activity specified by @param type.
        public static void BeginActivity(Context c,Type type, string variableName, int value)
        {
            try
            {
                Intent intent = new Intent(c, type);
                if (type != typeof(MainActivity))
                {
                    intent.PutExtra(variableName, value);
                }
                c.StartActivity(intent);
            }
            catch
            {
                // Because an error has happend at the application level
                // We delegate the responsibility to the GlobalApp class.
                Alert(c, 2);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Begins the Activity specified by @param type.
        public static void BeginActivity(Context c, Type type, string variableName, string value, string variableTwoName, int valueTwo)
        {
            try
            {
                Intent intent = new Intent(c, type);
                if (type != typeof(MainActivity))
                {
                    intent.PutExtra(variableTwoName, valueTwo);
                    intent.PutExtra(variableName, value);
                }
                c.StartActivity(intent);
            }
            catch
            {
                // because an error has happend at the Application level
                // We delegate the responsibility to the GlobalApp class.
                GlobalApp.Alert(c, 2);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Begins the Activity specified by @param type.
        public static void BeginActivity(Context c, Type type, string variableName, int value, string variableTwoName, int valueTwo)
        {
            try
            {
                Intent intent = new Intent(c, type);
                if (type != typeof(MainActivity))
                {
                    intent.PutExtra(variableTwoName, valueTwo);
                    intent.PutExtra(variableName, value);
                }
                c.StartActivity(intent);
            }
            catch
            {
                // because an error has happend at the Application level
                // We delegate the responsibility to the GlobalApp class.
                GlobalApp.Alert(c, 2);
            }
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
        //
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
        //
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
            System.Diagnostics.Debug.Write(character);
            System.Diagnostics.Debug.Write(content);
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
        // ----------------------------------------------------------------------------------------------------------------
        // Counts the number of characters in the content string.
        public static int findNumberOfCharacters(string character, string content)
        {
            int count = 0;
            // Remove an possibility of leading, and ending whitespace.
            content.Trim();

            for (int i = 0; i < content.Length; i++)
            {
                if (String.Compare(character, content.Substring(i, 1)) == 0)
                {
                    count++;
                }
            }

            return count;
        }

    }
}