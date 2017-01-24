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
    /// Interaction logic for DiscView.xaml
    /// </summary>
    public partial class DiscView : UserControl
    {
        #region ATTRIBUTES
        BoardState currentState;    //for test use only (pre-unit test)
        #endregion

        #region METHODS
        //Initialization
        public DiscView()
        {
            InitializeComponent();
        }

        //Set disc's state : external
        public void SetState(BoardState currentState_)
        {
            //black
            if (currentState_ == BoardState.PLACED_BLACK)
            {
                Content = Resources["black"] as Image;
                currentState = currentState_;
                IsEnabled = false;
            }
                
            if (currentState_ == BoardState.PLAYABLE_BLACK)
            {
                Content = Resources["p_black"] as Image;
                currentState = currentState_;
            }
                
            //white
            else if (currentState_ == BoardState.PLACED_WHITE)
            {
                Content = Resources["white"] as Image;
                currentState = currentState_;
                IsEnabled = false;
            }
                
            else if (currentState_ == BoardState.PLAYABLE_WHITE)
            {
                Content = Resources["p_white"] as Image;
                currentState = currentState_;
            }

            //no disc placed on the board
            else if (currentState_ == BoardState.HIDDEN)
            {
                Visibility = Visibility.Hidden;
                currentState = currentState_;
            }

            //test button click
            this.MouseDoubleClick += DiscView_MouseDoubleClick;
        }

        //Set disc's state : internal
        //for test use only (pre-unit test)
        public void SetState()
        {
            if (currentState == BoardState.PLAYABLE_BLACK)
            {
                Content = Resources["black"] as Image;
                currentState = BoardState.PLACED_BLACK;
            }
            else if (currentState == BoardState.PLAYABLE_WHITE)
            {
                Content = Resources["white"] as Image;
                currentState = BoardState.PLACED_WHITE;
            }
        }

        //Reaction to mouse click
        private void DiscView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Content == Resources["p_black"])
            {
                Console.WriteLine("Playable black at position \n\tx: {0} y: {1}", Grid.GetRow(this).ToString(), Grid.GetColumn(this).ToString());
                SetState(); //for test use only (pre-unit test)
            }
                
            else if (Content == Resources["p_white"])
            {
                Console.WriteLine("Playable white at position \n\tx: {0} y: {1}", Grid.GetRow(this).ToString(), Grid.GetColumn(this).ToString());
                SetState(); //for test use only (pre-unit test)
            }
                
            else
            {
                //debug test : won't send anything as the items are deactivated if not playable
                Console.WriteLine("Not valid position");
            }
                
        }
        #endregion
    }
}
