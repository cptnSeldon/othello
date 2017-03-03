using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;

namespace OthelloIA8
{
    /// <summary>
    /// Class of state data which contains a board grid, the possible moves and the 
    /// tiles to move for each possible move
    /// </summary>
    public class Data
    {
        public enum StateCase
        {
            Empty = -1,
            White = 0,
            Black = 1
        }

        private const int BOARDSIZE = 8;

        private int[,] board;
        private Dictionary<string, List<Tuple<int, int>>> possibleMoves;
        private bool isWhite;

        public Data(bool isWhite)
        {
            this.board = initializeBoard(board);
            this.possibleMoves = new Dictionary<string, List<Tuple<int, int>>>();
            this.isWhite = isWhite;
            computeMoves(isWhite);
        }

        // We don't call the parent constructor to not compute two times the possible moves
        public Data(bool isWhite, int[,] board)
        {
            this.board = board;
            this.possibleMoves = new Dictionary<string, List<Tuple<int, int>>>();
            this.isWhite = isWhite;
            computeMoves(isWhite);
        }

        private void computeMoves(bool isWhite)
        {
            //clear moves list
            possibleMoves.Clear();

            // adds all available tiles
            for (int column = 0; column < BOARDSIZE; column++)
            {
                for (int line = 0; line < BOARDSIZE; line++)
                {
                    //checks move's validity
                    computeMove(column, line, isWhite);
                }
            }
        }

        private void computeMove(int column, int line, bool isWhite)
        {
            //check if tile is already taken
            if (board[column, line] != (int)StateCase.Empty)
                return;

            //check Player's color
            int myColor;
            int opponentColor;

            if (isWhite)
            {
                opponentColor = (int)StateCase.Black;
                myColor = (int)StateCase.White;
            }
            else
            {
                opponentColor = (int)StateCase.White;
                myColor = (int)StateCase.Black;
            }


            //TODO : up, down, left, right
            //       diagonals left-to-right up, down
            //       diagonals right-to-left up, down

            //TEST 1
            List<Tuple<int, int>> neighborhood = new List<Tuple<int, int>>();

            for (int i = column - 1; i <= column + 1; i++)
            {
                for (int j = line - 1; j <= line + 1; j++)
                {
                    if (i < BOARDSIZE &&
                        i > 0 &&
                        j < BOARDSIZE &&
                        j > 0)
                    {
                        if (board[i, j] == opponentColor)
                            neighborhood.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            if (neighborhood.Count == 0)
                return;

            //TEST 2
            List<Tuple<int, int>> catchedTiles = new List<Tuple<int, int>>();

            foreach (Tuple<int, int> neighborn in neighborhood)
            {
                int dx = neighborn.Item1 - column;
                int dy = neighborn.Item2 - line;
                int x = neighborn.Item1;
                int y = neighborn.Item2;
                List<Tuple<int, int>> temp = new List<Tuple<int, int>>();

                while ((x + dx < BOARDSIZE &&
                        x + dx > -1 &&
                        y + dy < BOARDSIZE &&
                        y + dy > -1) &&
                        board[x, y] == opponentColor)
                {
                    temp.Add(new Tuple<int, int>(x, y));
                    x = x + dx;
                    y = y + dy;
                }

                if (board[x, y] == myColor)
                    catchedTiles.AddRange(temp);
            }
            if (catchedTiles.Count == 0)
                return;

            possibleMoves.Add(tupleToString(column, line), catchedTiles);

            return;
        }

        public bool playMove(int column, int line, bool isWhite)
        {
            int playerColor = isWhite ? (int)StateCase.White : (int)StateCase.Black;

            // To be sure we are always in a correct state
            computeMoves(isWhite);

            if (isPlayable(column, line, isWhite))
            {
                board[column, line] = playerColor;

                foreach (Tuple<int, int> item in possibleMoves[tupleToString(column, line)])
                {
                    board[item.Item1, item.Item2] = playerColor;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isPlayable(int column, int line, bool isWhite)
        {
            if (this.isWhite != isWhite)
            {
                computeMoves(isWhite);
                this.isWhite = isWhite;
            }

            return possibleMoves.ContainsKey(tupleToString(column, line));
        }


        public int[,] initializeBoard(int[,] board)
        {
            // Initialization with 4;3 and 3;4 in white, and 4;4 and 3;3 in black
            board = new int[BOARDSIZE, BOARDSIZE]{ { -1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 },
                                                       { -1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 },
                                                       { -1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 },
                                                       { -1 ,-1 ,-1 , 1 , 0 ,-1 ,-1 ,-1 },
                                                       { -1 ,-1 ,-1 , 0 , 1 ,-1 ,-1 ,-1 },
                                                       { -1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 },
                                                       { -1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 },
                                                       { -1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 ,-1 }
                };

            return board;
        }

        public Tuple<int, int> stringToTuple(String key)
        {
            int column = Convert.ToInt32(key[0].ToString());
            int line = Convert.ToInt32(key[1].ToString());

            return new Tuple<int, int>(column, line);
        }

        public string tupleToString(int x, int y)
        {
            return $"{x}{y}";
        }

        public string tupleToString(Tuple<int, int> tuple)
        {
            return tupleToString(tuple.Item1, tuple.Item2);
        }

        public override string ToString()
        {
            StringBuilder build = new StringBuilder();

            for (int i = 0; i < BOARDSIZE; i++)
            {
                for (int j = 0; j < BOARDSIZE; j++)
                {
                    if (board[i, j] == (int)StateCase.Black)
                        build.Append("X");

                    if (board[i, j] == (int)StateCase.White)
                        build.Append("O");

                    if (board[i, j] == (int)StateCase.Empty)
                        build.Append("-");
                }
                build.Append("\n");
            }
            return build.ToString();
        }

        public int[,] getBoard()
        {
            return this.board;
        }

        public Dictionary<string, List<Tuple<int, int>>> getPossibleMoves()
        {
            return this.possibleMoves;
        }
    }
}