using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Net;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;

/// <summary>
/// Created by: Ryan Cunneen
/// Student no: 3179234
/// Date modified: 13-Apr-2017
/// /// Date created: 08-Apr-2017
/// </summary>
namespace SCaR_Arcade
{
    static class FileInterface
    {
        private static Game game;
        private static Android.Content.Res.AssetManager assets;
        private const string SCOREFILESPATH = "ScoreFiles/";
        private const string GAMEDESCRIPTIONSPATH = "GameDescriptions/";
        private static string saveFileLocation = Android.App.Application.Context.FilesDir.AbsolutePath;
        private static string subFolderLocalPath = "";
        private static string subFolderOnlinePath = ""; 
        // ----------------------------------------------------------------------------------------------------------------
        // Adds the game the player has clicked on. 
        public static void addCurrentGame(Game g, Android.Content.Res.AssetManager assets)
        {
            game = g;

            // Initialize the Assets folder, so we may extract data from it. 
            initializeAssests(assets);

            // Create the Local, and Online txt files.
            createFilesForGame();

            // Add the predefined data from the Assets folder.
            // These will similar to the scores that are inbuilt for a game at an actual arcade. 
            addPredefinedScores(true);
            addPredefinedScores(false);

        }
        // ----------------------------------------------------------------------------------------------------------------
        // Will create the Local (.txt), and Online(.txt) file for a particular game (instance variable),
        // And saves the file path, and name into the instance variables of game.
        private static void createFilesForGame()
        {
            if (game != null)
            {
                string directory = "";
                // gTitle is the title of the game without spaces
                // So we can save it as a path for two files.
                // We first remove any leading, and end whitespaces using Trim().
                string gTitleTrimmed = game.gTitle.Trim();

                // Then we replace all whitespaces " " in between with empty;
                gTitleTrimmed = gTitleTrimmed.Replace(" ", String.Empty);

                if (!Directory.Exists(subFolderLocalPath))
                {
                    // Save the name so to be used later on with extracting, and inserting new data.
                    game.gLocalFileName = gTitleTrimmed + "Local.txt";
                    directory = SCOREFILESPATH + "Local/";

                    // Combine the two strings together.
                    directory = Path.Combine(saveFileLocation.ToString(), directory);

                    subFolderLocalPath = directory;
                    // Create the directory. This directory will contain the subfolder with all the data
                    // of scores by the player, and players around the world.
                    Directory.CreateDirectory(directory);
                }


                if (!Directory.Exists(subFolderOnlinePath))
                {
                    // Save the name so to be used later on with extracting, and inserting new data.
                    game.gOnlineFileName = gTitleTrimmed + "Online.txt";
                    directory = SCOREFILESPATH + "Online/";

                    // Combine the two strings together.
                    directory = Path.Combine(saveFileLocation.ToString(), directory);

                    subFolderOnlinePath = directory;
                    // Create the directory. This directory will contain the subfolder with all the data
                    // of scores by the player, and players around the world.
                    Directory.CreateDirectory(directory);
                }
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // 
        private static void addPredefinedScores(bool isOnline)
        {
            string assetsPath = "";
            string gameFilePath = "";
            List<string> scoreData = new List<string>();

            if (isOnline)
            {
                assetsPath = SCOREFILESPATH + "Online/onlineTest.txt";
                gameFilePath = subFolderOnlinePath + game.gOnlineFileName;
            }
            else
            {
                assetsPath = SCOREFILESPATH + "Local/localTest.txt";
                gameFilePath = subFolderLocalPath + game.gLocalFileName;
            }

            // Open a new connection to the .txt file, so we may extract the data.
            using (StreamReader sr = new StreamReader(assets.Open(assetsPath)))
            {
                while (sr.Peek() > -1)
                {
                    // Extract all the data from file located in the Assets folder. 
                    scoreData.Add(sr.ReadLine());
                }

                // Close the connection to the file (.txt).
                sr.Close();
            }

            // Open a new connection to the .txt file, so we can insert the new data.
            using (StreamWriter sw = new StreamWriter(gameFilePath))
            {
                for (int i = 0; i < scoreData.Count; i++)
                {
                    sw.WriteLine(scoreData[i]);
                }

                // Close the connection to the file (.txt).
                sw.Close();
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // 
        public static void addScoreToFile(bool isOnline, string score)
        {
            string path = "";
            if (isOnline)
            {
                path = subFolderOnlinePath + game.gOnlineFileName;
                // Determine if there is not a Local (.txt) file.
                if (!File.Exists(path))
                {
                    // Create the Files that will be used to score data on scores from the player.
                    // For this instance we are creating a Online (.txt) file, and not an Local (.txt).

                    //This is determined by the boolean parameter true is for Online, false for Local
                    createFilesForGame();

                    //Now add the predefine scores into the newly created .txt files.
                    addPredefinedScores(true);
                }
            }
            else
            {
                path = subFolderLocalPath + game.gLocalFileName;
                // Determine if there is not a Local (.txt) file.
                if (!File.Exists(path))
                {
                    // Create the Files that will be used to score data on scores from the player.
                    // For this instance we are creating a Local (.txt) file, and not an Online (.txt).

                    //This is determined by the boolean parameter true is for Online, false for Local
                    createFilesForGame();

                    //Now add the predefine scores into the newly created .txt files.
                    addPredefinedScores(false);
                }
            }
            // Write to the Local file containing scores.
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(score);
                sw.Close();
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // 
        public static string readFromDescription()
        {
            if (game.gDescription == null)
            {
                return "";
            }
            else
            {
                string path = GAMEDESCRIPTIONSPATH + game.gDescription;
                string content = "";

                // Open a new connection to the Assets folder, so we may extract the description from the .txt file.
                using (StreamReader sr = new StreamReader(assets.Open(path)))
                {
                    // Read from top to bottom of the file.
                    content = sr.ReadToEnd();
                }

                return content;
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // 
        public static List<string> readFromScoreFile(bool isOnline)
        {
            List<string> scoreLines = new List<string>();
            string path = "";
            string lineScore = "";
            if (isOnline)
            {
                path = subFolderOnlinePath + game.gOnlineFileName;
            }
            else
            {
                path = subFolderLocalPath + game.gLocalFileName;
            }

                
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() > -1)
                {
                    lineScore = sr.ReadLine();
                    scoreLines.Add(lineScore);
                }

                sr.Close();
            }
            return scoreLines;
        }
        // ----------------------------------------------------------------------------------------------------------------
        //
        private static void initializeAssests(Android.Content.Res.AssetManager a)
        {
            if (assets == null)
            {
                assets = a;
            }
        }
    }
}