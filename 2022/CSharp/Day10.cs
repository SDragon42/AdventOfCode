using System.Collections.ObjectModel;
using System.Text;

namespace AdventOfCode.CSharp.Year2022;

public class Day10_Cathode_Ray_Tube
{
    private const int DAY = 10;

    private readonly ITestOutputHelper output;
    public Day10_Cathode_Ray_Tube(ITestOutputHelper output) => this.output = output;



    private (List<string> input, int? expected) GetTestData_Part1(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName)
            .ToList();

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (input, expected);
    }

    private (List<string> input, string expected) GetTestData_Part2(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName)
            .ToList();

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}");

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData_Part1(part, inputName);

        var value = SumTheSixSignalStrengths(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }

    


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData_Part2(part, inputName);

        var value = BuildOuputMessage(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private int SumTheSixSignalStrengths(List<string> input)
    {
        var readCycles = new int[] { 20, 60, 100, 140, 180, 220 };
        var instructions = ParseInput(input);
        var signalStrengths = new List<int>();
        var cpu = new ScreenCpu(instructions);

        while (cpu.Cycle <= 220)
        {
            if (readCycles.Contains(cpu.Cycle))
            {
                var strength = cpu.Cycle * cpu.X;
                signalStrengths.Add(strength);
            }
            cpu.RunCycle();
        }

        var result = signalStrengths.Sum();
        return result;
    }

    private string BuildOuputMessage(List<string> input)
    {
        var instructions = ParseInput(input);
        var cpu = new ScreenCpu(instructions);
        var output = new char[240];
        
        while (cpu.Cycle <= 240)
        {
            var idx = cpu.Cycle - 1;
            var column = idx % 40;
            output[idx] = InRange(column, cpu.X) ? '#' : '.';
            cpu.RunCycle();
        }

        var result = string.Join(Environment.NewLine, AssembleLines());
        return result;

        bool InRange(int i, int value)
        {
            return ((i - 1) == value)
                || (i == value)
                || ((i + 1) == value);
        }

        IEnumerable<string> AssembleLines()
        {
            var i = 0;
            var step = 40;
            while (i < step * 6)
            {
                var line = new string(output[i..(i + step)]);
                i += step;
                yield return line;
            }
        }
    }

    private IEnumerable<(CpuInstruction command, int? value)> ParseInput(List<string> input)
    {
        foreach (var line in input)
        {
            var lineSpan = line.AsSpan();

            var cmdText = lineSpan[0..4];
            Enum.TryParse<CpuInstruction>(cmdText, out var cmd);

            var instruction = cmd switch
            {
                CpuInstruction.noop => (cmd, (int?)null),
                CpuInstruction.addx => (cmd, int.Parse(lineSpan[5..])),
                _ => throw new ApplicationException("Bad parse")
            };
            yield return instruction;
        }
    }


    private enum CpuInstruction { noop, addx }


    private class ScreenCpu
    {
        private readonly ReadOnlyCollection<(CpuInstruction command, int? value)> _instructions;
        private int _instructionIdx = 0;

        private int _cyclesToComplete = 0;
        private CpuInstruction? _CachedInstruction;
        private int? _CachedValue = null;

        public ScreenCpu(IEnumerable<(CpuInstruction, int?)> instructions)
        {
            _instructions = instructions.ToList().AsReadOnly();
            Cycle = 1;
            X = 1;
        }

        public int Cycle { get; private set; }
        public int X { get; private set; }


        public bool RunCycle()
        {
            if (_instructionIdx >= _instructions.Count && !_CachedInstruction.HasValue)
                return false;

            // Read next Command if one is not cached
            if (!_CachedInstruction.HasValue)
            {
                (_CachedInstruction, _CachedValue) = _instructions[_instructionIdx++];
                _cyclesToComplete = _CachedInstruction.Value switch
                {
                    CpuInstruction.noop => 1,
                    CpuInstruction.addx => 2,
                    _ => throw new ApplicationException($"Unknown instruction: {_CachedInstruction} {_CachedValue}")
                };
            }

            // Cycle
            Cycle++;

            // Attempt to finish command execution
            if (_cyclesToComplete > 0)
            {
                _cyclesToComplete--;
                if (_cyclesToComplete == 0)
                {
                    X += _CachedInstruction switch
                    {
                        CpuInstruction.addx => _CachedValue.Value,
                        _ => 0
                    };

                    _CachedInstruction = null;
                    _CachedValue = null;
                }
            }

            return true;
        }

    }
}