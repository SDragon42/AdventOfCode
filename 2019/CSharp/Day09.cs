namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/9
/// </summary>
class Day09 : PuzzleBase
{
    const int DAY = 9;


    public override IEnumerable<string> SolvePuzzle()
    {
        yield return "Day 9: Sensor Boost";

        yield return string.Empty;
        yield return RunExample(Example1);
        yield return RunExample(Example2);
        yield return RunExample(Example3);
        yield return Run(Part1);

        yield return string.Empty;
        yield return Run(Part2);
    }

    string Example1() => " Ex. 1) " + RunTestCase(GetPuzzleData(1, "example1"));
    string Example2() => " Ex. 2) " + RunTestCase(GetPuzzleData(1, "example2"));
    string Example3() => " Ex. 3) " + RunTestCase(GetPuzzleData(1, "example3"));
    string Part1() => "Part 1) " + RunBOOST(GetPuzzleData(1, "input"), 1);

    string Part2() => "Part 2) " + RunBOOST(GetPuzzleData(2, "input"), 2);

    class InputAnswer : IntCodeInputAnswer<string> { }
    InputAnswer GetPuzzleData(int part, string name)
    {
        var result = new InputAnswer()
        {
            Input = InputHelper.LoadInputFile(DAY, name).ToList(),
            ExpectedAnswer = InputHelper.LoadAnswerFile(DAY, part, name)?.FirstOrDefault()
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

    string RunTestCase(InputAnswer puzzleData)
    {
        var outputBuffer = new List<long>();
        var computer = new IntCode(puzzleData.Code);
        computer.Output += (s, e) => outputBuffer.Add(e.OutputValue);

        computer.Run();

        var answer = string.Join(',', outputBuffer);
        return Helper.GetPuzzleResultText($"Output: {answer}", answer, puzzleData.ExpectedAnswer);
    }

}
