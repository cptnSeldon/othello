using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPlayable;

namespace OthelloIA8
{
    public class OthelloBoard : IPlayable.IPlayable
    {
        private Data board;

        public const int SIZE_GRID = 8;
        public const int DEPTH = 5;
        private bool whiteTurn;


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
            this.whiteTurn = isWhite;
            return board.isPlayable(column, line, isWhite);
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            this.whiteTurn = isWhite;
            return board.playMove(column, line, isWhite);
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            this.board = new Data(whiteTurn, game);
            this.whiteTurn = whiteTurn;

            Tuple<int, Tuple<int, int>> moveToPlay = Alphabot(board, DEPTH, 0, 0, true);

            return moveToPlay.Item2;
        }

        public int[,] GetBoard()
        {
            return this.board.getBoard();
        }

        #endregion

        public bool IsTerminated(Data node)
        {
            Dictionary<String, List<Tuple<int, int>>> possibleMoves = board.getPossibleMoves();

            return possibleMoves.Count() == 0;
        }

        /** 
         * -> must have : next moves list -> retrieve data if move is played -> NODES
         * -> 
         *  */
        public Tuple<int, Tuple<int, int>> Alphabot(Data node, int depth, int alpha, int beta, bool isPlayerToMaximize)
        {
            if (depth == 0 || IsTerminated(node))
                return new Tuple<int, Tuple<int, int>>(HeuristicEvaluation(node), node.LastPlayedMove());

            //IF MAXIMIZING PLAYER == TRUE : nodes -> next board state right after my move
            if (isPlayerToMaximize)
            {
                int value = int.MinValue;
                Tuple<int, int> moveToPlay = new Tuple<int, int>(-1, -1);

                foreach (Data child in node.GetChildNodes())
                {
                    // TODO : Vérifier si c'est bien true comme dernier paramètre
                    Tuple<int, Tuple<int, int>> computedMove = Alphabot(child, depth - 1, alpha, beta, true);

                    if (computedMove.Item1 > value)
                    {
                        value = computedMove.Item1;
                        if (depth == DEPTH - 1)
                            moveToPlay = node.LastPlayedMove();

                        if (depth == DEPTH)
                            moveToPlay = computedMove.Item2;
                    }

                    // value = Math.Max(value, computedMove.Item1);

                    alpha = Math.Max(alpha, value);

                    if (beta <= alpha)
                    {
                        break; // beta cutoff
                    }
                }


                return new Tuple<int, Tuple<int, int>>(value, moveToPlay);
            }
            //IF MAXIMIZING PLAYER == FALSE
            else
            {
                int value = int.MaxValue;
                Tuple<int, int> moveToPlay = new Tuple<int, int>(-1, -1);

                foreach (Data child in node.GetChildNodes())
                {
                    Tuple<int, Tuple<int, int>> computedMove = Alphabot(child, depth - 1, alpha, beta, false);

                    if (computedMove.Item1 < value)
                    {
                        value = computedMove.Item1;

                        if (depth == DEPTH - 1)
                            moveToPlay = node.LastPlayedMove();

                        if (depth == DEPTH)
                            moveToPlay = computedMove.Item2;
                    }

                    alpha = Math.Min(alpha, value);

                    if (beta <= alpha)
                        break; // aplha cutoff
                }

                return new Tuple<int, Tuple<int, int>>(value, moveToPlay);
            }
        }

        //TODO : (players' score delta)
        public int HeuristicEvaluation(Data node)
        {
            return node.getScore(whiteTurn); ;
        }

        public int GetWhiteScore()
        {
            return board.getScore(this.whiteTurn);
        }

        public int GetBlackScore()
        {
            return board.getScore(this.whiteTurn);
        }
    }
}