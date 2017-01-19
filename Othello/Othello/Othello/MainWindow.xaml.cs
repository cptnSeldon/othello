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

        /************************
         *      ATTRIBUTES      *
         ************************/
        GameData data;
        DiscView[,] placedDiscs;

        /************************
         *        METHODS       *
         ************************/

         //CONSTRUCTOR
        public MainWindow()
        {
            InitializeComponent();

            InitializeGrid();
            //decomment following if in debug mode
            //InitializeBoard();
            UpdateBoard();
        }

        //GRID INITIALIZATION
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

        //BOARD (RE)INITIALIZATION
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

        //BOARD UPDATE
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

                    //BLACK
                    if (data.StateArray[row, column] == BoardState.PLACED_BLACK)
                    {
                        placedDiscs[row, column].SetState(BoardState.PLACED_BLACK);
                    }
                    //WHITE
                    if (data.StateArray[row, column] == BoardState.PLACED_WHITE)
                    {
                        placedDiscs[row, column].SetState(BoardState.PLACED_WHITE);
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
    }
}
