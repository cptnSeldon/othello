#region ClassData
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;


/// <summary>
/// Class of state data which contains a board grid, the possible moves and the 
/// tiles to move for each possible move
/// </summary>
public class Data
{
    public enum EtatCase
    {
        Empty = -1,
        White = 0,
        Black = 1
    }

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


    private const int BOARDSIZE = 8;

    private int[,] board;
    private Dictionary<string, List<Tuple<int, int>>> possibleMoves;
    private bool isWhite;
    private Tuple<int, int> lastPlayedMove;

    public Data(bool isWhite)
    {
        this.board = initializeBoard(board);
        this.possibleMoves = new Dictionary<string, List<Tuple<int, int>>>();
        this.isWhite = isWhite;
        lastPlayedMove = new Tuple<int, int>(-1, -1);
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

    // TODO : Peaufiner ce code, c'est important de tester la bonne recopie du tableau (passage par valeur de référence)
    // Et aussi de savoir si il faut changer la couleur du player directement, mais il semble plus logique de la changer dans getNextMove
    public Data(Data boardRef)
    {
        this.isWhite = boardRef.isWhite;
        this.board = new int[BOARDSIZE, BOARDSIZE];

        int[,] originalBoard = boardRef.getBoard();

        for (int i = 0; i < BOARDSIZE; i++)
            for (int j = 0; j < BOARDSIZE; j++)
            {
                this.board[i, j] = originalBoard[i, j];
            }
        this.possibleMoves = new Dictionary<string, List<Tuple<int, int>>>();
        this.lastPlayedMove = boardRef.lastPlayedMove;
        computeMoves(this.isWhite);
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

            this.lastPlayedMove = new Tuple<int, int>(column, line);
            this.isWhite = !this.isWhite;
            return true;
        }
        else
        {
            this.lastPlayedMove = new Tuple<int, int>(-1, -1);
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
        computeMoves(isWhite);
        return this.possibleMoves;
    }

    public int getScoreHeuristic(bool isWhite)
    {
        int player = isWhite ? 0 : 1;
        int[] scores = { 0, 0 };
        for (int y = 0; y < BOARDSIZE; y++)
        {
            for (int x = 0; x < BOARDSIZE; x++)
            {
                if (board[x, y] != -1)
                    scores[board[x, y]] += WEIGHT_MATRIX[x, y];
            }
        }

        scores[player] = scores[player] - scores[1 - player];
        return scores[player];
    }
    
    public int getScore(bool isWhite)
    {
        int score = 0;

        int colorToFound = isWhite ? 0 : 1;

        for (int i = 0; i < BOARDSIZE; i++)
            for (int j = 0; j < BOARDSIZE; j++)
            {
                if (board[i, j] == colorToFound)
                    score += 1;
            }

        return score;
    }

    public List<Data> GetChildNodes()
    {
        Dictionary<string, List<Tuple<int, int>>> possibleMoves = getPossibleMoves();
        List<Data> childList = new List<Data>();

        string key = "00";

        Data currentBoard = new Data(this);

        foreach (KeyValuePair<string, List<Tuple<int, int>>> entry in possibleMoves)
        {
            key = entry.Key;

            currentBoard = new Data(this);
            Tuple<int, int> coords = stringToTuple(key);
            if (currentBoard.playMove(coords.Item1, coords.Item2, currentBoard.isWhite))
            {
                Data child = new Data(currentBoard);
                childList.Add(child);
            }
        }
        return childList;
    }

    public Tuple<int, int> LastPlayedMove()
    {
        return this.lastPlayedMove;
    }
}

#endregion