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
    class Data
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

    }
}
