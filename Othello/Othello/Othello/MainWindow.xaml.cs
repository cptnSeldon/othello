using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Othello
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region ATTRIBUTES
        GameData data;
        GameState currentGameState;
        DiscView[,] placedDiscs;
        #endregion

        #region METHODS
        //Main Initialization
        public MainWindow()
        {
            //window init
            InitializeComponent();
            currentGameState = GameState.INIT;
            //panels init
            InitializePanels();
            black_label.DataContext = data;
            white_label.DataContext = data;
            //grid init
            InitializeGrid();
            //decomment following if in debug mode
            InitializeBoard();
            UpdateBoard();
        }

        #region PANELS
        //Panels Initialization
        public void InitializePanels()
        {
            data = new GameData();
            int j = data.TotalBlack;

            for (int i = 0; i < j; i++)
            {
                SideDisc disc = new SideDisc();
                disc.SetState(GameState.BLACK_TURN);
                b_discs.Children.Add(disc);
            }

            for (int i = 0; i < j; i++)
            {
                SideDisc disc = new SideDisc();
                disc.SetState(GameState.WHITE_TURN);
                w_discs.Children.Add(disc);
            }
        }

        //Panels Update
        public void UpdatePanel()
        {
            if (currentGameState == GameState.INIT)
                currentGameState = GameState.BLACK_TURN;

            else if (data.TotalWhite != 0 && currentGameState == GameState.BLACK_TURN)
            {
                data.TotalBlack--;
                w_discs.Children.RemoveAt(data.TotalBlack);

                currentGameState = GameState.WHITE_TURN;
            }

            else if (data.TotalWhite != 0 && currentGameState == GameState.WHITE_TURN)
            {
                data.TotalWhite--;
                b_discs.Children.RemoveAt(data.TotalWhite);

                currentGameState = GameState.BLACK_TURN;
            }
            else if (data.TotalWhite == 0 && data.TotalBlack == 0)
                currentGameState = GameState.GAME_END;
        }
        #endregion
        //TODO : adapt to disc placement
        /*
         in xaml
         <Button x:Name="Update_button" Height="50" Width="50">
            Click="Update_button_Click">
         </Button>
         
         here : 
         private void Update_button_Click(object sender, RoutedEventArgs e)
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
                    
                UpdatePanel();
            }
            else
            {
                data.StopTimer(GameState.BLACK_TURN);
                data.StopTimer(GameState.WHITE_TURN);
            }
        }
             */
        #region GRID
        //Grid Initialization
        private void InitializeGrid()
        {
            //retrieve size information from *.xaml
            GridLengthConverter lengthConverter = new GridLengthConverter();
            GridLength gridSize = (GridLength)lengthConverter.ConvertFromString("Auto");

            //grid initialization
            for (int i = 0; i < 8; i++)
            {
                board_grid.ColumnDefinitions.Add(new ColumnDefinition());
                board_grid.RowDefinitions.Add(new RowDefinition());
            }
        }
        #endregion
        #region BOARD
        //Board Initialization
        private void InitializeBoard()
        {
            data = new GameData();

            //initialize all to hidden state
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    data.StateArray[row, column] = BoardState.HIDDEN;
                }
            }

            //place the four start discs in the middle of the board
            data.StateArray[3, 3] = BoardState.PLACED_BLACK;
            data.StateArray[4, 4] = BoardState.PLACED_BLACK;
            data.StateArray[3, 4] = BoardState.PLACED_WHITE;
            data.StateArray[4, 3] = BoardState.PLACED_WHITE;
        }

        //Board Update
        private void UpdateBoard()
        {
            //remove the following if not in debug mode
            data = new GameData();

            //create discs' array
            placedDiscs = new DiscView[8, 8];

            //populate array + UI
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    
                    placedDiscs[row, column] = new DiscView();
                    
                    Grid.SetColumn(placedDiscs[row, column], column);
                    Grid.SetRow(placedDiscs[row, column], row);

                    //placed BLACK
                    if (data.StateArray[row, column] == BoardState.PLACED_BLACK)
                    {
                        placedDiscs[row, column].SetState(BoardState.PLACED_BLACK);
                    }
                    //playable BLACK
                    if (data.StateArray[row, column] == BoardState.PLAYABLE_BLACK)
                    {
                        placedDiscs[row, column].SetState(BoardState.PLAYABLE_BLACK);
                    }

                    //placed WHITE
                    if (data.StateArray[row, column] == BoardState.PLACED_WHITE)
                    {
                        placedDiscs[row, column].SetState(BoardState.PLACED_WHITE);
                    }
                    //playable WHITE
                    if (data.StateArray[row, column] == BoardState.PLAYABLE_WHITE)
                    {
                        placedDiscs[row, column].SetState(BoardState.PLAYABLE_WHITE);
                    }

                    //EMPTY
                    else if (data.StateArray[row, column] == BoardState.HIDDEN)
                    {
                        placedDiscs[row, column].SetState(BoardState.HIDDEN);
                    }

                    //populate Board (visual)
                    board_grid.Children.Add(placedDiscs[row, column]);
                }
            }
        }
        #endregion
        #endregion
    }
}
