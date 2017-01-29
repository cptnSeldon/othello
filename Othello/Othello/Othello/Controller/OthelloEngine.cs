/* 
    This class contains the logic for the board
*/


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;


namespace Othello
{
    //-> game state management : data modification -> notifies UI (update)
    public class OthelloEngine : IPlayable
    {
        #region ATTRIBUTES
        //data
        GameState currentGameState;
        BoardState currentPlayer;
        public GameData data { private set; get; }
        int oldTotalPossibleMoves;
        int nextPossibleMoves;
        bool hasSkipped;
        
        private Stack<int[,]> history;
        // clé: string représentant un mouvement, exemple : "07"
        // valeur: liste des tuiles capturées si ce mouvement est efefctuées
        private Dictionary<String, List<Tuple<int, int>>> possibleMoves;
        public bool CanMove { get { return possibleMoves.Count > 0; } }
        #endregion

        #region METHODS
        #region GAME LOOP
        /* 0. Initialization */
        public OthelloEngine()
        {
            StartNewGame();
        }

        /* 0.1 START NEW GAME */
        public void StartNewGame()
        {
            /** DATA INITIALIZATION */
            data = new GameData();
            hasSkipped = false;
            currentGameState = GameState.INIT;
            currentPlayer = BoardState.PLACED_BLACK;
            TimerManager();

            history = new Stack<int[,]>();
            possibleMoves = new Dictionary<String, List<Tuple<int, int>>>();
            oldTotalPossibleMoves = possibleMoves.Count;
            nextPossibleMoves = -1;
            /** BOARD INITIALIZATION */
            //initialize all to hidden state
            for (int row = 0; row < GameData.BOARDSIZE; row++)
            {
                for (int column = 0; column < GameData.BOARDSIZE; column++)
                {
                    data.StateArray[row, column] = BoardState.HIDDEN;
                }
            }

            //place the four start discs in the middle of the board
            data.StateArray[3, 3] = BoardState.PLACED_BLACK;
            data.StateArray[4, 4] = BoardState.PLACED_BLACK;
            data.StateArray[3, 4] = BoardState.PLACED_WHITE;
            data.StateArray[4, 3] = BoardState.PLACED_WHITE;

            /** GET POSSIBLE MOVES */
            ComputePossibleMoves();
        }

        /* 1.1 CHECK POSSIBLE MOVES */
        public void ComputePossibleMoves()
        {
            //possible move history step 1
            oldTotalPossibleMoves = nextPossibleMoves;
            //clear old moves list
            possibleMoves.Clear();

            for (int column = 0; column < GameData.BOARDSIZE; column++)
                for (int line = 0; line < GameData.BOARDSIZE; line++)
                    if (data.StateArray[column, line] == BoardState.PLAYABLE_WHITE || data.StateArray[column, line] == BoardState.PLAYABLE_BLACK)
                        data.StateArray[column, line] = BoardState.HIDDEN;

            // adds all available tiles
            for (int column = 0; column < GameData.BOARDSIZE; column++)
            {
                for (int line = 0; line < GameData.BOARDSIZE; line++)
                {
                    //checks move's validity
                    computeMove(column, line);
                }
            }
            //possible move history step 2
            nextPossibleMoves = possibleMoves.Count;
            Console.WriteLine("Moves : \nold : {0}\nnew : {1}", oldTotalPossibleMoves, nextPossibleMoves);
        }

