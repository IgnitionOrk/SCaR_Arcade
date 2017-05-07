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
        // Will create the Local (.txt), and Online(.txt) file for a particular game (instance variable),
        // And saves the file path, and name into the instance variables of game.
        private static void createFilesForGame(bool isOnline)
        {                
            try
            {
                if (game != null)
                {
                    string directory = "";
                    string path = "";
                    // gTitle is the title of the game without spaces
                    // So we can save it as a path for two files.
                    // We first remove any leading, and end whitespaces using Trim().
                    string gTitleTrimmed = game.gTitle.Trim();

                    // Then we replace all whitespaces in between with empty;
                    gTitleTrimmed = gTitleTrimmed.Replace(" ", String.Empty);
                    if (!Directory.Exists(game.gOnlineDirectory) || !Directory.Exists(game.gLocalDirectory))
                    {
                        if (isOnline && !Directory.Exists(game.gOnlineDirectory))
                        {
                            game.gOnlineFileName = gTitleTrimmed + "Online.txt";

                            // Create a path that contains the (.txt) file.
                            directory = SCOREFILESPATH + "Online/";
                            directory = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), directory);
                            game.gOnlineDirectory = directory;

                            // Create the directory 
                            Directory.CreateDirectory(directory);

                            //Used to create the path in which the .txt file will be located.
                            path = directory + game.gOnlineFileName;

                            // Create the .txt file at the specified location (directory).
                            File.Create(path);
                        }
                        else
                        {
                            game.gLocalFileName = gTitleTrimmed + "Local.txt";

                            // Create a path that contains the (.txt) file.
                            directory = SCOREFILESPATH + "Local/";
                            directory = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), directory);
                            game.gLocalDirectory = directory;

                            // Create the directory 
                            Directory.CreateDirectory(directory);

                            //Used to create the path in which the .txt file will be located.
                            path = directory + game.gLocalFileName;

                            // Create the .txt file at the specified location (directory).
                            File.Create(path);
                        }
                        
                    }            
                    System.Diagnostics.Debug.WriteLine("HEREREREEREREREREREERER:  "+ Directory.Exists(directory));
                    System.Diagnostics.Debug.WriteLine("HEREREREEREREREREREERER:  "+ path);
                    System.Diagnostics.Debug.WriteLine("HEREREREEREREREREREERER:  "+ File.Exists(path));
                }
            }
            catch
            {
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // 
        public static void addScoreToFile(bool isOnline, string score)
        {
            try
            {
                string path = "";
                if (isOnline)
                {
                    path = game.gOnlineDirectory + game.gOnlineFileName;
                    // Determine if there is not a Local (.txt) file.
                    if (!File.Exists(path))
                    {
                        // Create the Files that will be used to score data on scores from the player.
                        // For this instance we are creating a Online (.txt) file, and not an Local (.txt).

                        //This is determined by the boolean parameter true is for Online, false for Local
                        createFilesForGame(true);
                    }
                    // Write to the Local file containing scores.
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(score);
                    }
            
                }
                else
                {
                    path = game.gLocalDirectory + game.gLocalFileName;
                    // Determine if there is not a Local (.txt) file.
                    if (!File.Exists(path))
                    {
                        // Create the Files that will be used to score data on scores from the player.
                        // For this instance we are creating a Local (.txt) file, and not an Online (.txt).

                        //This is determined by the boolean parameter true is for Online, false for Local
                        createFilesForGame(false);
                    }
                    // Write to the Local file containing scores.
                    using (StreamWriter sw = File.AppendText(path))
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
        // 
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
        // 
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
        //
        private static void initializeAssests(Android.Content.Res.AssetManager a)
        {
            if(assets == null)
            {
                assets = a;
            }
        }
    }
}