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
/// Date created: 25-Mar-17
/// Date modified: 10-Apr-17
/// </summary>
namespace SCaR_Arcade
{ 
    class Player
    {
        private string name;
        // ----------------------------------------------------------------------------------------------------------------
        // Constructor:
        public Player()
        {
            this.numberOfMoves = 0;
            this.time = "00:00";
        }
    }
}