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
/// Date created 4-Apr-2017
/// Date Modified 4-Apr-2017
/// </summary>
namespace SCaR_Arcade.GameLogic
{
    class DiceRollsLogic
    {
        // Jagged array.
        private int[] gameBoard;
        // ----------------------------------------------------------------------------------------------------------------
        // Constructor:
        public DiceRollsLogic(int numOfDice)
        {
            gameBoard = new int[numOfDice];
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Remove the components of the game;
        public void deleteBoard()
        {
            gameBoard = null;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Determines if the game was been won.
        public bool ifWon()
        {
            for(int i = 0; i < gameBoard.Length; i++)
            {
                for(int j = 0; j < gameBoard.Length; j++)
                {
                    if (i != j && gameBoard[i].Equals(gameBoard[j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Removes, and adds the move made by the player. 
        public void finalizeMove(int dieWorth,int pos)
        {
            gameBoard[pos] = dieWorth;
        }
    }
}