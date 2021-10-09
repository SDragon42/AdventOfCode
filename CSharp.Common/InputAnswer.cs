using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.CSharp.Common
{
    public class InputAnswer<TI, TA>
    {
        public InputAnswer(TI input)
        {
            this.Input = input;
        }
        public InputAnswer(TI input, TA expectedAnswer) : this(input)
        {
            this.ExpectedAnswer = expectedAnswer;
        }


        public TI Input { get; private set; }
        public TA ExpectedAnswer { get; private set; }
    }
}
