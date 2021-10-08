using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.CSharp.Year2019.Day14
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
