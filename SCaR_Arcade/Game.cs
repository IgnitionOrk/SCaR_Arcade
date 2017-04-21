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
/// Created by: Martin O'Connell
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
        public string gStartFileURL { get; set; }
        public string gLeaderboardURL { get; set; }
        public int minDifficulty { get; set; }
        public int maxDifficulty { get; set; }
        public Activity activity { get; set; }
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