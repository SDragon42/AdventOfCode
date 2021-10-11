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
        public override IEnumerable<string> SolvePuzzle()
        {
            yield return "Day 1: The Tyranny of the Rocket Equation";

            yield return string.Empty;
            yield return RunExample(Example1);
            yield return RunExample(Example2);
            yield return RunExample(Example3);
            yield return RunExample(Example4);
            yield return Run(Part1);

            yield return string.Empty;
            yield return RunExample(Example2P2);
            yield return RunExample(Example3P2);
            yield return Run(Part2);
        }

        string Example1() => " Ex. 1) " + RunPart1(GetPuzzleData(1, "example1"));
        string Example2() => " Ex. 2) " + RunPart1(GetPuzzleData(1, "example2"));
        string Example3() => " Ex. 3) " + RunPart1(GetPuzzleData(1, "example3"));
        string Example4() => " Ex. 4) " + RunPart1(GetPuzzleData(1, "example4"));
        string Part1() => "Part 1) " + RunPart1(GetPuzzleData(1, "input"));

        string Example2P2() => " Ex. 2) " + RunPart1(GetPuzzleData(2, "example2"));
        string Example3P2() => " Ex. 3) " + RunPart1(GetPuzzleData(2, "example3"));
        string Part2() => "Part 2) " + RunPart2(GetPuzzleData(2, "input"));


        class InputAnswer : InputAnswer<List<int>, int?> { }
        InputAnswer GetPuzzleData(int part, string name)
        {
            const int DAY = 1;

            var result = new InputAnswer()
            {
                Input = InputHelper.LoadInputFile(DAY, name)
                    .Select(l => l.ToInt32())
                    .ToList(),
                ExpectedAnswer = InputHelper.LoadAnswerFile(DAY, part, name)?.FirstOrDefault()?.ToInt32()
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
