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
/// Creator: Martin O'Connor
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

        private static int localPosition = 0;
        private static int onlinePosition = 0;
        // ----------------------------------------------------------------------------------------------------------------
        // Populates the Leader board with data of scores that are either from the local, or online text files.
        public static List<LeaderBoard> PopulateLeaderBoardData(bool isOnline)
        {
            string path = "";
            if (isOnline)
            {
                path = GameInterface.getCurrentGame().gOnlinePath + GameInterface.getCurrentGame().gOnlineFileName;
            }
            else
            {
                path = GameInterface.getCurrentGame().gLocalPath + GameInterface.getCurrentGame().gLocalFileName;
            }

            // A particular .txt file (local, or online) will be used determined by the string (path) parameter.
            List<string> unsortedList = ScarStorageSystem.readData(path);
            
            List<LeaderBoard> unsortedLb = new List<LeaderBoard>();

            if (unsortedList != null)
            {
                unsortedLb = addLeaderboardObjects(unsortedList, isOnline);
            }

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

            string localPath = GameInterface.getCurrentGame().gLocalPath + GameInterface.getCurrentGame().gLocalFileName;
            string onlinePath = GameInterface.getCurrentGame().gOnlinePath + GameInterface.getCurrentGame().gOnlineFileName;
            // push scores up by one, starting after the onlinePosition (localPosition + 1)
            ScarStorageSystem.updateData(localPath, localPosition);
            if (ScarStorageSystem.reachedLimit(localPath, MAXNUMBEROFLOCALSCORES))
            {
                // Now we remove the score that has been pushed up to 21
                ScarStorageSystem.removeData(localPath, MAXNUMBEROFLOCALSCORES + 1);
            }
            // Now we can add the new score.
            // This will be in the format Position-Name-Diff-Score-Time
            ScarStorageSystem.addData(localPath, localPosition + "-" + playersScore);

            if (onlinePosition <= MAXNUMBEROFONLINESCORES)
            {
                // push scores up by one, starting after the onlinePosition (onlinePosition + 1)
                ScarStorageSystem.updateData(onlinePath, onlinePosition);
                if (ScarStorageSystem.reachedLimit(onlinePath, MAXNUMBEROFONLINESCORES))
                {
                    // Now we remove the score that has been pushed up to 101.
                    ScarStorageSystem.removeData(onlinePath, MAXNUMBEROFONLINESCORES + 1);
                }
                // Now we can add the new score. 
                // This will be in the format Position-Name-Diff-Score-Time
                ScarStorageSystem.addData(onlinePath, onlinePosition + "-" + playersScore);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Determines if the @param scoreStr, and timeStr is a players new high score. 
        // All parameters must have been extracted from a players score data (Name-Diff-Score-Time). 
        // @timeStr will be in the form of either HH:MM:SS or MM:SS
        public static bool newHighTimeScore(string scoreStr, string timeStr, string difStr)
        {
            int score = Convert.ToInt32(scoreStr);
            int minutes = 0;
            int seconds = 0;

            // Counts the number of special characters in timeStr,
            // In this case we are counting the number of ":"
            // There will be two if timeStr is in the format of HH:MM:SS
            // Otherwise there will be only one MM:SS
            int count = GlobalApp.findNumberOfCharacters(":", timeStr);

            // What we are determining is if the timeStr has 2 ":";
            // if it does have 2 ":", the format of timeStr is HH:MM:SS
            if (count < 2)
            {
                // First part of the string
                minutes = Convert.ToInt32(GlobalApp.splitString(timeStr, 0, ':'));

                // Second part of the string
                seconds = Convert.ToInt32(GlobalApp.splitString(timeStr, 1, ':'));

                localPosition = determinePosition(false, score, 0, minutes, seconds);
                onlinePosition = determinePosition(true, score, 0, minutes, seconds);
            }
            else
            {
                // First part of the string
                int hours = Convert.ToInt32(GlobalApp.splitString(timeStr, 0, ':'));

                // Second part of the string
                minutes = Convert.ToInt32(GlobalApp.splitString(timeStr, 1, ':'));

                // Third part of the string
                seconds = Convert.ToInt32(GlobalApp.splitString(timeStr, 2, ':'));

                localPosition = determinePosition(false, score, hours, minutes, seconds);
                onlinePosition = determinePosition(true, score, hours, minutes, seconds);
            }
            return localPosition <= MAXNUMBEROFLOCALSCORES || onlinePosition <= MAXNUMBEROFONLINESCORES;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Determines the position of the players score, by comparing the time it took to complete the game
        // @params score, hours, minutes, and seconds must be properly extracted from a string.
        private static int determinePosition(bool isOnline, int score, int hours, int minutes, int seconds)
        {
            int currentHours = 0;
            int currentMinutes = 0;
            int currentSeconds = 0;
            int position = 1;
       
            List<LeaderBoard> listLBd = PopulateLeaderBoardData(isOnline);

            for (int i = 0; i < listLBd.Count; i++)
            {
                if (hours == 0)
                {

                    // Extract the new comparable values from the strings stored in listLbd.
                    // We are extracting particular values separated by the special character ":"
                    currentMinutes = Convert.ToInt32(GlobalApp.splitString(listLBd[i].lbTime, 0 ,':'));
                    currentSeconds = Convert.ToInt32(GlobalApp.splitString(listLBd[i].lbTime, 1, ':'));
                    if (minutes > currentMinutes)
                    {
                        position++;
                    }
                    else if (minutes == currentMinutes)
                    {
                        if (seconds > currentSeconds)
                        {
                            position++;
                        }
                    }
                }
                else
                {
                    // Extract the new comparable values from the strings stored in listLbd.
                    // We are extracting particular values separated by the special character ":"
                    currentHours = Convert.ToInt32(GlobalApp.splitString(listLBd[i].lbTime, 0, ':'));
                    currentMinutes = Convert.ToInt32(GlobalApp.splitString(listLBd[i].lbTime, 1, ':'));
                    currentSeconds = Convert.ToInt32(GlobalApp.splitString(listLBd[i].lbTime, 2, ':'));
                    if (hours > currentHours)
                    {
                        position++;
                    }
                    else if (hours == currentHours)
                    {
                        if (minutes > currentMinutes)
                        {
                            position++;
                        }
                        else
                        {
                            if (minutes == currentMinutes)
                            {
                                if (seconds > currentSeconds)
                                {
                                    position++;
                                }
                            }
                        }
                    }
                }
            }
            return position;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Adds LeaderBoard objects into a List, data will be extracted from @param list.
        // @param list will have been populated with data in the form of Position-Name-Diff-Score-Time
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

                temp.Add(new LeaderBoard
                {
                    lbPosition = Convert.ToInt32(GlobalApp.splitString(line, 0, '-')),
                    lbName = GlobalApp.splitString(line, 1, '-'),
                    lbScore = Convert.ToInt32(GlobalApp.splitString(line, 2, '-')),
                    lbDiff = Convert.ToInt32(GlobalApp.splitString(line, 3, '-')),
                    lbTime = GlobalApp.splitString(line, 4, '-')
                });
            }
            return temp;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Standard format of how the data will be stored.
        // @param time will be in the format of either HH:MM:SS or MM:SS,
        // @param score, and dif will not equal or less than 0.
        public static string formatLeaderBoardScore(string name, string score, int dif, string time)
        {
            return name + "-" + score + "-" + dif + "-" + time;
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
        //will detirmine which column the game wants to sort by first and by what method
        private static List<LeaderBoard> LeaderBoardSort(List<LeaderBoard> sortedList, Game game)
        {
            int colPos = 0;
            int colNum;
            int colSortBy;

            //first sort
            colPos++;
            colNum = game.gLeaderBoardCol1;
            colSortBy = game.gLeaderBoardCol1SortBy;
            LeaderBoardSortBy(sortedList, game, colNum, colSortBy, colPos);

            //second sort
            colPos++;
            colNum = game.gLeaderBoardCol2;
            colSortBy = game.gLeaderBoardCol2SortBy;
            LeaderBoardSortBy(sortedList, game, colNum, colSortBy, colPos);

            //third sort
            colPos++;
            colNum = game.gLeaderBoardCol3;
            colSortBy = game.gLeaderBoardCol3SortBy;
            LeaderBoardSortBy(sortedList, game, colNum, colSortBy, colPos);


            return sortedList;
        }

        //is the sorting methoed for leaderboards that sorts column how the game wants them sorted, while taking into account the previous sorts if any
        private static List<LeaderBoard> LeaderBoardSortBy(List<LeaderBoard> sortedList, Game game, int colNum, int colSortBy, int colPos)
        {
            int position = 0;
            LeaderBoard temp;

            //sort colNum acending and make sure  it doesn't effect previous colPos's
            if (colSortBy == 1)
            {

                for (int x = 0; x < sortedList.Count - 1; x++)
                {
                    position = x;
                    for (int y = x + 1; y < sortedList.Count; y++)
                    {
                        //check which colNum we are sorting
                        switch (colNum)
                        {
                            //sort dif
                            case 1:
                                if (sortedList[y].lbDiff < sortedList[position].lbDiff)
                                {
                                    //if 1st sort, just sort
                                    if (colPos == 1)
                                    {
                                        position = y;
                                    }
                                    //if 2nd sort, don't affect 1st sort
                                    if (colPos == 2)
                                    {
                                        if (game.gLeaderBoardCol1SortBy == 1)
                                        {
                                            switch (game.gLeaderBoardCol1)
                                            {
                                                case 1:
                                                    if (sortedList[y].lbDiff <= sortedList[position].lbDiff)
                                                    {

                                                    }
                                                    break;
                                                case 2:
                                                    if (sortedList[y].lbScore <= sortedList[position].lbScore)
                                                    {
                                                        position = y;
                                                    }
                                                    break;
                                                case 3:
                                                    /*
                                                    if (sortedList[y].lbTime < sortedList[position].lbTime)
                                                    {
                                                        position = y;
                                                    }*/
                                                    break;
                                            }
                                        }
                                        else if (game.gLeaderBoardCol1SortBy == 2)
                                        {
                                            switch (game.gLeaderBoardCol1)
                                            {
                                                case 1:
                                                    if (sortedList[y].lbDiff >= sortedList[position].lbDiff)
                                                    {

                                                    }
                                                    break;
                                                case 2:
                                                    if (sortedList[y].lbScore >= sortedList[position].lbScore)
                                                    {
                                                        position = y;
                                                    }
                                                    break;
                                                case 3:
                                                    /*
                                                    if (sortedList[y].lbTime > sortedList[position].lbTime)
                                                    {
                                                        position = y;
                                                    }*/
                                                    break;
                                            }
                                        }
                                    }
                                    //if 3rd sort don't affect 1st and 2nd sorts
                                    if (colPos == 3)
                                    {



                                    }
                                }
                                break;
                            case 2:
                                if (sortedList[y].lbScore < sortedList[position].lbScore)
                                {
                                    position = y;
                                }
                                break;
                            case 3:
                                /*
                                if (sortedList[y].lbTime < sortedList[position].lbTime)
                                {
                                    position = y;
                                }*/
                                break;
                            default:
                                break;
                        }

                    }
                    if (position != x)
                    {
                        temp = sortedList[x];
                        sortedList[x] = sortedList[position];
                        sortedList[position] = temp;
                        sortedList[position].lbPosition = position;
                    }
                }
                return sortedList;

            }
            //sort colNum decending and make sure  it doesn't effect previous colPos's
            else if (colSortBy == 2)
            {

            }
            // ifanything else do not sort


            return sortedList;
        }
    }
}