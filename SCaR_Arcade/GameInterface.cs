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
        private static List<Game> gList;
        // When functionality of adding, and deleting games methods AddGame, and removeAt wil be implemented:
        // ----------------------------------------------------------------------------------------------------------------
        public static void addGame(Game g)
        {
            gList.Add(g);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Removes the Game in the List @param position.
        public static void removeAt(int position)
        {
            gList.RemoveAt(position);
        } 
        // ----------------------------------------------------------------------------------------------------------------
        // Returns the Game in the list @param position
        public static Game getGameAt(int position)
        {
            return gList.ElementAt(position);
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
                    gLogo = Resource.Drawable.game1,
                    gMenuBackground = Resource.Drawable.game1bg,
                    gMinDifficulty = 3,
                    gMaxDifficulty = 8,
                    gType = typeof(GameActivities.TowersOfHanoiActivity),
                    gDescription = "ToH_GameDescription.txt"
                }
                );
                gList.Add(new Game
                {
                    gTitle = "Dice Rolls",
                    gLogo = Resource.Drawable.gameBase,
                    gMinDifficulty = 1,
                    gMaxDifficulty = 5,
                    gType = typeof(GameActivities.DiceRolls),
                    gDescription = "DR_GameDescription.txt"
                }
                );

            }
        }
    }
}