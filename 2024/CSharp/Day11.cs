namespace AdventOfCode.CSharp.Year2024;

public class Day11(ITestOutputHelper output)
{
    private const int DAY = 11;



    private (IDictionary<long, long> input, long? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadText(DAY, inputName)
                               .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                               .Select(long.Parse)
                               .GroupBy(key => key)
                               .Select(g => (g.Key, (long)g.Count()))
                               .ToDictionary();

        var expected = TestServices.Input.ReadText(DAY, $"{inputName}-answer{part}")
                                  ?.ToInt64();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1", 1)]
    [InlineData(1, "example2", 25)]
    [InlineData(1, "input", 25)]
    public void Part1(int part, string inputName, int numBlinks)
    {
        var (stonesDict, expected) = GetTestData(part, inputName);

        for (int i = 0; i < numBlinks; i++)
        {
            stonesDict = BlinkStones(stonesDict);
        }

        var value = stonesDict.Sum(kv => kv.Value);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(2, "input", 75)]
    public void Part2(int part, string inputName, int numBlinks)
    {
        var (stonesDict, expected) = GetTestData(part, inputName);

        for (int i = 0; i < numBlinks; i++)
        {
            stonesDict = BlinkStones(stonesDict);
        }

        var value = stonesDict.Sum(kv => kv.Value);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private IDictionary<long, long> BlinkStones(IDictionary<long, long> stonesDict)
    {
        var newStones = new Dictionary<long, long>();
        foreach (var stone in stonesDict)
        {
            if (Rule_IsZero(stone.Key))
            {
                MoveTo(newStones, stone, 1);
            }
            else if (Rule_HasEvenNumberDigits(stone.Key))
            {
                SplitStone(newStones, stone);
            }
            else
            {
                MoveTo(newStones, stone, stone.Key * 2024);
            }
        }
        return newStones;
    }

    private static void MoveTo(IDictionary<long, long> stonesDict, KeyValuePair<long, long> stone, long stoneNum)
    {
        stonesDict[stoneNum] = stonesDict.ContainsKey(stoneNum)
            ? stonesDict[stoneNum] + stone.Value
            : stone.Value;
    }

    private static void SplitStone(IDictionary<long, long> stonesDict, KeyValuePair<long, long> stone)
    {
        var tNum = stone.Key.ToString();
        var len = tNum.Length / 2;
        var s1 = tNum.Substring(0, len).ToInt64();
        var s2 = tNum.Substring(len).ToInt64();

        var count = stone.Value;

        MoveTo(stonesDict, stone, s1);
        MoveTo(stonesDict, stone, s2);
    }

    private bool Rule_IsZero(long stone)
    {
        return stone == 0;
    }

    private bool Rule_HasEvenNumberDigits(long stone)
    {
        var numDigits = (int)Math.Floor(Math.Log10(Math.Abs(stone)) + 1);
        return int.IsEvenInteger(numDigits);
    }
}
