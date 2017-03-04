﻿using System;
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
            int consecutivePass = 0;

            bool stop = false;
            while (!stop)
            {
                if (isWhite)
                    Console.WriteLine("O");
                else
                    Console.WriteLine("X");

                Tuple<int, int> move = IA.GetNextMove(initialState.getBoard(), 5, isWhite);
                Console.WriteLine(move.ToString());

                IA.PlayMove(move.Item1, move.Item2, isWhite);

                Console.WriteLine(initialState);

                isWhite = !isWhite;

                if (move.Equals(new Tuple<int, int>(-1, -1)))
                    consecutivePass += 1;
                else
                    consecutivePass = 0;

                stop = ((move.Equals(new Tuple<int, int>(-1, -1)) && consecutivePass == 2) || isFinished(initialState));
            }

            Console.WriteLine("Partie terminée");
            Console.WriteLine("O " + initialState.getScore(true));
            Console.WriteLine("X " + initialState.getScore(false));
            Console.ReadKey();

        }

        private static bool isFinished(Data board)
        {
            int i = 0;
            int j = 0;
            bool emptyCase = false;
            while (i < 8 && !emptyCase)
            {
                while (j < 8 && !emptyCase)
                {
                    emptyCase = board.getBoard()[i, j] == -1;
                    j++;
                }
                j = 0;
                i++;
            }

            return !emptyCase;
        }
    }
}