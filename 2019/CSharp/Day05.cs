namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/5
/// </summary>
public class Day05
{
    public long RunPart1(List<long> input, long inputValue)
    {
        var answer = 0L;
        var computer = new IntCode(input);
        computer.Output += (s, e) => { answer = e.OutputValue; };

        computer.Run();
        if (computer.State == IntCodeState.NeedsInput)
        {
            computer.AddInput(inputValue);
            computer.Run();
        }

        return answer;
    }


    public long RunPart2(List<long> input)
    {
        var answer = 0L;
        var computer = new IntCode(input);
        computer.Output += (s, e) => { answer = e.OutputValue; };

        computer.Run();
        if (computer.State == IntCodeState.NeedsInput)
        {
            computer.AddInput(5);
            computer.Run();
        }

        return answer;
    }


    IEnumerable<string> ShowState(IntCode comp)
    {
        yield return $"State: {comp.State}   Address: {comp.AddressPointer}";
        yield return comp.Dump();
        yield return string.Empty;
    }

}
