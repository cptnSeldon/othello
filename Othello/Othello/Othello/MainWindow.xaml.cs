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
        /* MAIN INITIALIZATION */
        public MainWindow()
        {
            engine = new OthelloEngine();
            //window init
            StartNewGame();
        }

        /* START NEW GAME */
        public void StartNewGame()
        {
            InitializeComponent();

            //panels init
            ClearPanels();
            InitializePanels();

            black_label.DataContext = (engine as OthelloEngine).data;
            white_label.DataContext = (engine as OthelloEngine).data;
            b_score_label.DataContext = (engine as OthelloEngine).data;
            w_score_label.DataContext = (engine as OthelloEngine).data;
            //grid init
            ClearGrid();
            ClearBoard();
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

        /* CLEAR REMAINING DISCS' PANEL */
        public void ClearPanels()
        {
            b_discs.Children.Clear();
            w_discs.Children.Clear();
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

        /* CLEAR GRID */
        private void ClearGrid()
        {
            board_grid.ColumnDefinitions.Clear();
            board_grid.RowDefinitions.Clear();
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

        /* CLEAR BOARD */
        public void ClearBoard()
        {
            board_grid.Children.Clear();
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
            while (b_discs.Children.Count > engine.getTotalBlack()
                && engine.getTotalBlack() >= 0)
            {
                b_discs.Children.RemoveAt(0);
            }

            while (w_discs.Children.Count > engine.getTotalWhite()
                && engine.getTotalWhite() >= 0)
            {
                w_discs.Children.RemoveAt(0);
            }
        }

        #region EVENT HANDLERS
        /* DISCS ON BOARD */
        private void DiscView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            engine.playMove(Grid.GetRow((DiscView)sender), Grid.GetColumn((DiscView)sender), true);
            UpdateBoard(engine.getGameState());

            //skip management
            if (engine.HasSkipped() == true)
            {
                MessageBox.Show("No move left! The player's turn is been skipped.", "Skip", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            //game end management
            if (engine.GetCurrentGameState() == GameState.GAME_END)
            {
                string messageToShow;
                if (engine.getBlackScore() > engine.getWhiteScore())
                {
                    messageToShow = "Player 1 wins with "+engine.getBlackScore()+" points!";
                }
                else if(engine.getBlackScore() < engine.getWhiteScore())
                {
                    messageToShow = "Player 2 wins with " + engine.getWhiteScore() + " points!";
                }
                else
                {
                    messageToShow = "Game ended with a draw!";
                }
                
                MessageBox.Show(messageToShow, "End of the game", MessageBoxButton.OK, MessageBoxImage.Information); 
            }
        }
        /* MENU 1 : EXIT */
        private void AppExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("The program will now shut down.", "Quit", MessageBoxButton.OK, MessageBoxImage.Information);
            Application.Current.Shutdown();
        }
        /* MENU 2 : SAVE GAME */
        private void SaveGame_Click(object sender, RoutedEventArgs e)
        {
            //open file dialog
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            //setting filter
            dialog.DefaultExt = ".json";
            dialog.Filter = "JSON data format (.json)|*.json"; // Filter files by extension

            //show dialog call
            Nullable<bool> result = dialog.ShowDialog();
            //get selected file name
            if (result == true)
            {
                string filename = dialog.FileName;
                engine.Save(filename);
            }  
        }
        /* MENU 3 : NEW GAME */
        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            //engine = new OthelloEngine();
            engine.StartNewGame();
            StartNewGame();
        }
        /* MENU 4 : LOAD GAME */
        private void LoadGame_Click(object sender, RoutedEventArgs e)
        {
            //open file dialog
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            //setting filter
            dialog.DefaultExt = ".txt";
            //show dialog call
            Nullable<bool> result = dialog.ShowDialog();
            //get selected file name
            if (result == true)
            {
                string filename = dialog.FileName;
                engine.Load(filename);
            }
            StartNewGame();
        }
        /* MENU 5 : ABOUT */
        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, "Welcome,\n"+
                "If you aren't familiar with the game rules yet, this link may help you :\n"+
                "\n\t http://radagast.se/othello/Help/strategy.html \n\n"+
                "Have fun!\n\n\n" +
                "Designed and implemented by Julia Németh and Axel Roy.\n"+
                "HE-ARC, 3dlm-ab, January 2017",
                "Quick hello from the dev's", MessageBoxButton.OK, MessageBoxImage.Information
                );
        }
        #endregion

        #endregion


    }
}
