﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Othello
{
    #region BOARD & GAME STATE
    public enum GameState { INIT, BLACK_TURN, WHITE_TURN, GAME_END };   //game main loop
    [Serializable]
    public enum BoardState {
        PLAYABLE_BLACK=0,
        PLAYABLE_WHITE=1,
        HIDDEN=2,
        PLACED_BLACK=3,
        PLACED_WHITE=4 };  // the grid is populated with states
    #endregion

    public class GameData : INotifyPropertyChanged //each new event notifies data to be changed according to the engine
    {

        #region CONSTANTS
        public const int BOARDSIZE = 8;
        #endregion

        #region ATTRIBUTES
        //BOARD
        private BoardState[,] stateArray;
        public BoardState[,] StateArray {
            get { return stateArray; }
            set { stateArray = value; }
        }

        //SCORE
        private string blackScoreStr;
        private string whiteScoreStr;

        public string BlackScoreStr
        {
            get { return blackScoreStr; }
            set { blackScoreStr = value; OnPropertyChanged("BlackScoreStr"); }
        }

        public string WhiteScoreStr {
            get { return whiteScoreStr; }
            set { whiteScoreStr = value; OnPropertyChanged("WhiteScoreStr"); }
        }
        //PANEL : remaining discs
        public int TotalBlack { get; set; }
        public int TotalWhite { get; set; }
        //TIMERS
        public DispatcherTimer blackTimer;
        public string BlackTimerStr { get; set; }
        private int blackElapsedTime = 0;
        public int BlackElapsedTime
        {
            get { return blackElapsedTime; }
            set { blackElapsedTime = value; }
        }

        public DispatcherTimer whiteTimer;
        public string WhiteTimerStr { get; set; }
        private int whiteElapsedTime = 0;
        public int WhiteElapsedTime
        {
            get { return whiteElapsedTime; }
            set { whiteElapsedTime = value; }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region METHODS
        //Initialization
        public GameData()
        {
            //board initialization
            StateArray = new BoardState[BOARDSIZE, BOARDSIZE];

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
            {
                blackTimer.Start();
                BlackTimerUpdate();
            }
            else if (currentState == GameState.WHITE_TURN)
            {
                whiteTimer.Start();
                WhiteTimerUpdate();
            }
        }

        //Stop Timer
        public void StopTimer(GameState currentState)
        {
            if (currentState == GameState.BLACK_TURN)
            {
                blackTimer.Stop();
                BlackTimerUpdate();
            }
            else if (currentState == GameState.WHITE_TURN)
            {
                whiteTimer.Stop();
                WhiteTimerUpdate();
            }
        }

        //Tick management
        void BlackTimerTick(object sender, EventArgs e)
        {
            ++blackElapsedTime;
            BlackTimerUpdate();
        }
       private  void BlackTimerUpdate()
        {
            BlackTimerStr = TimeSpan.FromSeconds(blackElapsedTime).ToString();
            OnPropertyChanged("BlackTimerStr");
        }

        void WhiteTimerTick(object sender, EventArgs e)
        {
            whiteElapsedTime++;
            WhiteTimerUpdate();
        }
        void WhiteTimerUpdate()
        {
            WhiteTimerStr = TimeSpan.FromSeconds(whiteElapsedTime).ToString();
            OnPropertyChanged("WhiteTimerStr");
        }
        #endregion
        //Notifcation
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
