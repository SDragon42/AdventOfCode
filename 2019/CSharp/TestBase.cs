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
    }
}
