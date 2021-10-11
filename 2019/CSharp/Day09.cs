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
    /// https://adventofcode.com/2019/day/9
    /// </summary>
    class Day09 : PuzzleBase
    {
        public Day09(bool benchmark) : base(benchmark) { }

        public override IEnumerable<string> SolvePuzzle()
        {
            yield return "Day 9: Sensor Boost";

            yield return string.Empty;
            //yield return " Ex. 1) " + base.Run(() => RunExample(GetPuzzleData(1, "example1")));
            //yield return " Ex. 2) " + base.Run(() => RunExample(GetPuzzleData(1, "example2")));
            //yield return " Ex. 3) " + base.Run(() => RunExample(GetPuzzleData(1, "example3")));
            yield return "Part 1) " + base.Run(() => RunBOOST(GetPuzzleData(1, "input"), 1));

            yield return string.Empty;
            yield return "Part 2) " + base.Run(() => RunBOOST(GetPuzzleData(2, "input"), 2));
        }

        class InputAnswer : IntCodeInputAnswer<string> { }
        InputAnswer GetPuzzleData(int part, string name)
        {
            const int DAY = 9;

            var result = new InputAnswer()
            {
                Input = InputHelper.LoadInputFile(DAY, name).AsLines().ToList(),
                ExpectedAnswer = InputHelper.LoadAnswerFile(DAY, part, name)
            };
            return result;
        }


        string RunBOOST(InputAnswer puzzleData, long initalInput)
        {
            var answer = default(string);

            var computer = new IntCode(puzzleData.Code);
            computer.Output += (s, e) => answer = e.OutputValue.ToString();

            computer.AddInput(initalInput);
            computer.Run();

            return Helper.GetPuzzleResultText($"BOOST key-code: {answer}", answer, puzzleData.ExpectedAnswer);
        }

        string RunExample(InputAnswer puzzleData)
        {
            var outputBuffer = new List<long>();
            var computer = new IntCode(puzzleData.Code);
            computer.Output += (s, e) => outputBuffer.Add(e.OutputValue);

            computer.Run();

            var answer = string.Join(',', outputBuffer);
            return Helper.GetPuzzleResultText($"Output: {answer}", answer, puzzleData.ExpectedAnswer);
        }

    }
}
