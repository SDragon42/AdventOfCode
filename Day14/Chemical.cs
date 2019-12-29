using System;
using System.Collections.Generic;
using System.Text;

namespace Advent_of_Code.Day14
{
    class Chemical
    {
        public Chemical(string element, int amount)
        {
            Name = element;
            Quantity = amount;
        }

        public string Name { get; private set; }
        public int Quantity { get; set; }

        public override string ToString()
        {
            return $"{Quantity} {Name}";
        }
    }

}
