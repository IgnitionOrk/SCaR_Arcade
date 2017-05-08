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
/// Creator: Martin O'Connor
/// Student number: 3279660
/// Date modified: 21-Mar-2017
/// /// Date created: 21-Mar-2017
/// </summary>
namespace SCaR_Arcade
{
    class LeaderBoard
    {
        public int lbPosition { get; set; }
        public string lbName { get; set; }
        public int lbScore { get; set; }
        public double lbTime { get; set; }
    }
}