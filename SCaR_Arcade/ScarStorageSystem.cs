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
/// Date modified: 15-May-2017
/// /// Date created: 15-May-2017
/// </summary>
namespace SCaR_Arcade
{
    static class ScarStorageSystem
    {
        private static Storage currentStorage;
        // ----------------------------------------------------------------------------------------------------------------
        // determineCurrentStorage, is completely extensible. We have the ability to add any form of 
        // storage system, so long as they implement the Storage interface.
        static public void determineCurrentStorage(int identifier, Android.Content.Res.AssetManager assets)
        {
            switch (identifier)
            {
                case 0:
                    currentStorage = new FileStorage(assets);
                    break;
                default:
                    throw new Exception();
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Assigns the @param g to the currentStage.
        static public void assignGame(Game g)
        {
            currentStorage.assignGame(g);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Adds the @param score to the current storage system.
        static public void addData(bool isOnline, string score)
        {
            currentStorage.addData(isOnline, score);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // removes the data @param position.
        static public void removeData(bool isOnline, int position)
        {
            currentStorage.removeData(isOnline, position);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Returns the description for the game (previously assigned).
        static public string readDescription()
        {
            return currentStorage.readDescription();
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Returns a list of strings read from the current storage.
        static public List<string> readData(bool isOnline)
        {
            return currentStorage.readData(isOnline);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Updates the data @param atPosition
        static public void updateData(bool isOnline, int atPosition)
        {
            currentStorage.updateData(isOnline, atPosition);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Determines if the file has reached its current capacity. 
        static public bool reachedLimit(bool isOnline, int limit)
        {
            return currentStorage.reachedLimit(isOnline, limit);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Determines if the storage has already been created. 
        public static bool hasStorage()
        {
            return currentStorage != null;
        }
    }
}