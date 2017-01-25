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
        BoardState currentState;
        #endregion

        #region METHODS
        /* INITIALIZATION */
        public DiscView()
        {
            InitializeComponent();
        }

        /* SET DISC'S STATE */
        public void SetState(BoardState currentState)
        {
            //black
            switch (currentState)
            {
                case BoardState.PLACED_BLACK:
                    Content = Resources["black"] as Image;
                    break;
                case BoardState.PLAYABLE_BLACK:
                    Content = Resources["p_black"] as Image;
                    break;
                case BoardState.PLACED_WHITE:
                    Content = Resources["white"] as Image;
                    break;
                case BoardState.PLAYABLE_WHITE:
                    Content = Resources["p_white"] as Image;
                    break;
            }
            this.currentState = currentState;
            Visibility = currentState == BoardState.HIDDEN ? Visibility.Hidden : Visibility.Visible;
            IsEnabled = currentState == BoardState.PLAYABLE_BLACK || currentState == BoardState.PLAYABLE_WHITE;

            //test button click
            this.MouseDown += DiscView_MouseDown;
        }

        /* MOUSE CLICK EVENT */
        private void DiscView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Content == Resources["p_black"])
            {
                Console.WriteLine("Playable black at position \n\tx: {0} y: {1}", Grid.GetRow(this).ToString(), Grid.GetColumn(this).ToString());
            }

            else if (Content == Resources["p_white"])
            {
                Console.WriteLine("Playable white at position \n\tx: {0} y: {1}", Grid.GetRow(this).ToString(), Grid.GetColumn(this).ToString());
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
