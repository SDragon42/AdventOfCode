using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.CSharp.Common
{
    public interface IPuzzle
    {
        void Run();
    }

    public interface IPuzzle2
    {
        void Part1(string input, string expectedAnswer);
        void Part2(string input, string expectedAnswer);
    }
}
