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

namespace SCaR_Arcade
{
    static class GameInterface
    {
        private static List<Game> gList;

        public static void addGame(Game g)
        {
            gList.Add(g);
        }

        public static Game getGameAt(int index)
        {
            return gList.ElementAt(index);
        }
    }
}