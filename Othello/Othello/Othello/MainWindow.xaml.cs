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
        // GameData data;
        IPlayable engine;
        DiscView[,] placedDiscs;
        #endregion

        #region METHODS
        //Main Initialization
        public MainWindow()
        {
            engine = new OthelloEngine();
            //window init
            InitializeComponent();
            //panels init
            InitializePanels();
            black_label.DataContext = engine;
            white_label.DataContext = engine;
            //grid init
            InitializeGrid();
            InitializeBoard();
            //update board
            UpdateBoard(engine.getGameState());
        }
        
        /* REMAINING DISCS' PANEL INITIALIZATION */
        public void InitializePanels()
        {
            int j = engine.getTotalBlack();

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
               
        /* GRID INITIALIZATION */
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
        
        /* BOARD INITIALIZATION */
        private void InitializeBoard()
        {
            //create discs' array
            placedDiscs = new DiscView[8, 8];

            //populate array + UI
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    placedDiscs[row, column] = new DiscView();
                    placedDiscs[row, column].setMouseDown(DiscView_MouseDown);
                    
                    Grid.SetColumn(placedDiscs[row, column], column);
                    Grid.SetRow(placedDiscs[row, column], row);
                    
                    //populate Board (visual)
                    board_grid.Children.Add(placedDiscs[row, column]);
                }
            }
        }

        
        private void DiscView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            engine.playMove(Grid.GetRow((DiscView)sender), Grid.GetColumn((DiscView)sender), true);
            UpdateBoard(engine.getGameState());
        }

        /* UPDATE BOARD */
        public void UpdateBoard(BoardState[,] board)
        {
            for (int row = 0; row < GameData.BOARDSIZE; row++)
            {
                for (int column = 0; column < GameData.BOARDSIZE; column++)
                {
                    placedDiscs[row, column].SetState(board[row, column]);
                }
            }
        }
        #endregion
    }
}
