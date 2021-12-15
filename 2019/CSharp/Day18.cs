namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/17
/// </summary>
class Day18 : PuzzleBase
{
    const int DAY = 18;


    public override IEnumerable<string> SolvePuzzle()
    {
        yield return "Day 18: Many-Worlds Interpretation";
        yield return string.Empty;

        yield return RunExample(() => " Ex. 1) " + RunPart1(GetPuzzleData(1, "example1")));
        yield return RunExample(() => " Ex. 2) " + RunPart1(GetPuzzleData(1, "example2")));
        yield return RunExample(() => " Ex. 3) " + RunPart1(GetPuzzleData(1, "example3")));
        yield return RunExample(() => " Ex. 4) " + RunPart1(GetPuzzleData(1, "example4")));
        yield return RunExample(() => " Ex. 5) " + RunPart1(GetPuzzleData(1, "example5")));
        //yield return RunProblem(() => "Part 1) " + RunPart1(GetPuzzleData(1, "input")));

        //yield return string.Empty;

        //yield return RunExample(() => " Ex. 1) " + RunPart1(GetPuzzleData(2, "example1")));
        //yield return RunProblem(() => "Part 2) " + RunPart1(GetPuzzleData(2, "input")));
    }


    class InputAnswer : InputAnswer<List<List<char>>, int?> { }
    InputAnswer GetPuzzleData(int part, string name)
    {
        var result = new InputAnswer()
        {
            Input = InputHelper.LoadInputFile(DAY, name)
                .Select(l => l.ToList())
                .ToList(),
            ExpectedAnswer = InputHelper.LoadAnswerFile(DAY, part, name)?.FirstOrDefault()?.ToInt32()
        };

        return result;
    }


    string RunPart1(InputAnswer puzzleData)
    {
        var answer = 0;
        return Helper.GetPuzzleResultText($"How many steps is the shortest path that collects all of the keys? {answer}", answer, puzzleData.ExpectedAnswer);
    }

    string RunPart2(InputAnswer puzzleData)
    {
        var answer = 0;
        return Helper.GetPuzzleResultText($": {answer}", answer, puzzleData.ExpectedAnswer);
    }

}
