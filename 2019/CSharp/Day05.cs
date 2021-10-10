using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.CSharp.Common;
using AdventOfCode.CSharp.Year2019.IntCodeComputer;

namespace AdventOfCode.CSharp.Year2019
{
    class Day05 : PuzzleBase
    {
        public Day05(bool benchmark) : base(benchmark) { }

        public override IEnumerable<string> SolvePuzzle()
        {
            yield return "Day 5: Sunny with a Chance of Asteroids";

            yield return string.Empty;
            //yield return " Ex. 1) " + base.Run(() => RunPart1(GetPuzzleData(1, "example1"), 69, 69));
            //yield return " Ex. 2) " + base.Run(() => RunPart1(GetPuzzleData(1, "example2"), 1, 0));
            yield return "Part 1) " + base.Run(() => RunPart1(GetPuzzleData(1, "input"), 1));

            yield return string.Empty;
            //yield return base.Run(() => RunPart2Example3(8));
            //yield return base.Run(() => RunPart2Example3(7));
            //yield return base.Run(() => RunPart2Example4(0));
            //yield return base.Run(() => RunPart2Example4(42));
            yield return "Part 2) " + base.Run(() => RunPart2(GetPuzzleData(2, "input")));
        }


        class InputAnswer : InputAnswer<List<long>, long?> { }
        InputAnswer GetPuzzleData(int part, string name)
        {
            const int DAY = 5;

            var result = new InputAnswer()
            {
                Input = InputHelper.LoadInputFile(DAY, name)
                    .Split(',')
                    .Select(l => l.ToInt64())
                    .ToList(),
                ExpectedAnswer = InputHelper.LoadAnswerFile(DAY, part, name)?.ToInt64()
            };
            return result;
        }


        string RunPart1(InputAnswer puzzleData, long inputValue, long? overrideExpectedAnswer = null)
        {
            if (overrideExpectedAnswer.HasValue)
                puzzleData.ExpectedAnswer = overrideExpectedAnswer.Value;

            var answer = 0L;
            var computer = new IntCode(puzzleData.Input);
            computer.Output += (s, e) => { answer = e.OutputValue; };

            computer.Run();
            if (computer.State == IntCodeState.NeedsInput)
            {
                computer.AddInput(inputValue);
                computer.Run();
            }

            return Helper.GetPuzzleResultText($"The diagnostic Code is: {answer}", answer, puzzleData.ExpectedAnswer);
        }


        string RunPart2(InputAnswer puzzleData)
        {
            var answer = 0L;
            var computer = new IntCode(puzzleData.Input);
            computer.Output += (s, e) => { answer = e.OutputValue; };

            computer.Run();
            if (computer.State == IntCodeState.NeedsInput)
            {
                computer.AddInput(5);
                computer.Run();
            }

            return Helper.GetPuzzleResultText($"The diagnostic Code is: {answer}", answer, puzzleData.ExpectedAnswer);
        }

        string RunPart2Example3(long inputValue)
        {
            var sb = new StringBuilder();
            var inputList = InputHelper.LoadInputFile(5, "example3")
                .Split("\r\n")
                .Select(l => l.Split(',').Select(v => v.ToInt64()).ToList());

            sb.AppendLine($"Input: {inputValue}");
            foreach (var input in inputList)
            {
                var computer = new IntCode(input);
                computer.Output += (s, e) => sb.AppendLine($"Output: {e.OutputValue}");

                computer.Run();
                if (computer.State == IntCodeState.NeedsInput)
                {
                    computer.AddInput(inputValue);
                    computer.Run();
                }
            }
            sb.AppendLine();

            return sb.ToString();
        }

        string RunPart2Example4(int inputValue)
        {
            var sb = new StringBuilder();
            var inputList = InputHelper.LoadInputFile(5, "example4")
                .Split("\r\n")
                .Select(l => l.Split(',').Select(v => v.ToInt64()).ToList());

            sb.AppendLine($"Input: {inputValue}");
            foreach (var input in inputList)
            {
                Console.WriteLine("-------------------");
                var computer = new IntCode(input);
                //computer.AfterRunStep += (s, e) => ShowState((IntCode)s).ForEach(l => sb.AppendLine(l));
                computer.Output += (s, e) => sb.AppendLine($"Output: {e.OutputValue}");

                //ShowState(computer).ForEach(l => sb.AppendLine(l));
                computer.Run();
                if (computer.State == IntCodeState.NeedsInput)
                {
                    computer.AddInput(inputValue);
                    computer.Run();
                }
            }
            sb.AppendLine();

            return sb.ToString();
        }

        IEnumerable<string> ShowState(IntCode comp)
        {
            yield return $"State: {comp.State}   Address: {comp.AddressPointer}";
            yield return comp.Dump();
            yield return string.Empty;
        }

    }
}
