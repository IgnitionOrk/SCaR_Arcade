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
/// Student number: 3179234
/// Creator: Martin O'Connor
/// Student number: 3279660
/// Date created: 20-Apr-2017
/// Date modified: 20-Apr-2017
/// </summary>
/// 


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
        // Returns the name of the player. 
        public static string getName()
        {
            return playerName;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Determines if the user has entered a name from previously playing a game.
        public static bool isNewPlayer()
        {
            return playerName == null;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Returns the value of the variables (value represents the name).
        // Used to save data into Intents
        public static string getVariableDifficultyName()
        {
            return gDifficultyName;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Returns the value of the variables (value represents the name).
        // Used to save data into Intents
        public static string getVariableChoiceName()
        {
            return gChoice;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Returns the value of the variables (value represents the name).
        // Used to save data into Intents
        public static string getPlayersScoreVariable()
        {
            return playersScore;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Begins the Activity specified by @param type.
        // @param type must be a type exisiting in Scar Arcade application. 
        // @param value must be greater, than or equal to 0. 
        public static void BeginActivity(Context c,Type type, string variableName, int value)
        {
            try
            {
                if (value >= 0)
                {
                    Intent intent = new Intent(c, type);
                    if (type != typeof(MainActivity))
                    {
                        intent.PutExtra(variableName, value);
                    }
                    c.StartActivity(intent);
                }
                else
                {
                    throw new Exception();
                }
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
        // @param type must be a type exisiting in Scar Arcade application. 
        // @param valueTwo must be an integer greater than or equal to 0
        // Overrided method. 
        public static void BeginActivity(Context c, Type type, string variableName, string value, string variableTwoName, int valueTwo)
        {
            try
            {
                if(valueTwo >= 0)
                { 
                    Intent intent = new Intent(c, type);
                    if (type != typeof(MainActivity))
                    {
                        intent.PutExtra(variableTwoName, valueTwo);
                        intent.PutExtra(variableName, value);
                    }
                    c.StartActivity(intent);
                }
                else
                {
                    throw new Exception();
                }
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
        // @param type must be a type exisiting in Scar Arcade application. 
        // @param valueTwo must be greater than or equal to 0.
        // Overrided method. 
        public static void BeginActivity(Context c, Type type, string variableName, int value, string variableTwoName, int valueTwo)
        {
            try
            {
                if (valueTwo >= 0)
                {
                    Intent intent = new Intent(c, type);
                    if (type != typeof(MainActivity))
                    {
                        intent.PutExtra(variableTwoName, valueTwo);
                        intent.PutExtra(variableName, value);
                    }
                    c.StartActivity(intent);
                }
                else
                {
                    throw new Exception();
                }
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
            try
            {
                AlertDialog.Builder adb = new AlertDialog.Builder(c);
                adb.SetMessage(getApplicationErrorMessage(iApp));
                adb.SetTitle("Application failed");
                adb.Show();
            }
            catch
            {
                Alert(c, 1);
            }
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
        // Counts the number of characters in the content string.
        // @param content must contain the @param characters. Otherwise, the 
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
        // ----------------------------------------------------------------------------------------------------------------
        // @param content must be either in the form of 
        //      Name-Score-Time,
        //      HH:MM:SS,
        //      or MM:SS
        // delimiter will be a character ":" or "-", and index should not be less than 0. 
        // Returns a specific index of a string defined by the @param index,
        public static string splitString(string content, int index, Char delimiter)
        {
            String[] subStrings = content.Split(delimiter);
            return subStrings[index];
        }
    }
}