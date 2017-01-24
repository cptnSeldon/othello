﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;
using System.Windows.Input;



namespace Othello
{
    #region BOARD & GAME STATE
    public enum GameState { INIT, BLACK_TURN, WHITE_TURN, GAME_END };   //game main loop
    public enum BoardState { PLAYABLE_BLACK, PLAYABLE_WHITE, HIDDEN, PLACED_BLACK, PLACED_WHITE };  // the grid is populated with states
    #endregion
    
    class GameData : IPlayable, INotifyPropertyChanged //each new event notifies data to be changed according to the engine
    {

        #region ATTRIBUTES
        //BOARD
        public BoardState[,] StateArray { get; set; }
        //SCORE
        public string BlackScoreStr { get; private set; }
        public string WhiteScoreStr { get; private set; }
        //PANEL : remaining discs
        public int TotalBlack { get; set; }
        public int TotalWhite { get; set; }
        //TIMERS
        public DispatcherTimer blackTimer;
        public string BlackTimerStr { get; private set; }
        private int blackElapsedTime = 0;

        public DispatcherTimer whiteTimer;
        public string WhiteTimerStr { get; private set; }
        private int whiteElapsedTime = 0;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region METHODS
        //Initialization
        public GameData()
        {
            //board initialization
            StateArray = new BoardState[8, 8];
            FillArray();

            //score initialization
            BlackScoreStr = "2";
            WhiteScoreStr = "2";

            //remaining discs initialization
            TotalBlack = 30;
            TotalWhite = 30;

            //timers initialization
            InitializeTimer();
            BlackTimerStr = "00:00:00";
            WhiteTimerStr = "00:00:00";

        }

        #region TIMER METHODS
        //Timer Init
        public void InitializeTimer()
        {
            //black
            blackTimer = new DispatcherTimer();
            blackTimer.Interval = TimeSpan.FromSeconds(1);
            blackTimer.Tick += BlackTimerTick;
            //white
            whiteTimer = new DispatcherTimer();
            whiteTimer.Interval = TimeSpan.FromSeconds(1);
            whiteTimer.Tick += WhiteTimerTick;
        }

        //Start Timer
        public void StartTimer(GameState currentState)
        {
            if (currentState == GameState.BLACK_TURN)
                blackTimer.Start();
            else if (currentState == GameState.WHITE_TURN)
                whiteTimer.Start();
        }

        //Stop Timer
        public void StopTimer(GameState currentState)
        {
            if (currentState == GameState.BLACK_TURN)
                blackTimer.Stop();
            else if (currentState == GameState.WHITE_TURN)
                whiteTimer.Stop();
        }

        //Tick management
        void BlackTimerTick(object sender, EventArgs e)
        {
            BlackTimerStr = TimeSpan.FromSeconds(++blackElapsedTime).ToString();
            OnPropertyChanged("BlackTimerStr");
        }

        void WhiteTimerTick(object sender, EventArgs e)
        {
            WhiteTimerStr = TimeSpan.FromSeconds(++whiteElapsedTime).ToString();
            OnPropertyChanged("WhiteTimerStr");
        }
        #endregion
        //Notifcation
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #region DEBUG
        //Fill array to random
        private void FillArray()
        {

            Random random = new Random();

            // random filling
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    // <=a < b
                    int state = random.Next(1, 6);

                    if (state == 1)
                        StateArray[row, column] = BoardState.PLACED_BLACK;
                    if (state == 2)
                        StateArray[row, column] = BoardState.PLACED_WHITE;
                    if (state == 3)
                        StateArray[row, column] = BoardState.HIDDEN;
                    if (state == 4)
                        StateArray[row, column] = BoardState.PLAYABLE_WHITE;
                    if (state == 5)
                        StateArray[row, column] = BoardState.PLAYABLE_BLACK;
                }
            }
        }
        #endregion

        #region IPLAYABLE
        public bool isPlayable(int column, int line, bool isWhite)
        {
            return true;
        }

        public bool playMove(int column, int line, bool isWhite)
        {
            return true;
        }

        public Tuple<char, int> getNextMove(int[,] game, int level, bool whiteTurn)
        {
            return new Tuple<char, int>('a', 1);
        }

        public int getWhiteScore()
        {
            return 0;
        }

        public int getBlackScore()
        {
            return 0;
        }
        #endregion
        #endregion
    }
}