using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Othello
{
    // Outside of the class so it is accessible everywhere
    public enum Colors { White, Black };

    class Data
    {

        // Attributes
        private Disc[,] board;
        private OthelloEngine othelloEngine;

        private int whiteScore;
        private int whiteRemainingTime;
        private int whiteRemainingDiscs;

        private int blackRemainingDiscs;
        private int blackRemainingTime;
        private int blackScore;

        public Data()
        {
            board = new Disc[8, 8];
            whiteScore = 0;
            whiteRemainingDiscs = 32;
            whiteRemainingTime = 1000;
            blackScore = 0;
            blackRemainingDiscs = 32;
            blackRemainingTime = 1000;
        }

    }
}
