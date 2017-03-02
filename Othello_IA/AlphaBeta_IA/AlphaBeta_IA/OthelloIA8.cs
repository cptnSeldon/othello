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
namespace OthelloIA8
{
    public class AlphaBetaBoard : IPlayable.IPlayable
    {
        #region attributes
       
        private Data board;

        #endregion

        #region Constructors
        public AlphaBetaBoard()
        {
            board = new Data(false);
        }
        #endregion

        #region AlphaBot

        //TODO (players' score delta)
        public int HeuristicEvaluation(Data node)
        {
            return 1;
        }

        //TODO
        public List<Data> GetChildNodes(Data node)
        {
            return new List<Data>();
        }

        //TODO
        public bool IsTerminated(Data node)
        {
            return false;
        }

        /** 
         * -> must have : next moves list -> retrieve data if move is played -> NODES
         * -> 
         *  */
        public int Alphabot(Data node, int depth, int alpha, int beta, bool isPlayerToMaximize)
        {
            //END CONDITION -> tree root or game end
            if (depth == 0 || IsTerminated(node))
                return HeuristicEvaluation(node);

            //IF MAXIMIZING PLAYER == TRUE : nodes -> next board state right after my move
            if (isPlayerToMaximize)
            {
                int value = -100000;
                foreach (Data child in GetChildNodes(node))
                {
                    value = Math.Max(value, Alphabot(child, depth - 1, alpha, beta, false));
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
                foreach (Data child in GetChildNodes(node))
                {
                    value = Math.Min(value, Alphabot(child, depth - 1, alpha, beta, false));
                    alpha = Math.Min(alpha, value);
                    if (beta <= alpha)
                        break; // aplha cutoff
                }
                return value;
            }

        }

        #endregion

        #region public_methods
        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            this.board = new Data(whiteTurn, game);

            Dictionary<String, List<Tuple<int, int>>> possibleMoves = board.getPossibleMoves();
            string key = "00";

            foreach (KeyValuePair<string, List<Tuple<int, int>>> entry in possibleMoves)
            {
                //TODO : Alphabot
                Console.WriteLine(entry.Key);
                key = entry.Key;
            }

            return board.stringToTuple(key);
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            return board.isPlayable(column, line, isWhite);
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            return board.playMove(column, line, isWhite);
        }

        public int[,] GetBoard()
        {
            return this.board.getBoard();
        }

        public string GetName()
        {
            return "8: Nemeth_Roy";
        }
        #endregion


        #region private methods


        #endregion

        #region score
        public int GetBlackScore()
        {
            return 32;
        }

        public int GetWhiteScore()
        {
            return 32;
        }
        #endregion
    }
}
