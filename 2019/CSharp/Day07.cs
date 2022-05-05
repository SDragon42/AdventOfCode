namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/7
/// </summary>
public class Day07
{

    public long RunPart1(List<long> code, List<long> fixedPhase)
    {
        var phaseValues = new long[] { 0, 1, 2, 3, 4 };
        var answer = 0L;

        foreach (var phase in GetPhases(phaseValues, fixedPhase))
        {
            var ampA = new IntCode(code);
            var ampB = new IntCode(code);
            var ampC = new IntCode(code);
            var ampD = new IntCode(code);
            var ampE = new IntCode(code);

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

        return answer;
    }

    public long RunPart2(List<long> code, List<long> fixedPhase)
    {
        var phaseValues = new long[] { 5, 6, 7, 8, 9 };
        var answer = 0L;

        foreach (var phase in GetPhases(phaseValues, fixedPhase))
        {
            var ampA = new IntCode(code);
            var ampB = new IntCode(code);
            var ampC = new IntCode(code);
            var ampD = new IntCode(code);
            var ampE = new IntCode(code);

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

        return answer;
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
