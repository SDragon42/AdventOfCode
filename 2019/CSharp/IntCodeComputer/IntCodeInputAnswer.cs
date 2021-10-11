using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.CSharp.Common;

namespace AdventOfCode.CSharp.Year2019.IntCodeComputer
{
    class IntCodeInputAnswer<TA> : InputAnswer<List<string>, TA>
    {
        public IntCodeInputAnswer() : base() { }
        public IntCodeInputAnswer(List<string> input, TA expectedAnswer) : base(input, expectedAnswer)
        {
        }

        protected override void SetInput(List<string> value)
        {
            base.SetInput(value);
            Code = Input.First()
                .Split(',')
                .Select(v => v.ToInt64())
                .ToList();
        }

        public List<long> Code { get; private set; }
    }
}
