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
/// Student no: 3179234
/// Date modified: 13-Apr-2017
/// /// Date created: 20-Apr-2017
/// </summary>

namespace SCaR_Arcade
{
    static class GameInterface
    {
        private static Game game;
        private static List<Game> gList;
        // ----------------------------------------------------------------------------------------------------------------
        public static Game getCurrentGame()
        {
            return game;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Returns the Game in the list @param position
        public static Game getGameAt(int position)
        {
            game = gList.ElementAt(position);
            return game;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Returns the list of in-built games;
        public static List<Game> getGames()
        {
            populateGameData();
            return gList;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Populates the List with in built games. 
        private static void populateGameData()
        {
            if (gList == null)
            {
                gList = new List<Game>();
                // Here we
                // We can dynamically had the games the user has added.
                // by connecting to the cloud
                // but for now well just add these three.
                gList.Add(new Game
                {
                    gTitle = "Towers of Hanoi",
                    gLogo = Resource.Drawable.TowersLogo,
                    gMenuBackground = Resource.Drawable.TowersLogo_bg,
                    gMinDifficulty = 3,
                    gMaxDifficulty = 8,
                    gType = typeof(GameActivities.TowersOfHanoiActivity),
                    gDescription = "ToH_GameDescription.txt",
                    glocalTestFile = "tohLocalTest.txt",
                    gonlineTestFile = "tohOnlineTest.txt"
                }
                );
                gList.Add(new Game
                {
                    gTitle = "Dice Rolls",
                    gLogo = Resource.Drawable.DiceLogo,
                    gMenuBackground = Resource.Drawable.DiceLogo_bg,
                    gMinDifficulty = 2,
                    gMaxDifficulty = 5,
                    gType = typeof(GameActivities.DiceRolls),
                    gDescription = "DR_GameDescription.txt",
                    glocalTestFile = "drLocalTest.txt",
                    gonlineTestFile = "drOnlineTest.txt",
                    gLeaderBoardCol1 = 2,
                    gLeaderBoardCol1SortBy = 1,
                    gLeaderBoardCol2 = 1,
                    gLeaderBoardCol2SortBy = 2,
                    gLeaderBoardCol3 = 3,
                    gLeaderBoardCol3SortBy = 3
                }
                );
            }
        }
    }
}