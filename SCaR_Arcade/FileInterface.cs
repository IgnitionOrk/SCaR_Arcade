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
        private static Game game;
        private const int MAXNUMBEROFLINES = 20;
        private static Android.Content.Res.AssetManager assets;
        private const string SCOREFILESPATH = "ScoreFiles/";
        private const string GAMEDESCRIPTIONSPATH = "GameDescriptions/";
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
                if (!File.Exists(game.gLocalFileName))
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
                    using (StreamWriter sw = File.AppendText(game.gLocalFileName))
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
                if (!File.Exists(game.gLocalFileName))
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
                    using (StreamWriter sw = File.AppendText(game.gOnlineFileName))
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
                        string localPath =@"\Local\" + gTitleTrimmed + "Local.txt";
                        File.CreateText(localPath);
                        game.gLocalFileName = localPath;
                    }
                    else
                    { 
                        string onlinePath = @"\Online\" + gTitleTrimmed + "Online.txt";
                        File.CreateText(onlinePath);
                        game.gOnlineFileName = onlinePath;
                    }
                }
            }
            catch
            {

            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        public static string readFromDescription(Android.Content.Res.AssetManager assets)
        {
            try
            {
                initializeAssests(assets);
                string path = GAMEDESCRIPTIONSPATH + game.gDiscription;
                string content = "";
                using (StreamReader sr = new StreamReader(assets.Open(path)))
                {
                    content = sr.ReadToEnd();
                }
                return content;
            }
            catch
            {
                return "";
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        public static List<string> readFromScoreFile(bool isOnline, Android.Content.Res.AssetManager assets)
        {
            try
            {
                initializeAssests(assets);
                List<string> scoreLines = new List<string>();
                string path = "";
                string lineScore = "";
                if (isOnline)
                {
                    path = SCOREFILESPATH +"Online/" + game.gOnlineFileName;
                }
                else
                {
                    path = SCOREFILESPATH + "Local/" + game.gLocalFileName;
                }
                using (StreamReader sr = new StreamReader(assets.Open(path)))
                {
                    while (sr.Peek() > -1)
                    {
                        lineScore = sr.ReadLine();
                        scoreLines.Add(lineScore);
                    }
                }
                return scoreLines;
            }
            catch
            {
                return null;
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        private static void initializeAssests(Android.Content.Res.AssetManager a)
        {
            if(assets == null)
            {
                assets = a;
            }
        }
    }
}