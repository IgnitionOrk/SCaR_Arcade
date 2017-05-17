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
/// Student number: 3179234
/// Date modified: 13-Apr-2017
/// /// Date created: 08-Apr-2017
/// </summary>
namespace SCaR_Arcade
{
    class FileStorage : Storage
    {
        private Android.Content.Res.AssetManager assets;
        private const string SCOREFILESPATH = @"ScoreFiles/";
        private const string GAMEDESCRIPTIONSPATH = @"GameDescriptions/";
        private string saveFileLocation = Android.App.Application.Context.FilesDir.AbsolutePath;

        // We can simply change were to save the data; 
        // private string saveFileLocation = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        // ----------------------------------------------------------------------------------------------------------------
        // Constructor
        public FileStorage(Android.Content.Res.AssetManager assets)
        {
            this.assets = assets;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Adds the game the player has clicked on.
        // @param g must be a predefined game in the Game Interface. 
        public void assignGameFilePaths(Game game)
        {
            // We determine if the variables have already been set,
            // If they have, then the files have already been created.
            if (game.gOnlineFileName == null && game.gLocalFileName == null)
            {
                // Create the Local, and Online txt files.
                createFilesForGame(game);

                // Add the predefined data from the Assets folder.
                // These will similar to the scores that are inbuilt for a game at an actual arcade. 
                addPredefinedData(game);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Will create the Local (.txt), and Online(.txt) file for a particular game (instance variable),
        // And saves the file path, and name into the instance variables of game.
        private void createFilesForGame(Game game)
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

                if (!Directory.Exists(game.gLocalPath))
                {
                    // Save the name so to be used later on with extracting, and inserting new data.
                    game.gLocalFileName = gTitleTrimmed + "Local.txt";
                    directory = SCOREFILESPATH + "Local/";

                    // Combine the two strings together.
                    directory = Path.Combine(saveFileLocation.ToString(), directory);

                    // Create the directory. This directory will contain the subfolder with all the data
                    // of scores by the player, and players around the world.
                    Directory.CreateDirectory(directory);

                    // Save the directory.
                    game.gLocalPath = directory;

                    // Create the files we will be using to store the data.
                    FileStream file = File.Create(directory + game.gLocalFileName);

                    // Most important we close the connection to the file.
                    file.Close();
                }

                if (!Directory.Exists(game.gOnlinePath))
                {
                    // Save the name so to be used later on with extracting, and inserting new data.
                    game.gOnlineFileName = gTitleTrimmed + "Online.txt";
                    directory = SCOREFILESPATH + "Online/";

                    // Combine the two strings together.
                    directory = Path.Combine(saveFileLocation.ToString(), directory);

                    // Create the directory. This directory will contain the subfolder with all the data
                    // of scores by the player, and players around the world.
                    Directory.CreateDirectory(directory);

                    // Save the directory.
                    game.gOnlinePath = directory;

                    // Create the files we will be using to store the data.
                    FileStream file = File.Create(directory + game.gOnlineFileName);

                    // Most important we close the connection to the file.
                    file.Close();
                }
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Will add predefined data that is for the local leaderboard, notice how there isn't a need for an online .txt predefined data.
        // As the online should be purely about global players.
        private void addPredefinedData(Game game)
        {
            string assetsFile = "";
            string gameFilePath = "";
<<<<<<< HEAD
=======

>>>>>>> origin/master
            assetsFile = SCOREFILESPATH + "Local/" + game.glocalTestFile;
            gameFilePath = game.gLocalPath + game.gLocalFileName;
            // Open a new connection to the .txt file, so we may extract the data.
            using (StreamReader sr = new StreamReader(assets.Open(assetsFile)))
            {
                // Open a new connection to the .txt file, so we can insert the new data.
                using (StreamWriter sw = new StreamWriter(gameFilePath))
                {
                    while (sr.Peek() > -1)
                    {
                        // Extract all the data from file located in the Assets folder. 
                        sw.WriteLine(sr.ReadLine());
                    }

                    // Close the connection to the file (.txt).
                    sw.Close();

                }
                // Close the connection to the file (.txt).
                sr.Close();
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Removes the string (by not adding it to the list) from the .txt file Local or online. 
        // @param position must be determined.
        // @param position will be either: 
        //      - Greater than or equal to 20.
        //      - Less than or equal to 100.
        public void removeData(string gameFilePath, int position)
        {
            string lineScore = "";
            List<string> scoreData = new List<string>();
            int currentPosition = 0;

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

                    if (currentPosition < position)
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
        // Adds the @param score into the .txt file determined by the @param isOnline.
        // @param score will be formatted by the LeaderboardInterface, as position-name-score-dif-time
        public void addData(string gameFilePath, string score)
        {
            // Write to the Local file containing scores.
            // We are appending the @param score into the .txt file.
            using (StreamWriter sw = File.AppendText(gameFilePath))
            {
                sw.WriteLine(score);
                sw.Close();
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Returns the description of the current being played game. 
        // A description has to be defined in the Assets folder - 'Description' as a txt file.
        public string readDescription(string fileName)
        {
            string path = GAMEDESCRIPTIONSPATH + fileName;
            string content = "";

            // Open a new connection to the Assets folder, so we may extract the description from the .txt file.
            using (StreamReader sr = new StreamReader(assets.Open(path)))
            {
                // Read from top to bottom of the file.
                content = sr.ReadToEnd();
                sr.Close();
            }

            return content;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Reads all the data from either the local, or online file determined by the @param isOnline. 
        public List<string> readData(string path)
        {
            List<string> scoreLines = new List<string>();
            // Open a connection to the data file. 
            System.Diagnostics.Debug.WriteLine("READ DATA: " + path);
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() > -1)
                {
                    scoreLines.Add(sr.ReadLine());
                }

                sr.Close();
            }
            return scoreLines;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Updates all positions for each line score. 
        // @param atPosition must be either:
        //      - Greater than or equal to 20.
        //      - Less than or equal to 100.
        public void updateData(string path, int atPosition)
        {
            string lineScore = "";
            List<string> scoreData = new List<string>();
            int currentPosition = 0;
            int newPosition = 0;

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

                    // The position of the players score is at the start of the string. 
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
        // Determines if the file has reached its capacity. 
        // @param limit must be either:
        //      - Greater than or equal to 20.
        //      - Less than or equal to 100.
        public bool reachedLimit(string path, int limit)
        {
            int count = 0;
            //Open a connection to the txt file.
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
    }
}