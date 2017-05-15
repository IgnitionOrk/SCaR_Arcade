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
        public string gLocalFileName { get; set; }
        public string gOnlineFileName { get; set; }
        public string gLocalPath { get; set; }
        public string gOnlinePath { get; set; }
        public string localTestFile { get; set; }
        public string onlineTestFile { get; set; }
        public Type gType { get; set; }
        public int gMinDifficulty { get; set; }
        public int gMaxDifficulty { get; set; }
        public string gDescription { get; set; }
    }
}