namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/5
/// </summary>
class Day05 : PuzzleBase
{
    const int DAY = 5;


    public override IEnumerable<string> SolvePuzzle()
    {
        yield return "Day 5: Sunny with a Chance of Asteroids";

        yield return string.Empty;
        yield return RunExample(Example1);
        yield return RunExample(Example2);
        yield return Run(Part1);

        yield return string.Empty;
        //yield return RunExample(Example3a);
        //yield return RunExample(Example3b);
        //yield return RunExample(Example4a);
        //yield return RunExample(Example4b);
        yield return Run(Part2);
    }

    string Example1() => " Ex. 1) " + RunPart1(GetPuzzleData(1, "example1"), 69, 69);
    string Example2() => " Ex. 2) " + RunPart1(GetPuzzleData(1, "example2"), 1, 0);
    string Part1() => "Part 1) " + RunPart1(GetPuzzleData(1, "input"), 1);

    string Example3a() => " Ex. 3a) " + RunPart2Example3(8);
    string Example3b() => " Ex. 3b) " + RunPart2Example3(7);
    string Example4a() => " Ex. 4a) " + RunPart2Example4(0);
    string Example4b() => " Ex. 4b) " + RunPart2Example4(42);
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


    string RunPart1(InputAnswer puzzleData, long inputValue, long? overrideExpectedAnswer = null)
    {
        if (overrideExpectedAnswer.HasValue)
            puzzleData.ExpectedAnswer = overrideExpectedAnswer.Value;

        var answer = 0L;
        var computer = new IntCode(puzzleData.Code);
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
        var computer = new IntCode(puzzleData.Code);
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
            .Select(l => l.Split(',').Select(v => v.ToInt64()).ToList());

        sb.AppendLine($"Input: {inputValue}");
        foreach (var input in inputList)
        {
            sb.AppendLine("-------------------");
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

    IEnumerable<string> ShowState(IntCode comp)
    {
        yield return $"State: {comp.State}   Address: {comp.AddressPointer}";
        yield return comp.Dump();
        yield return string.Empty;
    }

}
