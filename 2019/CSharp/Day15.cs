using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.CSharp.Common;
using AdventOfCode.CSharp.Year2019.IntCodeComputer;

namespace AdventOfCode.CSharp.Year2019
{
    /// <summary>
    /// https://adventofcode.com/2019/day/15
    /// </summary>
    class Day15 : PuzzleBase
    {
        public override IEnumerable<string> SolvePuzzle()
        {
            yield return "Day 15: Oxygen System";

            yield return string.Empty;
            yield return Run(Part1);

            yield return string.Empty;
            yield return Run(Part2);
        }

        string Part1() => "Part 1) " + RunPart1(GetPuzzleData(1, "input"));
        string Part2() => "Part 2) " + RunPart2(GetPuzzleData(2, "input"));



        class InputAnswer : IntCodeInputAnswer<long?> { }
        InputAnswer GetPuzzleData(int part, string name)
        {
            const int DAY = 13;

            var result = new InputAnswer()
            {
                Input = InputHelper.LoadInputFile(DAY, name).ToList(),
                ExpectedAnswer = InputHelper.LoadAnswerFile(DAY, part, name)?.FirstOrDefault()?.ToInt64()
            };
            return result;
        }



        string RunPart1(InputAnswer puzzleData)
        {
            return string.Empty;
        }

        string RunPart2(InputAnswer puzzleData)
        {
            return string.Empty;
        }
    }
}
