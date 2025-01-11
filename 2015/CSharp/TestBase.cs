using AdventOfCode.Common.Services;
using AdventOfCode.Common.Services.Interfaces;
using NUnit.Framework;

namespace AdventOfCode.CSharp.Year2015
{
    public abstract class TestBase
    {

        protected void Output(string text)
        {
            TestContext.Out.WriteLine(text);
        }

        private const string INPUT_ROOT_PATH = @"../../../../../../AdventOfCode.Input/2015";
        public IInputReaderService Input { get; } = new InputReaderService(TestContext.CurrentContext.TestDirectory + "/" + INPUT_ROOT_PATH);
    }
}
