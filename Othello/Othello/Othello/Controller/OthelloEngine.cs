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

        //new Game

        //next Turn

        //get Next Move

        //get Moves


        #region IPLAYABLE
        public bool isPlayable(int column, int line, bool isWhite)
        {
            return true;
        }

        public bool playMove(int column, int line, bool isWhite)
        {
            return true;
        }

        public Tuple<char, int> getNextMove(int[,] game, int level, bool whiteTurn)
        {
            return new Tuple<char, int>('a', 1);
        }

        public int getWhiteScore()
        {
            return 0;
        }

        public int getBlackScore()
        {
            return 0;
        }
        #endregion
        #endregion
    }
}
