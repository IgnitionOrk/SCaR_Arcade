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

        public static void removeAt(int index)
        {
            gList.RemoveAt(index);
        }

        public static Game getGameAt(int index)
        {
            return gList.ElementAt(index);
        }

        public static List<Game> getGames()
        {
            populateGameData();
            return gList;
        }

        private static void populateGameData()
        {
            if (gList == null)
            {
                gList = new List<Game>();
                // Here we
                // We can dynamically had the games the user has added.
                // by connecting to the cloud
                // ------------------------
                // but for now well just add these three.
                gList.Add(new Game { gTitle = "Tower of Hanoi", gLogo = Resource.Drawable.game1, minDifficulty = 3, maxDifficulty = 8, activity = new TowersOfHanoiActivity() });
                gList.Add(new Game { gTitle = "Memory test", gLogo = Resource.Drawable.game2 });
                gList.Add(new Game { gTitle = "A Game with a long name", gLogo = Resource.Drawable.game3 });
            }
        }
    }
}