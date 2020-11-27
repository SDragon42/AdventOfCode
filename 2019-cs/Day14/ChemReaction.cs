using System;
using System.Collections.Generic;
using System.Text;

namespace Advent_of_Code.Day14
{
    class ChemReaction
    {
        public ChemReaction(Chemical chem)
        {
            Product = chem;
            Reactants = new List<ChemReaction>();
        }

        public Chemical Product { get; private set; }
        public List<ChemReaction> Reactants { get; private set; }

        public override string ToString()
        {
            return Product.ToString();
        }
    }

}
