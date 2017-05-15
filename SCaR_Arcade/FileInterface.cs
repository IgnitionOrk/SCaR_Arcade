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
/// Creator: Ryan Cunneen
/// Creator: Martin O'Connor
/// Student number: 3179234
/// Student number: 3279660
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

            // We determine if the variables have already been set,
            // If they have, then the files have already been created.
            if (game.gOnlineFileName == null && game.gLocalFileName == null)
            {
                // Create the Local, and Online txt files.
                createFilesForGame();

                // Add the predefined data from the Assets folder.
                // These will similar to the scores that are inbuilt for a game at an actual arcade. 
                addPredefinedData(true);
                addPredefinedData(false);
            }


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
        private static void addPredefinedData(bool isOnline)
        {
            try
            {
                string assetsFile = "";
                string gameFilePath = "";
                List<string> scoreData = new List<string>();

                if (isOnline)
                {
                    assetsFile = game.onlineTestFile;
                    gameFilePath = subFolderOnlinePath + game.gOnlineFileName;
                }
                else
                {
                    assetsFile = game.localTestFile;
                    gameFilePath = subFolderLocalPath + game.gLocalFileName;
                }

                // Open a new connection to the .txt file, so we may extract the data.
                using (StreamReader sr = new StreamReader(assets.Open(assetsFile)))
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
            catch
            {
                System.Diagnostics.Debug.Write("Crash caused in FileInterface.addPredefinedScores().");
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Removes the string (by not adding it to the list) from the .txt file Local or online. 
        // @param position must be determined.
        public static void removeScoreAtPosition(bool isOnline, int position)
        {
            string gameFilePath = "";
            string lineScore = "";
            List<string> scoreData = new List<string>();
            int currentPosition = 0;

            // Determine the game file path, where the .txt file is saved.
            if (isOnline)
            {
                gameFilePath = subFolderOnlinePath + game.gOnlineFileName;
            }
            else
            {
                gameFilePath = subFolderLocalPath + game.gLocalFileName;
            }

            // Open a new connection to the .txt file, so we may extract the data.
            // Essentially we are only adding the scores from the .txt file that do not equal the @param position.
            using (StreamReader sr = new StreamReader(gameFilePath))
            {
                while (sr.Peek() > -1)
                {
                    // Read the next line from the .txt file.
                    lineScore = sr.ReadLine();

                    // Remove any possibility of extra whitespace at the start, and end of the string.
                    lineScore.Trim();

                    // The position of the score is at the start of the string. 
                    currentPosition = Convert.ToInt32(lineScore.Substring(0, lineScore.IndexOf("-")));

                    if (currentPosition != position)
                    {
                        // We are only adding in the scores that do not match the @param position.
                        // Thereby, essentially removing it from the current scores. 
                        scoreData.Add(lineScore);
                    }
                }

                // Close the connection to the file (.txt).
                sr.Close();
            }


            // Open a new connection to the .txt file, so we can insert the new data.
            // Using StreamWrite overrides the current .txt file there, so we are not adding data onto the end of .txt file.
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
        public static void addDataToFile(bool isOnline, string score)
        {
            string gameFilePath = "";

            // Determine the game file path, where the .txt file is saved.
            if (isOnline)
            {
                gameFilePath = subFolderOnlinePath + game.gOnlineFileName;
            }
            else
            {
                gameFilePath = subFolderLocalPath + game.gLocalFileName;
            }
            // Write to the Local file containing scores.
            // We are appending the @param score into the .txt file.
            using (StreamWriter sw = File.AppendText(gameFilePath))
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
        public static void updateData(bool isOnline, int atPosition)
        {
            string path = "";
            string lineScore = "";
            List<string> scoreData = new List<string>();
            int currentPosition = 0;
            int newPosition = 0;
            // Determine the game file path, where the .txt file is saved.
            if (isOnline)
            {
                path = subFolderOnlinePath + game.gOnlineFileName;
            }
            else
            {
                path = subFolderLocalPath + game.gLocalFileName;
            }


            // Open a new connection to the .txt file, so we may extract the data.
            // Essentially we are only adding the scores from the .txt file that do not equal the @param position.
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() > -1)
                {
                    // Read the next line from the .txt file.
                    lineScore = sr.ReadLine();

                    // Remove any possibility of extra whitespace at the start, and end of the string.
                    lineScore.Trim();

                    // The position of the score is at the start of the string. 
                    currentPosition = Convert.ToInt32(lineScore.Substring(0, lineScore.IndexOf("-")));

                    if (currentPosition >= atPosition)
                    {
                        // We are only adding in the scores that do not match the @param position.
                        // Thereby, essentially removing it from the current scores. 
                        // Push the currentPosition by 1, and the atPosition will now come before it.
                        newPosition = currentPosition + 1;
                        lineScore = lineScore.Substring(lineScore.IndexOf("-") + 1, lineScore.Length - lineScore.IndexOf("-") - 1);
                        lineScore = newPosition + "-" + lineScore;
                        scoreData.Add(lineScore);
                    }
                }

                // Close the connection to the file (.txt).
                sr.Close();
            }

            // Open a new connection to the .txt file, so we can insert the new data.
            // Using StreamWrite overrides the current .txt file there, so we are not adding data onto the end of .txt file.
            using (StreamWriter sw = new StreamWriter(path))
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
        public static bool fileReachedLimit(bool isOnline, int limit)
        {
            string path = "";
            int count = 0;
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
                    sr.ReadLine();
                    count++;
                }
                sr.Close();
            }
            return count == limit;
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