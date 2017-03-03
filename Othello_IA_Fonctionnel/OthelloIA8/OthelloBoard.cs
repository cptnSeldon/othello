using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPlayable;

namespace OthelloIA9
{
    public class OthelloBoard : IPlayable.IPlayable
    {
        /*
         * matrice d'indication des poids de chaque cases
        500  -150 30 10 10 30 -150 500
        -150 -250 0  0  0  0  -250 -150
        30   0    1  2  2  1  0    30
        10   0    2  16 16 2  0    10
        10   0    2  16 16 2  0    10
        30   0    1  2  2  1  0    30
        -150 -250 0  0  0  0  -250 -150
        500  -150 30 10 10 30 -150 500
        */
        public static readonly int[,] WEIGHT_MATRIX = {
            {500, -150, 30, 10, 10, 30, -150, 500},
            {-150, -250, 0, 0, 0, 0, -250, -150},
            {30, 0, 1, 2, 2, 1, 0, 30},
            {10, 0, 2, 16, 16, 2, 0, 10},
            {10, 0, 2, 16, 16, 2, 0, 10},
            {30, 0, 1, 2, 2, 1, 0, 30},
            {-150, -250, 0, 0, 0, 0, -250, -150},
            {500, -150, 30, 10, 10, 30, -150, 500}
        };

        private int[] scores = { 2, 2 };
        
        public int BlacksScore
        {
            get { return scores[1]; }
            set { scores[1] = value; }
        }

        public int WhitesScore
        {
            get { return scores[0]; }
            set { scores[0] = value; }
        }

        private Data board;

        public const int SIZE_GRID = 8;

        // clé: string représentant un mouvement, exemple : "07"
        // valeur: liste des tuiles capturées si ce mouvement est effectué
        //private Dictionary<string, List<Tuple<int, int>>> possibleMoves;

        public OthelloBoard()
        {
            board = new Data(false);
        }

        #region IPlayable implementation
        public string GetName()
        {
            return "8: Nemeth_Roy";
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            return board.isPlayable(column, line, isWhite);
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            return board.playMove(column, line, isWhite);
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            this.board = new Data(whiteTurn, game);

            Dictionary<String, List<Tuple<int, int>>> possibleMoves = board.getPossibleMoves();

            if (possibleMoves.Count > 0)
            { 
                string key = "00";

                foreach (KeyValuePair<string, List<Tuple<int, int>>> entry in possibleMoves)
                {
                    //TODO : Travail correct à faire ici
                    key = entry.Key;
                }
            
                return board.stringToTuple(key);
            }
            else
            {
                return new Tuple<int, int>(-1, -1);
            }
        }

        public int[,] GetBoard()
        {
            return this.board.getBoard();
        }

        public int GetWhiteScore()
        {
            return WhitesScore;
        }

        public int GetBlackScore()
        {
            return BlacksScore;
        }

        private void incrementScore(int player, int delta)
        {
            if (player == 1)
                BlacksScore += delta;
            else if (player == 0)
                WhitesScore += delta;
        }
        #endregion
        
        
        #region IA
       

        private int score(int[,] board, int player)
        {
            int[] scores = { 0, 0 };
            for (int y = 0; y < SIZE_GRID; y++)
            {
                for (int x = 0; x < SIZE_GRID; x++)
                {
                    if (board[x, y] != -1)
                        scores[board[x, y]] += WEIGHT_MATRIX[x,y];
                }
            }
            return scores[player];
            // mon score moins le score de l'autre
            //return scores[player] - scores[1 - player];
        }

        #endregion
    }

    public class Data
    {
        public enum EtatCase
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
            if (board[column, line] != (int)EtatCase.Empty)
                return;

            //check Player's color
            int myColor;
            int opponentColor;

            if (isWhite)
            {
                opponentColor = (int)EtatCase.Black;
                myColor = (int)EtatCase.White;
            }
            else
            {
                opponentColor = (int)EtatCase.White;
                myColor = (int)EtatCase.Black;
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
            int playerColor = isWhite ? (int)EtatCase.White : (int)EtatCase.Black;

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
                                                       { -1 ,-1 ,-1 , 0 , 1 ,-1 ,-1 ,-1 },
                                                       { -1 ,-1 ,-1 , 1 , 0 ,-1 ,-1 ,-1 },
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
                    if (board[i, j] == (int)EtatCase.Black)
                        build.Append("X");

                    if (board[i, j] == (int)EtatCase.White)
                        build.Append("O");

                    if (board[i, j] == (int)EtatCase.Empty)
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
