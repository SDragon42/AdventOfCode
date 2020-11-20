using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Advent_of_Code.Day11
{


    class HullPanel
    {
        public HullPanel(Point location)
        {
            Location = location;
            Color = HullColor.Black;
        }

        public Point Location { get; private set; }
        public HullColor Color { get; set; }
    }

    enum HullColor
    {
        Undefined = -1,
        Black = 0,
        White = 1
    }
}
