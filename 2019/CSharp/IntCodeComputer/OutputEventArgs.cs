using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.CSharp.Year2019.IntCodeComputer
{
    class OutputEventArgs : EventArgs
    {
        public OutputEventArgs(long value) : base()
        {
            OutputValue = value;
        }
        public long OutputValue { get; private set; }
    }
}
