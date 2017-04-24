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
        public static void addScoreToLocal(string score)
        {
            try
            {
                // Determine if there is not a Local (.txt) file.
                if (!File.Exists(game.gLocalFileURL))
                {
                    // Create the Files that will be used to score data on scores from the player.
                    // For this instance we are creating a Local (.txt) file, and not an Online (.txt).
                    createFilesForGame(true, false);

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
        public static void addScoreToOnline(string score)
        {
            try
            {
                // Determine if there is not a Local (.txt) file.
                if (!File.Exists(game.gLocalFileURL))
                {
                    // Create the Files that will be used to score data on scores from the player.
                    // For this instance we are creating a Local (.txt) file, and not an Online (.txt).
                    createFilesForGame(false, true);

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
        private static void createFilesForGame(bool isLocal, bool isOnline)
        {                
            try
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
                    string localPath = @"\Local\" + gTitleTrimmed + "Local.txt";
                    File.CreateText(localPath);
                    game.gLocalFileURL = localPath;
                }

                // Create an Online (.txt) file.
                if (isOnline)
                {
                    string onlinePath = @"\Online\" + gTitleTrimmed + "Online.txt";
                    File.CreateText(onlinePath);
                    game.gOnlineFileURL = onlinePath;
                }
            }
            catch
            {

            }
        }
    }
}