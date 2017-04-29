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
/// Date modified: 21-Mar-2017
/// /// Date created: 21-Mar-2017
/// </summary>

namespace SCaR_Arcade
{
    class Game
    {
        public string gTitle { get; set; }
        public int gLogo { get; set; }
        public int gMenuBackground { get; set; }
        public string gLocalFileURL { get; set; }
        public string gOnlineFileURL { get; set; } 
        public Activity gStart { get; set; }
        public int minDifficulty { get; set; }
        public int maxDifficulty { get; set; }
        
        /*
         * Things to add  
         * -------------
         * Game title
         * logo location
         * game location
         * leaderboard location
         * getters
         * -------------
         * 
         * This will help when calling upon info for multiple games through multiple layers of menus.
         */
    }
}