        /* 1.2 CHECK MOVE'S VALIDITY */
        private void computeMove(int column, int line)
        {
            //check if tile is already taken
            if (data.StateArray[column, line] != BoardState.HIDDEN)
                return;

            //check Player's color
            BoardState myColor;
            BoardState opponentColor;

            if (currentPlayer == BoardState.PLACED_WHITE)
            {
                opponentColor = BoardState.PLACED_BLACK;
                myColor = BoardState.PLACED_WHITE;
            }
            else
            {
                opponentColor = BoardState.PLACED_WHITE;
                myColor = BoardState.PLACED_BLACK;
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
                    if (i < GameData.BOARDSIZE &&
                        i > 0 &&
                        j < GameData.BOARDSIZE &&
                        j > 0) { 
                        if (data.StateArray[i, j] == opponentColor)
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

                while ((x + dx < GameData.BOARDSIZE &&
                        x + dx > -1 &&
                        y + dy < GameData.BOARDSIZE &&
                        y + dy > -1) &&
                        data.StateArray[x, y] == opponentColor)
                {
                    temp.Add(new Tuple<int, int>(x, y));
                    x = x + dx;
                    y = y + dy;
                }

                if (data.StateArray[x, y] == myColor)
                    catchedTiles.AddRange(temp);

            }
            if (catchedTiles.Count == 0)
                return;

            possibleMoves.Add(tupleToString(column, line), catchedTiles);
            
            data.StateArray[column, line] = currentPlayer == BoardState.PLACED_WHITE ? BoardState.PLAYABLE_WHITE : BoardState.PLAYABLE_BLACK;

            return;
        }

        /* 2. DISC'S PLACEMENT ON THE BOARD : binded to click */
        public bool playMove(int column, int line, bool isWhite)
        {
            hasSkipped = false;
            //update board status if valid move
            if (IsPlayable(column, line))
            {
                //place disc on the board
                data.StateArray[column, line] = currentPlayer;

                foreach (Tuple<int, int> move in possibleMoves[tupleToString(column, line)])
                {
                    //update board
                    data.StateArray[move.Item1, move.Item2] = currentPlayer;
                }

                if(currentPlayer == BoardState.PLACED_BLACK)
                    data.TotalBlack--;
                else
                    data.TotalWhite--;

                UpdateScore();
                // We don't remove the possible move in the datas, because we will change the player and compute the moves directly after
                GameStateChange();
                //go to 1.
                ComputePossibleMoves();

                //Console test
                Console.WriteLine("Black Timer : {0}", data.BlackTimerStr);
                Console.WriteLine("White Timer : {0}", data.WhiteTimerStr);
                Console.WriteLine("Black Score : {0}", data.BlackScoreStr);
                Console.WriteLine("White Score : {0}", data.WhiteScoreStr);

                // If there's no playable move for the player, we switch again
                if (possibleMoves.Count == 0)
                {
                    hasSkipped = true;
                    GameStateChange();
                    ComputePossibleMoves();
                    Console.WriteLine("Pas de coups possibles, deux fois le même joueur");
                }

                if (nextPossibleMoves == 0 && oldTotalPossibleMoves == 0)
                {
                    currentGameState = GameState.GAME_END;
                    Console.WriteLine("Fin du jeu");
                    TimerManager();
                }

                return true;
            }
            Console.WriteLine($"can't make the move : {column}:{line}");
            return false;
        }

        /* 3.1 PLAYER CHANGE */
        public void ChangePlayer()
        {
            this.currentPlayer = currentPlayer == BoardState.PLACED_WHITE ? BoardState.PLACED_BLACK : BoardState.PLACED_WHITE;
        }

        /* 3.2 TIMER MANAGEMENT */
        public void TimerManager()
        {                
            if (currentGameState != GameState.GAME_END)
            {
                if (currentPlayer == BoardState.PLACED_WHITE)
                {
                    data.StopTimer(GameState.BLACK_TURN);
                    data.StartTimer(GameState.WHITE_TURN);
                }

                else if (currentPlayer == BoardState.PLACED_BLACK)
                {
                    data.StopTimer(GameState.WHITE_TURN);
                    data.StartTimer(GameState.BLACK_TURN);
                }
            }
            if (currentGameState == GameState.GAME_END)
            {
                data.StopTimer(GameState.BLACK_TURN);
                data.StopTimer(GameState.WHITE_TURN);
            }
        }

        /* 3.2 GAME STATE CHANGE */
        public void GameStateChange()
        {

            Console.WriteLine("Current game state {0}", currentGameState);
            Console.WriteLine("Total : {0} ", data.TotalBlack + data.TotalWhite);
            ChangePlayer();
            TimerManager();

            //INIT -> BLACK TURN
            if (currentGameState == GameState.INIT)
            {
                currentGameState = GameState.BLACK_TURN;
            }
            //BLACK TURN -> WHITE TURN
            else if (data.TotalBlack != 0 
                && currentGameState == GameState.BLACK_TURN
                || nextPossibleMoves == 0 && oldTotalPossibleMoves != 0
                && currentGameState == GameState.BLACK_TURN)
            {
                currentGameState = GameState.WHITE_TURN;
            }
            //WHITE TURN -> BLACK TURN
            else if (data.TotalWhite != 0 
                && currentGameState == GameState.WHITE_TURN
                || nextPossibleMoves == 0 && oldTotalPossibleMoves != 0
                && currentGameState == GameState.WHITE_TURN)
            {
                currentGameState = GameState.BLACK_TURN;
            }

            TimerManager();

        }
        
        /* GET GAME STATE */
        public BoardState[,] getGameState()
        {
            return data.StateArray;
        }


        #endregion

        public BoardState GetCurrentPlayer()
        {
            return currentPlayer;
        }

        public GameState GetCurrentGameState()
        {
            return currentGameState;
        }

        public bool HasSkipped()
        {
            return hasSkipped;
        }

        #region SCORE & SIDE DISCS MANAGEMENT
        /* UPDATE SCORE */
        public void UpdateScore()
        {
            data.WhiteScoreStr = data.BlackScoreStr = "0";
            int whiteScore = 0;
            int blackScore = 0;

            for (int y = 0; y < GameData.BOARDSIZE; y++)
            {
                for (int x = 0; x < GameData.BOARDSIZE; x++)
                {
                    if (data.StateArray[x, y] != BoardState.HIDDEN)
                    {
                        if (data.StateArray[x, y] == BoardState.PLACED_WHITE)
                            whiteScore++;
                        else
                        if (data.StateArray[x, y] == BoardState.PLACED_BLACK)
                            blackScore++;
                    }
                }
            }

            data.BlackScoreStr = blackScore.ToString();
            data.WhiteScoreStr = whiteScore.ToString();
        }

        /* GET SCORE */
        public int getWhiteScore() { return Convert.ToInt32(data.WhiteScoreStr); }
        public int getBlackScore() { return Convert.ToInt32(data.BlackScoreStr); }

        /* REMAINING BLACK MOVES PANEL */
        public int getTotalBlack() { return data.TotalBlack; }

        /* REMAINING WHITE MOVES PANEL */
        public int getTotalWhite() { return data.TotalWhite; }
        #endregion

        #region MENU
        /* SAVE TO JSON FILE */
        /* Uses the Json.NET packet. It has been chosen over Serialization because we can't 
           serialize a 2d array of enum.
        */
        public void Save(string filePath)
        {
            JObject playersObject = new JObject();

            playersObject.Add("scoreWhite", data.WhiteScoreStr);
            playersObject.Add("scoreBlack", data.BlackScoreStr);

            playersObject.Add("timerBlack", data.BlackElapsedTime);
            playersObject.Add("timerWhite", data.WhiteElapsedTime);

            playersObject.Add("totalWhite", data.TotalWhite);
            playersObject.Add("totalBlack", data.TotalBlack);

            JArray boardObject = new JArray();

            for (int column = 0; column < GameData.BOARDSIZE; column++)
            {
                for (int line = 0; line < GameData.BOARDSIZE; line++)
                {
                    JObject entry = new JObject();
                    entry.Add("X", line);
                    entry.Add("Y", column);
                    entry.Add("Value", data.StateArray[column, line].ToString());
                    boardObject.Add(entry);
                }
            }

            JObject gameObject = new JObject();
            gameObject.Add("Players", playersObject);
            gameObject.Add("Board", boardObject);
            gameObject.Add("isWhiteTurn", currentPlayer == BoardState.PLACED_BLACK ? "black" : "white");

            File.WriteAllText(filePath, gameObject.ToString());
        }
        /* LOAD FROM JSON FILE */
        public void Load(string filePath)
        {
            JObject gameObject = JObject.Parse(File.ReadAllText(filePath));

            var playersObject = (JObject)gameObject["Players"];

            data.WhiteScoreStr = (string)playersObject["scoreWhite"];
            data.BlackScoreStr = (string)playersObject["scoreBlack"];

            data.WhiteElapsedTime = (int)playersObject["timerWhite"];
            data.BlackElapsedTime = (int)playersObject["timerBlack"];

            data.TotalBlack = (int)playersObject["totalBlack"];
            data.TotalWhite = (int)playersObject["totalWhite"];

            string playerTurn = (string)gameObject["isWhiteTurn"];
            currentPlayer = playerTurn == "white" ? BoardState.PLACED_WHITE : BoardState.PLACED_BLACK;
            TimerManager();
            foreach (var tile in gameObject["Board"])
            {
                int line = (int)tile["X"];
                int column = (int)tile["Y"];
                string value = (string)tile["Value"];
                BoardState state = (BoardState)Enum.Parse(typeof(BoardState), value, true);

                data.StateArray[column, line] = state;
            }
            ComputePossibleMoves();
        }
        
        #endregion

        #region IA
        /* Get Next Move : IPlayable */
        public Tuple<char, int> getNextMove(int[,] game, int level, bool whiteTurn)
        {
            //ask GameEngine next valid move given a game position
            return new Tuple<char, int>('a', 1);
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

        /// <summary>
        /// Not used yet. Will be to implemented for the alphabeta IA.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="line"></param>
        /// <param name="isWhite"></param>
        /// <returns></returns>
        public bool isPlayable(int column, int line, bool isWhite)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region STRING
        /// <summary>
        /// Convert a tuple column, line to a String concatenation.
        /// This is used for the Dictionnary key. It has intialy been design to use 
        /// an array as the key, but an array isn't hashable.
        /// Tests with a Point key hasn't been successful.
        /// </summary>
        /// <param name="tuple"></param>
        /// <returns></returns>
        private String tupleToString(Tuple<int, int> tuple)
        {
            return tupleToString(tuple.Item1, tuple.Item2);
        }

        private String tupleToString(int x, int y)
        {
            return $"{x}{y}";
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
                    string tile = data.StateArray[x, y] == BoardState.HIDDEN ? "_" : (data.StateArray[x, y] == BoardState.PLACED_WHITE ? "w" : "b");
                    if (possibleMoves.ContainsKey(tupleToString(x, y)))
                        tile = ".";
                    str.Append($"{tile} ");
                }
                str.Append("\n");
            }
            return str.ToString();
        }
        #endregion
        #endregion
    }
}
