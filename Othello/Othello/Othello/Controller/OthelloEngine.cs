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
        int currentPossibleMoves;
        
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
            currentGameState = GameState.INIT;
            currentPlayer = BoardState.PLACED_BLACK;

            history = new Stack<int[,]>();
            possibleMoves = new Dictionary<String, List<Tuple<int, int>>>();
            oldTotalPossibleMoves = -1;
            currentPossibleMoves = -1;
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
            oldTotalPossibleMoves = possibleMoves.Count;
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
            //OLD POSSIBLE MOVES
            currentPossibleMoves = possibleMoves.Count;
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
                    try
                    {
                        if (data.StateArray[i, j] == opponentColor)
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

            //TEST 2
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
                    while (data.StateArray[x, y] == opponentColor)
                    {
                        temp.Add(new Tuple<int, int>(x, y));
                        x = x + dx;
                        y = y + dy;
                    }

                    if (data.StateArray[x, y] == myColor)
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
            
            data.StateArray[column, line] = currentPlayer == BoardState.PLACED_WHITE ? BoardState.PLAYABLE_WHITE : BoardState.PLAYABLE_BLACK;

            return;
        }

        /* 2. DISC'S PLACEMENT ON THE BOARD : binded to click */
        public bool playMove(int column, int line, bool isWhite)
        {
            GameStateChange();
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
                UpdateScore();
                // We don't remove the possible move in the datas, because we will change the player and compute the moves directly after

                //go to 1.
                ComputePossibleMoves();

                //store possible moves
                currentPossibleMoves = possibleMoves.Count;

                //Console test
                Console.WriteLine("Black Timer : {0}", data.BlackTimerStr);
                Console.WriteLine("White Timer : {0}", data.WhiteTimerStr);
                Console.WriteLine("Black Score : {0}", data.BlackScoreStr);
                Console.WriteLine("White Score : {0}", data.WhiteScoreStr);

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
                if (currentGameState == GameState.WHITE_TURN)
                {
                    data.StopTimer(GameState.BLACK_TURN);
                    data.StartTimer(GameState.WHITE_TURN);
                }

                else if (currentGameState == GameState.BLACK_TURN)
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
            ChangePlayer();

            //INIT -> BLACK TURN
            if (currentGameState == GameState.INIT)
            {
                currentGameState = GameState.BLACK_TURN;
            }
            //BLACK TURN -> WHITE TURN
            else if (data.TotalWhite != 0 
                && currentGameState == GameState.BLACK_TURN
                || oldTotalPossibleMoves == 0)
            {
                data.TotalBlack--;
                currentGameState = GameState.WHITE_TURN;
            }
            //WHITE TURN -> BLACK TURN
            else if (data.TotalWhite != 0 
                && currentGameState == GameState.WHITE_TURN
                || oldTotalPossibleMoves == 0)
            {
                data.TotalWhite--;
                currentGameState = GameState.BLACK_TURN;
            }
            //GAME END
            else if ((data.TotalWhite + data.TotalBlack) == 0
                || (oldTotalPossibleMoves + currentPossibleMoves == 0))
            {
                currentGameState = GameState.GAME_END;
            }

            Console.WriteLine("Old : {0}\nNew : {1}", oldTotalPossibleMoves, currentPossibleMoves);
            Console.WriteLine("Total black : {0}\nTotal white : {1}", data.TotalBlack, data.TotalWhite);
            Console.WriteLine("Current game state {0}", currentGameState);
            TimerManager();
        }
        
        /* GET GAME STATE */
        public BoardState[,] getGameState()
        {
            return data.StateArray;
        }


        #endregion

        #region SCORE & SIDE DISCS MANAGEMENT
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

        /* REMAINING BLACK MOVES PANEL */
        public int getTotalBlack() { return data.TotalBlack; }

        /* REMAINING WHITE MOVES PANEL */
        public int getTotalWhite() { return data.TotalWhite; }
        #endregion

        #region MENU
        public void Save(string filePath)
        {
            WriteToXmlFile<GameData>(filePath, data);
        }
        public void Load(string filePath)
        {
            data = ReadFromXmlFile<GameData>(filePath);
        }
        /* SAVE TO XML FILE */
        public void WriteToXmlFile<GameData>(string filePath, GameData objectToWrite, bool append = false) where GameData : new()
        {
            System.IO.TextWriter writer = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(GameData));
                using (writer = new StreamWriter(filePath, append))
                {
                    serializer.Serialize(writer, objectToWrite);
                }
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
        /* READ FROM XML FILE */
        public GameData ReadFromXmlFile<GameData>(string filePath) where GameData : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(GameData));
                using (reader = new StreamReader(filePath))
                {
                    return (GameData)serializer.Deserialize(reader);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
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

        public bool isPlayable(int column, int line, bool isWhite)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region STRING
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
