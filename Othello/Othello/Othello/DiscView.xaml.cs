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
            
        }

        /* MOUSE CLICK EVENT */
        public void setMouseDown(MouseButtonEventHandler eventHandler)
        {
            this.MouseDown += eventHandler;
        }
        #endregion
    }
}
