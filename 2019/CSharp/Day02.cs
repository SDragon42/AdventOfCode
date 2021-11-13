namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/2
/// </summary>
class Day02 : PuzzleBase
{
    const int DAY = 2;


    public override IEnumerable<string> SolvePuzzle()
    {
        yield return "Day 2: 1202 Program Alarm";

        yield return string.Empty;
        yield return RunExample(Example1);
        yield return Run(Part1);

        yield return string.Empty;
        yield return Run(Part2);
    }

    string Example1() => " Ex. 1) " + RunPart1(GetPuzzleData(1, "example1"));
    string Part1() => "Part 1) " + RunPart1(GetPuzzleData(1, "input"), 12, 2);
    string Part2() => "Part 2) " + RunPart2(GetPuzzleData(2, "input"));


    class InputAnswer : IntCodeInputAnswer<long?> { }
    InputAnswer GetPuzzleData(int part, string name)
    {
        var result = new InputAnswer()
        {
            Input = InputHelper.LoadInputFile(DAY, name).ToList(),
            ExpectedAnswer = InputHelper.LoadAnswerFile(DAY, part, name)?.FirstOrDefault()?.ToInt64()
        };
        return result;
    }


    string RunPart1(InputAnswer puzzleData, int valueAt1 = -1, int valueAt2 = -1)
    {
        var answer = RunCode(puzzleData.Code, valueAt1, valueAt2);
        return Helper.GetPuzzleResultText($"Value as position 0 : {answer}", answer, puzzleData.ExpectedAnswer);
    }



    string RunPart2(InputAnswer puzzleData)
    {
        var answer = FindNounVerb(puzzleData.Code, 19690720);
        return Helper.GetPuzzleResultText($"Noun-Verb pair is : {answer}", answer, puzzleData.ExpectedAnswer);
    }


    private long RunCode(List<long> code, long valueAt1, long valueAt2)
    {
        var computer = new IntCode(code);
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
