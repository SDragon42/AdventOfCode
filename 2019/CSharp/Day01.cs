using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.CSharp.Year2019
{
    public class Day01 : TestBase
    {
        public Day01(ITestOutputHelper output) : base(output, 1) { }


        private (List<int>, int?) GetTestData(string name, int part)
        {
            var input = Input.ReadLines(DAY, name)
                .Select(l => l.ToInt32())
                .ToList();

            var expected = Input.ReadLines(DAY, $"{name}-answer{part}")
                ?.FirstOrDefault()
                ?.ToInt32();

            return (input, expected);
        }


        [Theory]
        [InlineData("example1")]
        [InlineData("example2")]
        [InlineData("example3")]
        [InlineData("example4")]
        [InlineData("input")]
        public void Part1(string inputName)
        {
            var (input, expected) = GetTestData(inputName, 1);

            var value = input.Sum(CalcFuel);

            output.WriteLine($"Answer: {value}");

            Assert.Equal(expected, value);
        }

        [Theory]
        [InlineData("example2")]
        [InlineData("example3")]
        [InlineData("input")]
        public void Part2(string inputName)
        {
            var (input, expected) = GetTestData(inputName, 2);

            var value = input.Sum(CalcTotalFuel);

            output.WriteLine($"Answer: {value}");

            Assert.Equal(expected, value);
        }




        /// <summary>
        /// Calculates the Fuel needed for the mass.
        /// </summary>
        /// <param name="mass"></param>
        /// <returns></returns>
        int CalcFuel(int mass)
        {
            return (mass / 3) - 2;
        }


        /// <summary>
        /// Calculates the fuel need to lift the mass, including the fuel.
        /// </summary>
        /// <param name="mass"></param>
        /// <returns></returns>
        int CalcTotalFuel(int mass)
        {
            var fuelMass = CalcFuel(mass);
            if (fuelMass <= 0L)
                return 0;
            return fuelMass + CalcTotalFuel(fuelMass);
        }

    }
}
