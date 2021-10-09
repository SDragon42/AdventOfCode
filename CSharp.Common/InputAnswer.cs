using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.CSharp.Common
{
    public class InputAnswer<TI, TA>
    {
        public TI Input { get; set; }
        public TA ExpectedAnswer { get; set; }
    }
}
