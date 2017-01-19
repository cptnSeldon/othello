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
    class GameData : IPlayable
    {

        /************************
         *      ATTRIBUTES      *
         ************************/

        //board
        public BoardState[,] StateArray
        {
            get;
            set;
        }
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
        public GameData()
        {
            //board initialization
            StateArray = new BoardState[8, 8];
            FillArray();

            //black
            blackScore = 0;
            blackRemainingDiscs = 32;
            blackRemainingTime = 1000;
            //white
            whiteScore = 0;
            whiteRemainingDiscs = 32;
            whiteRemainingTime = 1000;
            
        }

        //Debug test : fill array to random
        //inside method : fill array
        private void FillArray()
        {

            Random random = new Random();

            // random filling
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    // <=a < b
                    int state = random.Next(1, 4);

                    if (state == 1)
                        StateArray[row, column] = BoardState.PLACED_BLACK;
                    if (state == 2)
                        StateArray[row, column] = BoardState.PLACED_WHITE;
                    if (state == 3)
                        StateArray[row, column] = BoardState.HIDDEN;
                }
            }
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
