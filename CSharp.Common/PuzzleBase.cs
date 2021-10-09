using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AdventOfCode.CSharp.Common
{
    public abstract class PuzzleBase
    {
        readonly Stopwatch benchmark;

        public PuzzleBase(bool benchmark)
        {
            if (benchmark)
                this.benchmark = new Stopwatch();
        }


        public abstract IEnumerable<string> SolvePuzzle();


        protected string Run(Func<string> method)
        {
            benchmark?.Start();
            var text = method.Invoke();
            benchmark?.Stop();

            if (benchmark != null)
                return text + $" - {benchmark.ElapsedMilliseconds} ms";
            else
                return text;
        }


    }
}
