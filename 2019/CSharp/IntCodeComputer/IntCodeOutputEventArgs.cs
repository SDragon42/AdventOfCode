using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.CSharp.Year2019.IntCodeComputer
{
    class IntCodeOutputEventArgs : EventArgs
    {
        public IntCodeOutputEventArgs(long value) : base()
        {
            OutputValue = value;
        }
        public long OutputValue { get; private set; }
    }
}
