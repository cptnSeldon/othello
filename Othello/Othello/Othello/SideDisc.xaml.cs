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
    /// Interaction logic for SideDisc.xaml
    /// </summary>
    public partial class SideDisc : UserControl
    {
        #region METHODS
        //Initialization
        public SideDisc()
        {
            InitializeComponent();
        }

        //Set disc's state
        public void SetState(GameState currentState)
        {
            if (currentState == GameState.BLACK_TURN)
                Content = Resources["sb_disc"] as Image;
            
            else if (currentState == GameState.WHITE_TURN)
                Content = Resources["sw_disc"] as Image;
        }
        #endregion
    }
}
