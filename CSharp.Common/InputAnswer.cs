using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.CSharp.Common
{
    public class InputAnswer<TI, TA>
    {
        public InputAnswer() { }
        public InputAnswer(TI input, TA expectedAnswer)
        {
            Input = input;
            ExpectedAnswer = expectedAnswer;
        }

        private TI _input;
        public TI Input
        {
            get => _input;
            set => SetInput(value);
        }
        protected virtual void SetInput(TI value)
        {
            _input = value;
        }

        private TA _expectedAnswer;
        public TA ExpectedAnswer
        {
            get => _expectedAnswer;
            set => SetExpectedAnswer(value);
        }
        protected virtual void SetExpectedAnswer(TA value)
        {
            _expectedAnswer = value;
        }
    }
}
