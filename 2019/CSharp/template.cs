namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/17
/// </summary>
class Template : PuzzleBase
{
    const int DAY = 0;


    public override IEnumerable<string> SolvePuzzle()
    {
        yield return "Title";
        yield return string.Empty;

        yield return RunExample(() => " Ex. 1) " + RunPart1(GetPuzzleData(1, "example1")));
        yield return RunProblem(() => "Part 1) " + RunPart1(GetPuzzleData(1, "input")));

        yield return string.Empty;

        yield return RunExample(() => " Ex. 1) " + RunPart1(GetPuzzleData(2, "example1")));
        yield return RunProblem(() => "Part 2) " + RunPart1(GetPuzzleData(2, "input")));
    }


    class InputAnswer : InputAnswer<List<int>, int?> { }
    InputAnswer GetPuzzleData(int part, string name)
    {
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
        var answer = 0;
        return Helper.GetPuzzleResultText($": {answer}", answer, puzzleData.ExpectedAnswer);
    }

    string RunPart2(InputAnswer puzzleData)
    {
        var answer = 0;
        return Helper.GetPuzzleResultText($": {answer}", answer, puzzleData.ExpectedAnswer);
    }

}
