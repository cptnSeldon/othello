using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Othello
{
    class Disc
    {
        private Colors colors;

        public Colors Colors
                {
                    get { return colors; }
                    set { colors = value; }
                }

        /*
                // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
                public static readonly DependencyProperty MyPropertyProperty =
                    DependencyProperty.Register("MyProperty", typeof(int), typeof(ownerclass), new PropertyMetadata(0));
        */
    }
}
