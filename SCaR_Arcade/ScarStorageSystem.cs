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
        // determineCurrentStorage, is very extensible. We have the ability to add any form of 
        // storage system, so long as they implement the Storage interface methods.
        // @param identifier will determine what particular Storage system Scar Arcade will be using. 
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
        static public void assignGameFilePaths(Game g)
        {
            currentStorage.assignGameFilePaths(g);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Adds the @param score to the current storage system.
        static public void addData(string path, string score)
        {
            currentStorage.addData(path, score);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // removes the data @param position.
        // @param position must be either: 
        //      - Greater than or equal to 20.
        //      - Less than or equal to 100.
        static public void removeData(string path, int position)
        {
            currentStorage.removeData(path, position);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Returns the description for the game (previously assigned).
        static public string readDescription(string path)
        {
            return currentStorage.readDescription(path);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Returns a list of strings read from the current storage.
        static public List<string> readData(string path)
        {
            return currentStorage.readData(path);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Updates the data @param atPosition
        // @param @Position must be either: 
        //      - Greater than or equal to 20.
        //      - Less than or equal to 100.
        static public void updateData(string path, int atPosition)
        {
            currentStorage.updateData(path, atPosition);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Determines if the file has reached its current capacity. 
        // @param limit must be:
        //      - Greater than or equal to 20.
        //      - Less than or equal to 100.
        static public bool reachedLimit(string path, int limit)
        {
            return currentStorage.reachedLimit(path, limit);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Determines if the storage has already been created. 
        public static bool hasStorage()
        {
            return currentStorage != null;
        }
    }
}