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
        public string glocalTestFile { get; set; }
        public string gonlineTestFile { get; set; }
        public Type gType { get; set; }
        public int gMinDifficulty { get; set; }
        public int gMaxDifficulty { get; set; }
        public string gDescription { get; set; }
        //sort by what first, dif = 1, score = 2, time = 3
        public int gLeaderBoardCol1 { get; set; }
        public int gLeaderBoardCol2 { get; set; }
        public int gLeaderBoardCol3 { get; set; }
        // sort by 1 = ascending, 2 = decending, 3 = doesn't mater
        public int gLeaderBoardCol1SortBy { get; set; }
        public int gLeaderBoardCol2SortBy { get; set; }
        public int gLeaderBoardCol3SortBy { get; set; }
    }
}