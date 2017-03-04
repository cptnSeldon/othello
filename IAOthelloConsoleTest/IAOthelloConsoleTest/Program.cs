using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAOthelloConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isWhite = false;
            OthelloBoard IA = new OthelloBoard();
            Data initialState = new Data(isWhite);

            bool stop = false;
            while (!stop)
            {
                Tuple<int, int> move = IA.GetNextMove(initialState.getBoard(), 5, isWhite);
                Console.WriteLine(move.ToString());

                IA.PlayMove(move.Item1, move.Item2, isWhite);

                Console.WriteLine(initialState);

                isWhite = !isWhite;

                stop = move != new Tuple<int, int>(-1, -1);
            }

            Console.WriteLine("Je suis coincé");
            Console.ReadKey();

        }
    }
}
