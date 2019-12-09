using System;
using System.Collections.Generic;
using System.Text;

namespace Advent_of_Code
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    class OpCodeAttribute : Attribute
    {
        public OpCodeAttribute(int opCode)
        {
            OpCode = opCode;
        }

        public int OpCode { get; private set; }
    }
}
