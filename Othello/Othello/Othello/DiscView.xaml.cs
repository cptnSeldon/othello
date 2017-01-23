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
        //CONSTRUCTOR
        public DiscView()
        {
            InitializeComponent();
        }

        //inside method : setting disc state
        public void SetState(BoardState currentState)
        {
            //black
            if (currentState == BoardState.PLACED_BLACK)
                Content = Resources["black"] as Image;
            if (currentState == BoardState.PLAYABLE_BLACK)
                Content = Resources["p_black"] as Image;

            //white
            else if (currentState == BoardState.PLACED_WHITE)
                Content = Resources["white"] as Image;

            else if (currentState == BoardState.PLAYABLE_WHITE)
                Content = Resources["p_white"] as Image;

            //no disc placed on the board
            else if (currentState == BoardState.HIDDEN)
                this.Visibility = Visibility.Hidden;

            //test button click
            this.MouseDoubleClick += DiscView_MouseDoubleClick;
        }

        private void DiscView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("x: {0} y: {1}", Grid.GetRow(this).ToString(), Grid.GetColumn(this).ToString());
        }
    }
}
