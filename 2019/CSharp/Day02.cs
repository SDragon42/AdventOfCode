namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/2
/// </summary>
public class Day02
{
    public long RunCode(List<long> code, long valueAt1, long valueAt2)
    {
        var computer = new IntCode(code);
        if (valueAt1 >= 0) computer.Poke(1, valueAt1);
        if (valueAt2 >= 0) computer.Poke(2, valueAt2);
        computer.Run();

        var valAt0 = computer.Peek(0);
        return valAt0;
    }


    public int FindNounVerb(List<long> input, long desiredValueAt0)
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
