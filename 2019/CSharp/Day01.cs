using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.CSharp.Common;

namespace AdventOfCode.CSharp.Year2019
{
    /// <summary>
    /// https://adventofcode.com/2019/day/1
    /// </summary>
    class Day01 : PuzzleBase
    {
        public Day01(bool benchmark) : base(benchmark) { }

        public override IEnumerable<string> SolvePuzzle()
        {
            yield return "Day 1: The Tyranny of the Rocket Equation";

            yield return string.Empty;
            //yield return " Ex. 1) " + base.Run(() => RunPart1(GetPuzzleData(1, "example1")));
            //yield return " Ex. 2) " + base.Run(() => RunPart1(GetPuzzleData(1, "example2")));
            //yield return " Ex. 3) " + base.Run(() => RunPart1(GetPuzzleData(1, "example3")));
            //yield return " Ex. 4) " + base.Run(() => RunPart1(GetPuzzleData(1, "example4")));
            yield return "Part 1) " + base.Run(() => RunPart1(GetPuzzleData(1, "input")));

            yield return string.Empty;
            //yield return " Ex. 2) " + base.Run(() => RunPart2(GetPuzzleData(2, "example2")));
            //yield return " Ex. 3) " + base.Run(() => RunPart2(GetPuzzleData(2, "example3")));
            yield return "Part 2) " + base.Run(() => RunPart2(GetPuzzleData(2, "input")));
        }


        class InputAnswer : InputAnswer<List<int>, int?> { }
        InputAnswer GetPuzzleData(int part, string name)
        {
            const int DAY = 1;

            var result = new InputAnswer()
            {
                Input = InputHelper.LoadInputFile(DAY, name)
                    .Split("\r\n")
                    .Select(l => l.ToInt32())
                    .ToList(),
                ExpectedAnswer = InputHelper.LoadAnswerFile(DAY, part, name).ToInt32()
            };

            return result;
        }


        string RunPart1(InputAnswer puzzleData)
        {
            var answer = puzzleData.Input.Sum(CalcFuel);
            return Helper.GetPuzzleResultText($"Total Fuel needed: {answer}", answer, puzzleData.ExpectedAnswer);
        }

        string RunPart2(InputAnswer puzzleData)
        {
            var answer = puzzleData.Input.Sum(CalcTotalFuel);
            return Helper.GetPuzzleResultText($"Total Fuel needed: {answer}", answer, puzzleData.ExpectedAnswer);
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
