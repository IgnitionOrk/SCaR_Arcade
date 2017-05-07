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
        public static void addCurrentGame(Game g, Android.Content.Res.AssetManager assets)
        {
            game = g;

            // Create the Local, and Online txt files.
            createFilesForGame(true);
            createFilesForGame(false);

            // Add the predefined data from the Assets folder.
            addPredefinedScores(true, game.gOnlineDirectory + game.gOnlineFileName, assets );
            addPredefinedScores(false, game.gLocalDirectory + game.gLocalFileName, assets);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Will create the Local (.txt), and Online(.txt) file for a particular game (instance variable),
        // And saves the file path, and name into the instance variables of game.
        private static void createFilesForGame(bool isOnline)
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
                    }
                    else
                    {
                        game.gLocalFileName = gTitleTrimmed + "Local.txt";

                        // Create a path that contains the (.txt) file.
                        directory = SCOREFILESPATH + "Local/";
                        directory = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), directory);
                    }

                    game.gOnlineDirectory = directory;

                    // Create the directory 
                    Directory.CreateDirectory(directory);

                    //Used to create the path in which the .txt file will be located.
                    path = directory + game.gOnlineFileName;

                    // Create the .txt file at the specified location (directory).
                    File.Create(path);
                }
                System.Diagnostics.Debug.WriteLine("HEREREREEREREREREREERER:  " + Directory.Exists(directory));
                System.Diagnostics.Debug.WriteLine("HEREREREEREREREREREERER:  " + path);
                System.Diagnostics.Debug.WriteLine("HEREREREEREREREREREERER:  " + File.Exists(path));
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        private static void addPredefinedScores(bool isOnline, string pathToFile, Android.Content.Res.AssetManager assets)
        {
            try
            {
                string path = "";
                if (isOnline)
                {
                    path = SCOREFILESPATH + "Online/onlineTest.txt";
                }
                else
                {
                    path = SCOREFILESPATH + "Local/localTest.txt";
                }
                if (File.Exists(pathToFile))
                {
                    System.Diagnostics.Debug.WriteLine(pathToFile);
                    using (StreamReader sr = new StreamReader(assets.Open(path)))
                    {
                        // StreamWriter is the problem and I don't know why!!!

                        // The pathToFile is correct because I've checked if the file exists, and it returns true. 
                        using (StreamWriter sw = new StreamWriter(pathToFile))
                        {
                            try
                            {
                                while (sr.Peek() > -1)
                                {


                                    System.Diagnostics.Debug.WriteLine(sr.ReadLine());


                                    // For some reason it bugs out here and I can't get it to work.

                                    //sw.WriteLine(sr.ReadLine());
                                }
                            }
                            catch
                            {
                                System.Diagnostics.Debug.WriteLine("HERE");

                            }
                            finally
                            {
                                sr.Close();
                                sw.Close();
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // 
        public static void addScoreToFile(bool isOnline, string score, Android.Content.Res.AssetManager assets)
        {
            try
            {
                initializeAssests(assets);
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

                        //Now add the predefine scores into the newly created .txt files.
                        addPredefinedScores(true, path, assets);
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

                        //Now add the predefine scores into the newly created .txt files.
                        addPredefinedScores(false, path, assets);
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
                    try
                    {
                        content = sr.ReadToEnd();
                    }
                    catch
                    {

                    }
                    finally
                    {
                        sr.Close();
                    }
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
                    path = game.gOnlineDirectory + game.gOnlineFileName;
                }
                else
                {
                    path = game.gLocalDirectory + game.gLocalFileName;
                }
                using (StreamReader sr = new StreamReader(path))
                {
                    try
                    {
                        while (sr.Peek() > -1)
                        {
                            lineScore = sr.ReadLine();
                            scoreLines.Add(lineScore);
                        }
                    }
                    catch
                    {

                    }
                    finally
                    {
                        sr.Close();
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