﻿using System;
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
    class Day02 : PuzzleBase
    {
        public Day02(bool benchmark) : base(benchmark) { }

        public override IEnumerable<string> SolvePuzzle()
        {
            yield return "Day 2: 1202 Program Alarm";

            yield return string.Empty;
            yield return " Ex. 1) " + base.Run(() => RunPart1(GetPuzzleData(1, "example1")));
            yield return "Part 1) " + base.Run(() => RunPart1(GetPuzzleData(1, "input"), 12, 2));

            yield return string.Empty;
            yield return "Part 2) " + base.Run(() => RunPart2(GetPuzzleData(2, "input")));
        }


        class InputAnswer : InputAnswer<List<long>, long?> { }
        InputAnswer GetPuzzleData(int part, string name)
        {
            const int DAY = 2;

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


        string RunPart1(InputAnswer puzzleData, int valueAt1 = -1, int valueAt2 = -1)
        {
            var answer = RunCode(puzzleData.Input, valueAt1, valueAt2);
            return Helper.GetPuzzleResultText($"Value as position 0 : {answer}", answer, puzzleData.ExpectedAnswer);
        }



        string RunPart2(InputAnswer puzzleData)
        {
            var answer = FindNounVerb(puzzleData.Input, 19690720);
            return Helper.GetPuzzleResultText($"Noun-Verb pair is : {answer}", answer, puzzleData.ExpectedAnswer);
        }


        private long RunCode(List<long> code, long valueAt1, long valueAt2)
        {
            var computer = new IntCodeComputer.IntCode(code);
            if (valueAt1 >= 0) computer.Poke(1, valueAt1);
            if (valueAt2 >= 0) computer.Poke(2, valueAt2);
            computer.Run();

            var valAt0 = computer.Peek(0);
            return valAt0;
        }


        int FindNounVerb(List<long> input, long desiredValueAt0)
        {
            for (var noun = 0; noun <= 99; noun++)
            {
                for (var verb = 0; verb <= 99; verb++)
                {
                    var result = RunCode(input, noun, verb);
                    if (result == desiredValueAt0)
                        return (100 * noun) + verb;
                }
            }

            return -1;
        }

    }
}
