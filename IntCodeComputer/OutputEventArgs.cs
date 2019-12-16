using System;
using System.Collections.Generic;
using System.Text;

namespace Advent_of_Code.IntCodeComputer
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
