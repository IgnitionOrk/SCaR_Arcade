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
/// Student name: Ryan Cunneen.
/// Student Number: 3179234
/// Date created 4-Apr-2017
/// Date Modified 4-Apr-2017
/// </summary>
namespace SCaR_Arcade.GameLogic
{
    class DiceRollsLogic
    {
        // Jagged array.
        private int[][] gameBoard;
        private int width;
        private int height;
        // ----------------------------------------------------------------------------------------------------------------
        // Constructor:
        public DiceRollsLogic(int width, int height)
        {
            //Width is the number of poles;
            this.height = height;
            this.width = width;
            gameBoard = new int[width][];
            initializeGameBoard();
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Remove the components of the game;
        public void deleteBoard()
        {
            gameBoard = null;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Create the game but in array form (with populated integers as data).
        // The idea is index 0 is the furtherest pole (left), with MAXCOMPONENTS representing the last (right) pole.
        private void initializeGameBoard()
        {
            for (int x = 0; x < width; x++)
            {
                if (x == 0)     // Furthest left pole. 
                {
                    gameBoard[x] = new int[height];
                    for (int y = 0; y < height; y++)
                    {
                        gameBoard[x][y] = height - y;  // Add in values (descending) into the array index (ascending).
                    }
                }
                else
                {
                    // Every other pole will not have any disks saved into them.
                    // Therefore, their length is 0.
                    gameBoard[x] = new int[0];
                }
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Determines the optimal number of moves the player can beat the game in.
        public int calOptimalNoOfMoves(int number)
        {
            return ((int)Math.Pow(2, number)) - 1;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Determines if the game was been won.
        public bool ifWon()
        {
            return gameBoard[gameBoard.Length - 1].Length == height;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Determines whether the player is allowed to make their desired move. 
        public bool canDropDisk(int from, int to)
        {
            // Return the values from the top of there respective arrays. 
            int fromTopDisk = topIndexValue(from);
            int toTopDisk = topIndexValue(to);
            if (toTopDisk == 0)   // The array @param 'to' has length 0. Therefore, you can always be able to drop.
            {
                return true;
            }
            else
            {
                // Why is the boolean condition defined as <=, and no <?
                // Because we might be dropping the disk back into the same LinearLayout from whence it came. 
                return fromTopDisk <= toTopDisk;
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Determines the 'top' value of the array (the last value in the array).
        private int topIndexValue(int index)
        {
            int value = 0;
            if (gameBoard[index] != null && gameBoard[index].Length != 0)
            {
                // The jagged array can be thought of a representation of values in the cartesian plane.
                // gameBoard[index] can be thought of as the x component;
                // With gameBoard[index].Length - 1 as the y component;
                // Essentially we are finding the y value specified by the x component. 
                value = gameBoard[index][gameBoard[index].Length - 1];
            }
            return value;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Removes, and adds the move made by the player. 
        public void finalizeMove(int from, int to)
        {
            // Remove the top value (last value in the array).
            int iMove = this.remove(from);

            // Re-add the value back into the gameBoard.
            add(to, iMove);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Adds the @param iMove into the array specified by the @param to index;
        private void add(int to, int iMove)
        {
            int[] temp = copy(gameBoard[to], true);

            // Save iMove into the temporary array;
            temp[temp.Length - 1] = iMove;

            //Save the temporary array back into the gameBoard;
            gameBoard[to] = temp;
        }

        // ----------------------------------------------------------------------------------------------------------------
        // Remove the integer data from the array specified by the @param from index.
        private int remove(int from)
        {
            // Getting the top value from the array specifed by the @param from.
            // Before we save the new data back into the gameBoard;
            int iDisk = gameBoard[from][gameBoard[from].Length - 1];

            // Now we save the new array of integers (by creating a temporary array) into the gameBoard.
            gameBoard[from] = copy(gameBoard[from], false);
            return iDisk;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Copies a temporary array of integers.
        // The @param ifAdd specifies if we need to increase or decrease by 1 the length of the @param array. 
        private int[] copy(int[] array, bool ifAdd)
        {
            int[] temp;
            if (ifAdd)
            {
                temp = new int[array.Length + 1];
                // Because we are adding in only the contents of @param array.
                // So we can have array.length + 1;
                // Otherwise an exception will be thrown;
                for (int i = 0; i < array.Length; i++)
                {
                    temp[i] = array[i];
                }
                // The last index of the temporary array has not been populate with data as of yet.
            }
            else
            {
                temp = new int[array.Length - 1];
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = array[i];
                }
            }
            return temp;
        }
    }
}