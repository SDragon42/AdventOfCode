namespace AdventOfCode.CSharp.Year2022;

using CreateDictionary = Dictionary<int, Stack<char>>;

public class Day05_Supply_Stacks
{
    private const int DAY = 5;

    private readonly ITestOutputHelper output;
    public Day05_Supply_Stacks(ITestOutputHelper output) => this.output = output;

    

    private (List<string> input, string? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.LoadInputFile(DAY, inputName)
            .ToList();

        var expected = InputHelper.LoadAnswerFile(DAY, part, inputName)
            ?.FirstOrDefault();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = FindTheTopCrateOfEachStack(input, MoveCrates_9000);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = FindTheTopCrateOfEachStack(input, MoveCrates_9001);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private string FindTheTopCrateOfEachStack(List<string> input, Action<CreateDictionary, string> craneOperation)
    {
        var (crates, instructions) = ParseInput(input);

        foreach (var instruction in instructions)
            craneOperation(crates, instruction);

        var result = ReadTopCrates(crates);
        return result;
    }

    private void MoveCrates_9000(CreateDictionary crates, string instruction)
    {
        var (number, from, to) = ParseInstruction(instruction);

        var tmpQueue = new Queue<char>();
        while (number > 0)
        {
            number--;
            var crate = crates[from].Pop();
            tmpQueue.Enqueue(crate);
        }

        while (tmpQueue.Count > 0)
        {
            var crate = tmpQueue.Dequeue();
            crates[to].Push(crate);
        }
    }

    private void MoveCrates_9001(CreateDictionary crates, string instruction)
    {
        var (number, from, to) = ParseInstruction(instruction);

        var tmpStack = new Stack<char>();
        while (number > 0)
        {
            number--;
            var crate = crates[from].Pop();
            tmpStack.Push(crate);
        }

        while (tmpStack.Count > 0)
        {
            var crate = tmpStack.Pop();
            crates[to].Push(crate);
        }
    }

    private string ReadTopCrates(CreateDictionary crates)
    {
        var chars = crates
            .Select(kv => kv.Value.Peek())
            .ToArray();
        return new string(chars);
    }

    private (CreateDictionary crates, List<string> instructions) ParseInput(List<string> input)
    {
        var blankIndex = input.IndexOf(string.Empty);

        var crateInput = input.Take(blankIndex)
            .Reverse()
            .ToList();

        var crates = GetStackInfo(crateInput[0])
            .ToDictionary(
                key => key - 48,
                value => new Stack<char>());

        foreach (var line in crateInput.Skip(1))
        {
            var info = GetStackInfo(line.AsSpan());
            for (var i = 0; i < info.Count; i++)
            {
                var val = info[i];
                if (val != ' ')
                    crates[i + 1].Push(val);
            }
        }

        var instructions = input.Skip(blankIndex + 1).ToList();

        return (crates, instructions);

        List<char> GetStackInfo(ReadOnlySpan<char> data)
        {
            const int Increment = 4;
            var idx = 1;

            var values = new List<char>();
            while (idx < data.Length)
            {
                values.Add(data[idx]);
                idx += Increment;
            }
            return values;
        }
    }

    private Regex instructionRegex = new Regex("move (?<x>.*) from (?<y>.*) to (?<z>.*)", RegexOptions.Compiled);

    private (int number, int from, int to) ParseInstruction(string instruction)
    {
        var match = instructionRegex.Match(instruction);
        if (!match.Success)
            throw new ApplicationException("Instruction does not match the pattern");
        var num = (int)Convert.ChangeType(match.Groups[1].Value, typeof(int));
        var from = (int)Convert.ChangeType(match.Groups[2].Value, typeof(int));
        var to = (int)Convert.ChangeType(match.Groups[3].Value, typeof(int));
        return (num, from, to);
    }
}