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
/// Creator: Martin O'Connor
/// Student number: 3179234
/// Student number: 3279660
/// Date modified: 08-Apr-2017
/// Date created: 08-Apr-2017
/// </summary>
namespace SCaR_Arcade
{
    static class LeaderBoardInterface
    {
        // The top 20 of the players scores will be added, and shown.
        private const int MAXNUMBEROFLOCALSCORES = 20;

        // The top 100 of players scores around the world will be added, and shown.
        private const int MAXNUMBEROFONLINESCORES = 100;

        private static int localPosition = 1;
        private static int onlinePosition = 1;
        // ----------------------------------------------------------------------------------------------------------------
        // Populates the Leader board with data of scores that are either from the local, or online text files.
        public static List<LeaderBoard> PopulateLeaderBoardData(bool isOnline)
        { 
            // A particular .txt file (local, or online) will be used determined by the boolean parameter.
            List<string> unsortedList = FileInterface.readFromScoreFile(isOnline);

            // This list will be sorted;
            List<LeaderBoard> unsortedLb = new List<LeaderBoard>();

            if (unsortedList != null)
            {
                unsortedLb = addLeaderboardObjects(unsortedList, isOnline);
            }
            // Return a sorted Leaderboard list. 
            return selectionSort(unsortedLb);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // @param playersScore should be in the format Name-Score-Time
        // Will format the @parameter playersScore into the format Position-Name-Score-Time
        // Position - The position the player is in the list.
        // Name - Name of the player.
        // Score - The number of moves taken to win the game.
        // Time - How long it took to win the game. 
        public static void addNewScore(string playersScore)
        {

            if (FileInterface.fileReachedLimit(false, MAXNUMBEROFLOCALSCORES))
            {
                // Remove the current score at position localPosition;
                FileInterface.removeScoreAtPosition(false, localPosition);
            }
            else
            {
                // push scores up by one, starting after the localPosition
                FileInterface.updateData(false, localPosition);
            }
            // Now we can add the new score.
            // This will be in the format Position-Name-Score-Time
            FileInterface.addDataToFile(false, localPosition + "-" + playersScore);

            if (onlinePosition <= MAXNUMBEROFONLINESCORES)
            {
                if (FileInterface.fileReachedLimit(true, MAXNUMBEROFONLINESCORES))
                {
                    // Remove the current score at position onlinePosition;
                    FileInterface.removeScoreAtPosition(true, onlinePosition);
                }
                else
                {
                    // push scores up by one, starting after the onlinePosition;
                    FileInterface.updateData(true, onlinePosition);
                }
                // Now we can add the new score. 
                // This will be in the format Position-Name-Score-Time
                FileInterface.addDataToFile(true, onlinePosition + "-" + playersScore);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // 
        public static bool newHighTimeScore(string scoreStr, string timeStr, string difStr)
        {
            int score = Convert.ToInt32(scoreStr);
            int hours = 0;
            int minutes = 0;
            int seconds = 0;

            // Counts the number of ":" in timeStr,
            // There will be two if timeStr is in the format of HH:MM:SS
            // Otherwise there will be only one MM:SS
            int count = GlobalApp.findNumberOfCharacters(":", timeStr);

            if (count < 2)
            {
                // First part of the string
                minutes = Convert.ToInt32(GlobalApp.extractValuesFromString(":", timeStr, false));

                // Second part of the string
                seconds = Convert.ToInt32(GlobalApp.extractValuesFromString(":", timeStr, true));
            }
            else
            {
                // First part of the string
                hours = Convert.ToInt32(GlobalApp.extractValuesFromString(":", timeStr, false));

                // Second part of the string
                minutes = Convert.ToInt32(GlobalApp.extractValuesFromString(":", timeStr, true));

                // Third part of the string
                seconds = Convert.ToInt32(timeStr.Substring(timeStr.LastIndexOf(":"), 2));
            }

            localPosition = determinePosition(false, score, hours, minutes, seconds);
            onlinePosition = determinePosition(true, score, hours, minutes, seconds);
            return localPosition <= MAXNUMBEROFLOCALSCORES || onlinePosition <= MAXNUMBEROFONLINESCORES;
        }
        // ----------------------------------------------------------------------------------------------------------------
        //
        private static int determinePosition(bool isOnline, int score, int hours, int minutes, int seconds)
        {
            int currentHours = 0;
            int currentMinutes = 0;
            int currentSeconds = 0;
            int position = 0;
            if (isOnline)
            {
                position = MAXNUMBEROFONLINESCORES;
            }
            else
            {
                position = MAXNUMBEROFLOCALSCORES;
            }
            // The method populateLeaderBoardData will have been sorted.
            List<LeaderBoard> listLBd = PopulateLeaderBoardData(isOnline);
            for (int i = listLBd.Count - 1; i > 0; i--)
            {
                if (hours == 0)
                {
                    currentMinutes = Convert.ToInt32(GlobalApp.extractValuesFromString(":", listLBd[i].lbTime, false));
                    currentSeconds = Convert.ToInt32(GlobalApp.extractValuesFromString(":", listLBd[i].lbTime, true));
                    if (minutes < currentMinutes)
                    {
                        position--;
                    }
                    else if (minutes == currentMinutes)
                    {
                        if (seconds < currentSeconds)
                        {
                            position--;
                        }
                    }
                }
                else
                {
                    currentHours = Convert.ToInt32(GlobalApp.extractValuesFromString(":", listLBd[i].lbTime, false));
                    currentMinutes = Convert.ToInt32(GlobalApp.extractValuesFromString(":", listLBd[i].lbTime, true));
                    currentSeconds = Convert.ToInt32(listLBd[i].lbTime.Substring(listLBd[i].lbTime.LastIndexOf(":"), 2));
                    if (hours < currentHours)
                    {
                        position--;
                    }
                    else if (hours == currentHours)
                    {
                        if (minutes < currentMinutes)
                        {
                            position--;
                        }
                        else
                        {
                            if (minutes == currentMinutes)
                            {
                                if (seconds < currentSeconds)
                                {
                                    position--;
                                }
                            }
                        }
                    }
                }
            }
            return position;  
        }
        // ----------------------------------------------------------------------------------------------------------------
        //
        private static List<LeaderBoard> addLeaderboardObjects(List<string> list, bool isOnline)
        {
            int count = 0;
            if (isOnline)
            {
                count = MAXNUMBEROFONLINESCORES;
            }
            else
            {
                count = MAXNUMBEROFLOCALSCORES;
            }
            // This list will be sorted;
            List<LeaderBoard> temp = new List<LeaderBoard>();
            for (int i = 0; i < count && i < list.Count; i++)
            {
                // Return the data (string) and index i;
                string line = list[i];

                Char delimiter = '-';
                String[] subStrings = line.Split(delimiter);

                temp.Add(new LeaderBoard
                {
                    lbPosition = Convert.ToInt32(subStrings[0]),
                    lbName = subStrings[1],
                    lbScore = Convert.ToInt32(subStrings[2]),
                    lbDiff = Convert.ToInt32(subStrings[3]),
                    lbTime = subStrings[4]
                });
            }
            return temp;
        }
        // ----------------------------------------------------------------------------------------------------------------
        //
        public static string formatLeaderBoardScore(string name, string score,int dif, string time)
        {
            return name + "-" + score + "-" + dif + "-"+ time;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // We will sort the entire list here, using the sorting algorithm selection.
        // Resource: http://cforbeginners.com/CSharp/SelectionSort.html helped us create the algorithm.
        // Returns a list of Leaderboard data sorted in ascending order, by the instance variable lbPosition.
        private static List<LeaderBoard> selectionSort(List<LeaderBoard> sortedList)
        {
            int position = 0;
            LeaderBoard temp;
            for (int x = 0; x < sortedList.Count - 1; x++)
            {
                position = x;
                for (int y = x + 1; y < sortedList.Count; y++)
                {
                    if (sortedList[y].lbPosition < sortedList[position].lbPosition)
                    {
                        position = y;
                    }
                }
                if (position != x)
                {
                    temp = sortedList[x];
                    sortedList[x] = sortedList[position];
                    sortedList[position] = temp;
                }
            }
            return sortedList;
        }
    }
}