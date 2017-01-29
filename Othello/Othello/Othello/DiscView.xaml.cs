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

            this.MouseEnter += DiscView_MouseEnter;
            this.MouseLeave += DiscView_MouseLeave;
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

            //
            this.currentState = currentState;
            //not visible
            IsEnabled = currentState == BoardState.PLAYABLE_BLACK || currentState == BoardState.PLAYABLE_WHITE;
            if (this.currentState == BoardState.PLAYABLE_BLACK || this.currentState == BoardState.PLAYABLE_WHITE)
            {
                Opacity = 0;
            }
            //visible
            Visibility = currentState == BoardState.HIDDEN ? Visibility.Hidden : Visibility.Visible;


        }

        /* MOUSE CLICK EVENT */
        public void setMouseDown(MouseButtonEventHandler eventHandler)
        {
            this.MouseDown += eventHandler;
        }

        /* MOUSE ENTER */
        private void DiscView_MouseEnter(object sender, MouseEventArgs e)
        {
            if (currentState == BoardState.PLAYABLE_BLACK || currentState == BoardState.PLAYABLE_WHITE)
            {
                Opacity = 100;
            }
        }

        /* MOUSE LEAVE */
        private void DiscView_MouseLeave(object sender, MouseEventArgs e)
        {
            if (currentState == BoardState.PLAYABLE_BLACK || currentState == BoardState.PLAYABLE_WHITE)
            {
                Opacity = 0;
            }
        }

        #endregion
    }
}
