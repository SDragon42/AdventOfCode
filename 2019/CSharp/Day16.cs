namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/12
/// </summary>
class Day16 : PuzzleBase
{
    const int DAY = 16;

    private readonly IReadOnlyList<int> basePattern = new List<int>() { 0, 1, 0, -1 };

    public override IEnumerable<string> SolvePuzzle()
    {
        Debug = (t) => { };

        yield return "Day 16: Flawed Frequency Transmission";
        yield return string.Empty;

        yield return RunExample(() => " Ex. 1) " + RunPart1(GetPuzzleData(1, "example1")));
        yield return RunExample(() => " Ex. 2) " + RunPart1(GetPuzzleData(1, "example2")));
        yield return RunExample(() => " Ex. 3) " + RunPart1(GetPuzzleData(1, "example3")));
        yield return RunExample(() => " Ex. 4) " + RunPart1(GetPuzzleData(1, "example4")));
        yield return RunProblem(() => "Part 1) " + RunPart1(GetPuzzleData(1, "input")));

        yield return string.Empty;

        //yield return RunExample(() => " Ex. 1) " + RunPart2(GetPuzzleData(2, "example1")));
        //yield return RunExample(() => " Ex. 2) " + RunPart2(GetPuzzleData(2, "example2")));
        //yield return RunProblem(() => "Part 2) " + RunPart2(GetPuzzleData(1, "input")));
    }



    class InputAnswer : InputAnswer<string, string> { }
    InputAnswer GetPuzzleData(int part, string name)
    {
        var result = new InputAnswer()
        {
            Input = InputHelper.LoadInputFile(DAY, name).First(),
            ExpectedAnswer = InputHelper.LoadAnswerFile(DAY, part, name)?.FirstOrDefault()
        };
        return result;
    }

    string ListToString(IList<int> l) => string.Join("", l.Select(x => x.ToString()));
    
    Action<string> Debug = (t) => Console.WriteLine(t);

    string RunPart1(InputAnswer puzzleData)
    {
        var phase = 0;

        var signal = puzzleData.Input.Select(c => c.ToString().ToInt32()).ToList();
        var maxDigits = signal.Count;

        Debug("Input Signal: " + ListToString(signal));
        Debug("");

        var line = new StringBuilder();
        while (phase < 100)
        {
            phase++;

            var newSignal = new List<int>();
            for (var i = 0; i < maxDigits; i++)
            {
                line.Clear();
                var newSignalLine = signal.ToList();
                var dupCount = i + 1;
                var total = 0;
                for (var j = 0; j < maxDigits; j++)
                {
                    var val = GetPatternDigit(dupCount, j);

                    var tmp = $"{newSignalLine[j]}*{val}";
                    line.Append($"{tmp,-5}");
                    if (j + 1 < maxDigits)
                        line.Append(" + ");
                    newSignalLine[j] *= val;
                    total += newSignalLine[j];
                }
                total = Helper.GetDigitRight(Math.Abs(total), 1);
                line.Append($" = {total}");

                newSignal.Add(total);

                Debug(line.ToString());
            }

            signal = newSignal;

            Debug("");
            Debug($"After {phase} Phase: {ListToString(signal)}");
            Debug("");
        }

        var result = ListToString(signal.Take(8).ToList());

        return Helper.GetPuzzleResultText($"first 8 digits after {phase} phases : {result}", result, puzzleData.ExpectedAnswer);
    }

    int GetPatternDigit(int repeatCount, int idx)
    {
        idx++;
        var a = basePattern.Count * repeatCount;
        while (idx >= a)
            idx -= a;
        var i = idx / repeatCount;
        return basePattern[i];
    }

}