/* 
    This class contains the logic for the board
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    //-> game state management : data modification -> notifies UI (update)
    public class OthelloEngine : IPlayable
    {
        #region ATTRIBUTES
        #endregion

        #region METHODS

        /* Initialization */
        public OthelloEngine()
        {

        }
        /* New Game */
        public void StartNewGame()
        {
            //initialize board
            //next turn
        }

        /* Next Turn */
        public void GameStateChange()
        {
            //check if valid moves
        }

        /* Get Next Move : IPlayable */
        
        public Tuple<char, int> getNextMove(int[,] game, int level, bool whiteTurn)
        {
            //ask GameEngine next valid move given a game position
            return new Tuple<char, int>('a', 1);
        }


        /* Get Moves : IPlayable */
        public bool playMove(int column, int line, bool isWhite)
        {
            //update board status if valid move
            return true;
        }

        //returns true if move is valid
        public bool isPlayable(int column, int line, bool isWhite)
        {
            //up
            //down
            //left
            //right
            //diagonal up
            //diagonal down
            return true;
        }
        

        /* Scores : IPlayable */
        //black : BlackTimeStr
        public int getBlackScore()
        {
            //number of black discs on board
            return 0;
        }

        //white : WhiteTimeStr
        public int getWhiteScore()
        {
            //number of white discs on board
            return 0;
        }


        /* Save : BoardState[,] StateArray */

        /* Reload */

        /* Undo */
        
        #endregion
    }
}
