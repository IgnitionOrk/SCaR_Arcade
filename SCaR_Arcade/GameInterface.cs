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
/// Creator: Martin O'Connor
/// Student number: 3279660
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
        // Returns the current game the user is playing. 
        // Variable game must have been initialized, otherwise will be null;
        public static Game getCurrentGame()
        {
            return game;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Returns the Game in the list @param position
        // @param position will be a number greater or equal to 0.
        public static Game getGameAt(int position)
        {
            game = gList.ElementAt(position);
            return game;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Returns the list of in-built games;
        // gList must have populated with Game data. 
        public static List<Game> getGames()
        {
            return populateGameData();
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Populates the List with in built games. 
        private static List<Game> populateGameData()
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
                    gType = typeof(GameActivities.DiceRollsActivity),
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
            return gList;
        }
    }
}