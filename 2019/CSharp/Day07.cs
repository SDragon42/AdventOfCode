namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/7
/// </summary>
class Day07 : PuzzleBase
{
    const int DAY = 7;


    public override IEnumerable<string> SolvePuzzle()
    {
        yield return "Day 7: Amplification Circuit";

        yield return string.Empty;
        yield return RunExample(Example1);
        yield return RunExample(Example2);
        yield return RunExample(Example3);
        yield return RunProblem(Part1);

        yield return string.Empty;
        yield return RunExample(Example4);
        yield return RunExample(Example5);
        yield return RunProblem(Part2);
    }

    string Example1() => " Ex. 1) " + RunPart1(GetPuzzleData(1, "example1"));
    string Example2() => " Ex. 2) " + RunPart1(GetPuzzleData(1, "example2"));
    string Example3() => " Ex. 3) " + RunPart1(GetPuzzleData(1, "example3"));
    string Part1() => "Part 1) " + RunPart1(GetPuzzleData(1, "input"));

    string Example4() => " Ex. 2) " + RunPart2(GetPuzzleData(2, "example4"));
    string Example5() => " Ex. 2) " + RunPart2(GetPuzzleData(2, "example5"));
    string Part2() => "Part 2) " + RunPart2(GetPuzzleData(2, "input"));


    class InputAnswer : IntCodeInputAnswer<long?>
    {
        protected override void SetInput(List<string> value)
        {
            base.SetInput(value);
            Phase = Input.Skip(1).FirstOrDefault()
                ?.Split(',')
                ?.Select(v => v.ToInt64())
                ?.ToList();
        }

        public List<long> Phase { get; set; }
    }
    InputAnswer GetPuzzleData(int part, string name)
    {
        var result = new InputAnswer()
        {
            Input = InputHelper.LoadInputFile(DAY, name).ToList(),
            ExpectedAnswer = InputHelper.LoadAnswerFile(DAY, part, name)?.FirstOrDefault()?.ToInt64()
        };
        return result;
    }


    string RunPart1(InputAnswer puzzleData)
    {
        var phaseValues = new long[] { 0, 1, 2, 3, 4 };
        var answer = 0L;

        foreach (var phase in GetPhases(phaseValues, puzzleData.Phase))
        {
            var ampA = new IntCode(puzzleData.Code);
            var ampB = new IntCode(puzzleData.Code);
            var ampC = new IntCode(puzzleData.Code);
            var ampD = new IntCode(puzzleData.Code);
            var ampE = new IntCode(puzzleData.Code);

            var outputValue = 0L;

            ampA.Output += (s, e) => outputValue = e.OutputValue;
            ampB.Output += (s, e) => outputValue = e.OutputValue;
            ampC.Output += (s, e) => outputValue = e.OutputValue;
            ampD.Output += (s, e) => outputValue = e.OutputValue;
            ampE.Output += (s, e) => outputValue = e.OutputValue;

            ampA.AddInput(phase[0], outputValue);
            ampA.Run();

            ampB.AddInput(phase[1], outputValue);
            ampB.Run();

            ampC.AddInput(phase[2], outputValue);
            ampC.Run();

            ampD.AddInput(phase[3], outputValue);
            ampD.Run();

            ampE.AddInput(phase[4], outputValue);
            ampE.Run();

            if (outputValue > answer)
                answer = outputValue;
        }

        return Helper.GetPuzzleResultText($"Highest signal that can be sent to the thrusters: {answer}", answer, puzzleData.ExpectedAnswer);
    }

    string RunPart2(InputAnswer puzzleData)
    {
        var phaseValues = new long[] { 5, 6, 7, 8, 9 };
        var answer = 0L;

        foreach (var phase in GetPhases(phaseValues, puzzleData.Phase))
        {
            var ampA = new IntCode(puzzleData.Code);
            var ampB = new IntCode(puzzleData.Code);
            var ampC = new IntCode(puzzleData.Code);
            var ampD = new IntCode(puzzleData.Code);
            var ampE = new IntCode(puzzleData.Code);

            var outputValue = 0L;

            ampA.Output += (s, e) => outputValue = e.OutputValue;
            ampB.Output += (s, e) => outputValue = e.OutputValue;
            ampC.Output += (s, e) => outputValue = e.OutputValue;
            ampD.Output += (s, e) => outputValue = e.OutputValue;
            ampE.Output += (s, e) => outputValue = e.OutputValue;

            ampA.AddInput(phase[0]);
            ampB.AddInput(phase[1]);
            ampC.AddInput(phase[2]);
            ampD.AddInput(phase[3]);
            ampE.AddInput(phase[4]);

            while (true)
            {
                ampA.AddInput(outputValue);
                ampA.Run();

                ampB.AddInput(outputValue);
                ampB.Run();

                ampC.AddInput(outputValue);
                ampC.Run();

                ampD.AddInput(outputValue);
                ampD.Run();

                ampE.AddInput(outputValue);
                ampE.Run();
                if (ampE.State == IntCodeState.Finished) break;
            }

            if (outputValue > answer)
                answer = outputValue;
        }

        return Helper.GetPuzzleResultText($"Highest signal that can be sent to the thrusters: {answer}", answer, puzzleData.ExpectedAnswer);
    }

    IEnumerable<IList<long>> GetPhases(IList<long> sourceValues, IList<long> fixedPhase = null)
    {
        if (fixedPhase != null)
        {
            yield return fixedPhase;
            yield break;
        }

        var result = GetPermutations(sourceValues);
        foreach (var item in result)
            yield return item.ToList();
    }

    /// <summary>
    /// Returns a list of all possible combinations of the item list.  (item list only tested with unique values)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    /// <remarks>
    /// Sourced and modified from:
    /// https://stackoverflow.com/questions/5132758/words-combinations-without-repetition
    /// </remarks>
    public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items)
    {
        if (items.Count() == 1)
        {
            yield return new T[] { items.First() };
            yield break;
        }

        foreach (var item in items)
        {
            var nextItems = items.Where(i => !i.Equals(item));
            foreach (var result in GetPermutations(nextItems))
                yield return new T[] { item }.Concat(result);
        }
    }

}
