using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Project references :
/// 
/// 
/// </summary>
namespace IAOthelloConsoleTest
{
    public class OthelloBoard : IPlayable
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
            int bestScore = -100000;
            string bestMove = null;
            int score = 0;
            Data tempBoard;
            foreach (KeyValuePair<string, List<Tuple<int, int>>> entry in board.getPossibleMoves())
            {
                tempBoard = new Data(board);
                Tuple<int, int> coords = tempBoard.stringToTuple(entry.Key);
                if (tempBoard.playMove(coords.Item1, coords.Item2, whiteTurn))
                {
                    score = Alphabot(tempBoard, DEPTH, 0, 0, false);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = entry.Key;
                    }
                }
            }

            return board.stringToTuple(bestMove);
        }

        public int[,] GetBoard()
        {
            return this.board.getBoard();
        }

        #endregion

        public bool IsTerminated(Data node)
        {

            Dictionary<String, List<Tuple<int, int>>> possibleMoves = node.getPossibleMoves();

            return possibleMoves.Count() == 0;
        }

        /** 
         * -> must have : next moves list -> retrieve data if move is played -> NODES
         * -> 
         *  */
        public int Alphabot(Data node, int depth, int alpha, int beta, bool isPlayerToMaximize)
        {
            //END CONDITION -> tree root or game end
            if (depth == 0 || IsTerminated(node))
                return HeuristicEvaluation(node, isPlayerToMaximize);

            //IF MAXIMIZING PLAYER == TRUE : nodes -> next board state right after my move
            if (isPlayerToMaximize)
            {
                int value = -100000;
                foreach (Data child in node.GetChildNodes())
                {
                    value = Math.Max(value, Alphabot(child, depth - 1, alpha, beta, !isPlayerToMaximize));
                    alpha = Math.Max(alpha, value);
                    if (beta <= alpha)
                        break; // beta cutoff
                }
                return value;
            }
            //IF MAXIMIZING PLAYER == FALSE
            else
            {
                int value = 100000;
                foreach (Data child in node.GetChildNodes())
                {
                    value = Math.Min(value, Alphabot(child, depth - 1, alpha, beta, !isPlayerToMaximize));
                    alpha = Math.Min(alpha, value);
                    if (beta <= alpha)
                        break; // aplha cutoff
                }
                return value;
            }

        }

        //TODO : (players' score delta)
        public int HeuristicEvaluation(Data node, bool isWhiteTurn)
        {
            return node.getScoreHeuristic(isWhiteTurn);
        }

        public int GetWhiteScore()
        {
            return board.getScore(false);
        }

        public int GetBlackScore()
        {
            return board.getScore(true);
        }
    }
}
