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

using System.IO;

namespace SCaR_Arcade
{
    static class FileInterface
    {
        private static string filePath = @"SCaR_Arcade\ScoreFiles";
        private static Game game;
        private const int MAXNUMBEROFLINES = 20;
        /*
         * IMPORTANT NOTE:
         * We need to create a method that determines if the file has the MAXNUMBEROFLINES;
         * We need to order the files scores into ascending order (probably another method to be implemented). 
         */
        // ----------------------------------------------------------------------------------------------------------------
        public static void addCurrentGame(Game g)
        {
            game = g;
        }

        // ----------------------------------------------------------------------------------------------------------------
        // Adds the @param to the Local (.txt) file.
        public static void addScoreToLocal(string score)
        {
            try
            {
                // Determine if there is not a Local (.txt) file.
                if (!File.Exists(game.gLocalFileURL))
                {
                    // Create the Files that will be used to score data on scores from the player.
                    // For this instance we are creating a Local (.txt) file, and not an Online (.txt).
                    createFilesForGame(true);

                    // Retry to add the score into the Files (if needed) because we have created the new .txt files
                    addScoreToLocal(score);
                }
                else
                {
                    // Write to the Local file containing scores.
                    using (StreamWriter sw = File.AppendText(game.gLocalFileURL))
                    {
                        sw.WriteLine(score);
                    }
                }
            }
            catch
            {

            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Adds the @param to the Online (.txt) file.
        public static void addScoreToOnline(string score)
        {
            try
            {
                // Determine if there is not a Local (.txt) file.
                if (!File.Exists(game.gLocalFileURL))
                {
                    // Create the Files that will be used to score data on scores from the player.
                    // For this instance we are creating a Local (.txt) file, and not an Online (.txt).
                    createFilesForGame(false);

                    // Retry to add the score into the Files (if needed) because we have created the new .txt files
                    addScoreToLocal(score);
                }
                else
                {
                    // Write to the Online file containing scores.
                    using (StreamWriter sw = File.AppendText(game.gOnlineFileURL))
                    {
                        sw.WriteLine(score);
                    }
                }
            }
            catch
            {

            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Will create the Local (.txt), and Online(.txt) file for a particular game (instance variable),
        // And saves the file path into the instance variable game.
        private static void createFilesForGame(bool isLocal)
        {                
            try
            {
                if (game != null)
                {
                    // Note when you read this 
                    /*
                     *      IMPORTANT MARTIN, AND SAXON:
                     *      We must trim all the whitespace but .Trim()
                     *      only trims the beginning, and the end of a string. 
                     *      So good idea is to make a method that removes all whitespace in a string.            
                     */

                    // gTitle is the title of the game without spaces
                    // So we can save it as a path for two files.
                    string gTitleTrimmed = game.gTitle.Trim();

                    // Create a Local (.txt) file;
                    if (isLocal)
                    {
                        string localPath = filePath+@"\Local\" + gTitleTrimmed + "Local.txt";
                        File.CreateText(localPath);
                        game.gLocalFileURL = localPath;
                    }
                    else
                    { 
                        string onlinePath = filePath + @"\Online\" + gTitleTrimmed + "Online.txt";
                        File.CreateText(onlinePath);
                        game.gOnlineFileURL = onlinePath;
                    }
                }
            }
            catch
            {

            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Reads a particular line (determine by the @param count), and returns it.
        // Line will come from the Local file of the game.
        public static string readFromLocalFile(int line)
        {
            try {
                
                string fileName = Path.Combine(path, game.gLocalFileURL);

                System.Diagnostics.Debug.WriteLine(fileName);

                string lineScore = "8";
                // Determine if there is not a Local (.txt) file.
                if (fileName != null && File.Exists(fileName))
                {
                    lineScore = "10";
                    using (StreamReader sr = File.OpenText(fileName))
                    {
                        for (int i = 0; i < line && sr.Peek() > -1; i++)
                        {
                            System.Diagnostics.Debug.WriteLine("HERE");
                            lineScore = sr.ReadLine();
                        }
                    }
                }
                return lineScore;
            }
            catch
            {
                return "9";
            }
 }
        // ----------------------------------------------------------------------------------------------------------------
        // Reads a particular line (determine by the @param count), and returns it.
        // Line will come from the Online file of the game.
        public static string readFromOnlineFile(int count)
        {
            string lineScore = "";
            // Determine if there is not a Local (.txt) file.
            if (File.Exists(game.gOnlineFileURL))
            {
                using (StreamReader sr = File.OpenText(game.gOnlineFileURL))
                {
                    for (int i = 0; i < count && sr.Peek() > -1; i++)
                    {
                        lineScore = sr.ReadLine();
                    }
                }
            }
            return lineScore;
        }
    }
}