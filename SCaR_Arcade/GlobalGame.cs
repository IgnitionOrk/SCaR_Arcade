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
    static class GlobalGame
    {
        private static string gDifficultyName = "gameDifficulty";
        // ----------------------------------------------------------------------------------------------------------------
        // Returns the name of the variable.
        public static string getVariableName()
        {
            return gDifficultyName;
        }
    }
}