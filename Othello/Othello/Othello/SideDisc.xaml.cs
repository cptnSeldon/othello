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
        //CONSTRUCTOR
        public SideDisc()
        {
            InitializeComponent();
        }

        //inside method : used for setting the right image to the right disc
        public void SetState(GameState currentState)
        {
            // BLACK
            if (currentState == GameState.BLACK_TURN)
                Content = Resources["sb_disc"] as Image;

            //WHITE
            else if (currentState == GameState.WHITE_TURN)
                Content = Resources["sw_disc"] as Image;
        }
    }
}
