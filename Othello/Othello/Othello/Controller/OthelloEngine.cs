/* 
    This class contains the logic for the board
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    //-> game state management : data modification -> notifies UI (update)
    public class OthelloEngine : IPlayable
    {
        #region ATTRIBUTES

        private Stack<int[,]> history;
        
        // clé: string représentant un mouvement, exemple : "07"
        // valeur: liste des tuiles capturées si ce mouvement est efefctuées
        private Dictionary<String, List<Tuple<int, int>>> possibleMoves;
        public bool CanMove { get { return possibleMoves.Count > 0; } }

        private BoardState currentPlayer = BoardState.HIDDEN; 
        public BoardState CurrentPlayer { get { return currentPlayer; } }
        private GameData datas;

        #endregion

        #region METHODS


        /* New Game */
        public void StartNewGame()
        {
            //initialize board
            //next turn
        }


        /* Initialization */
        public OthelloEngine(GameData datas, BoardState player)
        {
            this.datas = datas;
            currentPlayer = player;

            // intialisation
            history = new Stack<int[,]>();
            possibleMoves = new Dictionary<String, List<Tuple<int, int>>>();

            // if wee need tiles to know their positions
            for (int i = 0; i < GameData.BOARDSIZE; i++)
            {
                for (int j = 0; j < GameData.BOARDSIZE; j++)
                {
                    datas.StateArray[i, j] = BoardState.HIDDEN;
                }
            }

            datas.StateArray[3, 4] = datas.StateArray[4, 3] = BoardState.PLACED_BLACK;
            datas.StateArray[3, 3] = datas.StateArray[4, 4] = BoardState.PLACED_WHITE;

            ComputePossibleMoves();
        }

        /* Next Turn */
        public void GameStateChange()
        {
            //check if valid moves
        }

        /* Get Next Move : IPlayable */
        
        public Tuple<char, int> getNextMove(int[,] game, int level, bool whiteTurn)
        {
            //ask GameEngine next valid move given a game position
            return new Tuple<char, int>('a', 1);
        }


        /* Get Moves : IPlayable */
        public bool playMove(int column, int line, bool isWhite)
        {
            //update board status if valid move
            if (IsPlayable(column, line))
            {  
                // pose une pièce sur la case jouée
                datas.StateArray[column, line] = currentPlayer;

                foreach (Tuple<int, int> item in possibleMoves[tupleToString(column, line)])
                {
                    datas.StateArray[item.Item1, item.Item2] = currentPlayer;
                    datas.UpdateScore();
                }
                // We don't remove the possible move in the datas, because we will change the player and compute the moves directly after

                ChangePlayer();

                ComputePossibleMoves();

                return true;
            }
            Console.WriteLine($"can't make the move : {column}:{line}");
            return false;
        }

        public void ChangePlayer()
        {
            this.currentPlayer = currentPlayer == BoardState.PLACED_WHITE ? BoardState.PLACED_BLACK : BoardState.PLACED_WHITE;
        }

        public void ComputePossibleMoves()
        {
            possibleMoves.Clear();

            // adds all available tiles
            for (int column = 0; column < GameData.BOARDSIZE; column++)
            {
                for (int line = 0; line < GameData.BOARDSIZE; line++)
                {
                    computeMove(column, line);
                }
            }
        }

        private void computeMove(int column, int line)
        {
            if (datas.StateArray[column, line] != BoardState.HIDDEN)
                return;

            BoardState ennemi;

            if (currentPlayer == BoardState.PLACED_WHITE)
                ennemi = BoardState.PLACED_BLACK;
            else
                ennemi = BoardState.PLACED_WHITE;

            List<Tuple<int, int>> neighborhood = new List<Tuple<int, int>>();

            for (int i = column - 1; i <= column + 1; i++)
            {
                for (int j = line - 1; j <= line + 1; j++)
                {
                    try
                    {
                        if (datas.StateArray[i, j] == ennemi)
                            neighborhood.Add(new Tuple<int, int>(i, j));
                    }
                    catch (Exception)
                    {
                        // si la case n'existe pas on ne fait rien
                    }
                }
            }

            if (neighborhood.Count == 0)
                return;

            List<Tuple<int, int>> catchedTiles = new List<Tuple<int, int>>();

            foreach (Tuple<int, int> neighborn in neighborhood)
            {
                int dx = neighborn.Item1 - column;
                int dy = neighborn.Item2 - line;
                int x = neighborn.Item1;
                int y = neighborn.Item2;
                List<Tuple<int, int>> temp = new List<Tuple<int, int>>();
                try
                {
                    while (datas.StateArray[x, y] == ennemi)
                    {
                        temp.Add(new Tuple<int, int>(x, y));
                        x = x + dx;
                        y = y + dy;
                    }

                    if (datas.StateArray[x, y] == currentPlayer)
                        catchedTiles.AddRange(temp);
                }
                catch (Exception)
                {
                    // if out of the grid
                }
            }
            if (catchedTiles.Count == 0)
                return;

            possibleMoves.Add(tupleToString(column, line), catchedTiles);

            return;
        }

        private String tupleToString(Tuple<int, int> tuple)
        {
            return tupleToString(tuple.Item1, tuple.Item2);
        }

        private String tupleToString(int x, int y)
        {
            return $"{x}{y}";
        }


        /// <summary>
        /// Returns true if the move is valid for specified color
        /// </summary>
        /// <param name="column">value between 0 and 7</param>
        /// <param name="line">value between 0 and 7</param>
        /// <returns></returns>
        public bool IsPlayable(int column, int line)
        {
            return possibleMoves.ContainsKey(tupleToString(column, line));
        }

        /* Scores : IPlayable */
        //black : BlackTimeStr
        public int getBlackScore()
        {
            //number of black discs on board
            return 0;
        }

        //white : WhiteTimeStr
        public int getWhiteScore()
        {
            //number of white discs on board
            return 0;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("  0 1 2 3 4 5 6 7\n");
            for (int y = 0; y < GameData.BOARDSIZE; y++)
            {
                str.Append($"{y} ");
                for (int x = 0; x < GameData.BOARDSIZE; x++)
                {
                    string tile = datas.StateArray[x, y] == BoardState.HIDDEN ? "_" : (datas.StateArray[x, y] == BoardState.PLACED_WHITE ? "w" : "b");
                    if (possibleMoves.ContainsKey(tupleToString(x, y)))
                        tile = ".";
                    str.Append($"{tile} ");
                }
                str.Append("\n");
            }
            return str.ToString();
        }

        public bool isPlayable(int column, int line, bool isWhite)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
