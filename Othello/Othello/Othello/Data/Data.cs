using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Othello
{
    // Outside of the class so it is accessible everywhere
    public enum GameState { INIT, BLACK_TURN, WHITE_TURN, GAME_END };   //game main loop
    public enum BoardState { PLAYABLE_BLACK, PLAYABLE_WHITE, HIDDEN, PLACED_BLACK, PLACED_WHITE };  // the grid is populated with states

    //each new event notifies data to be changed according to the engine
    class Data : IPlayable
    {

        /************************
         *      ATTRIBUTES      *
         ************************/

        //board
        private Disc[,] board;
        //black
        private int blackScore;
        private int blackRemainingTime;
        private int blackRemainingDiscs;
        //white
        private int whiteScore;
        private int whiteRemainingTime;
        private int whiteRemainingDiscs;

        /************************
         *        METHODS       *
         ************************/
         
        //Initialization
        public Data()
        {
            //board
            board = new Disc[8, 8]; // has to be populated with BoardStates
            //black
            blackScore = 0;
            blackRemainingDiscs = 32;
            blackRemainingTime = 1000;
            //white
            whiteScore = 0;
            whiteRemainingDiscs = 32;
            whiteRemainingTime = 1000;
            
        }

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
    }
}
