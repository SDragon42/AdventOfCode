using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.CSharp.Common
{
    public class PuzzleInput<T, U>
    {
        public PuzzleInput() { }
        public PuzzleInput(T inputData)
        {
            Input = inputData;
        }
        public PuzzleInput(T inputData, U answer) : this(inputData)
        {
            ExpectedAnswer = answer;
        }


        public T Input { get; init; }
        public U ExpectedAnswer { get; private set; }
    }
}
