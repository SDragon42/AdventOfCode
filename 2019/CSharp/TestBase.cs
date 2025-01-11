using AdventOfCode.Common.Services.Interfaces;
using AdventOfCode.Common.Services;
using Xunit.Abstractions;

namespace AdventOfCode.CSharp.Year2019
{
    public abstract class TestBase
    {
        protected readonly int DAY;
        protected readonly ITestOutputHelper output;


        public TestBase(ITestOutputHelper output, int day)
        {
            this.DAY = day;
            this.output = output;
        }

        private const string INPUT_ROOT_PATH = @"../../../../../../AdventOfCode.Input/2019";
        public static IInputReaderService Input { get; } = new InputReaderService(INPUT_ROOT_PATH);
    }
}
