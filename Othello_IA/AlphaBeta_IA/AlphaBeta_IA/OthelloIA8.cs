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

        #region public_methods
        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            this.board = new Data(whiteTurn, game);

            Dictionary<String, List<Tuple<int, int>>> possibleMoves = board.getPossibleMoves();
            string key = "00";

            foreach (KeyValuePair<string, List<Tuple<int, int>>> entry in possibleMoves)
            {
                //TODO : Travail correct à faire ici
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
