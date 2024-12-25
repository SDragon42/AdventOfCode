using System.Data;

namespace AdventOfCode.CSharp.Year2024;

public class Day11(ITestOutputHelper output)
{
    private const int DAY = 11;



    private (IList<long> input, long? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.ReadText(DAY, inputName)
                               .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                               .Select(long.Parse) 
                               .ToList();

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
                                  ?.ToInt64();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1", 1)]
    [InlineData(1, "example2", 25)]
    [InlineData(1, "input", 25)]
    public void Part1(int part, string inputName, int numBlinks)
    {
        var (stones, expected) = GetTestData(part, inputName);

        for (int i = 0; i < numBlinks; i++)
        {
            BlinkStones(stones);
        }

        var value = stones.Count;

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }

    private void BlinkStones(IList<long> stones)
    {
        for (int i = 0; i < stones.Count; i++)
        {
            if (Rule_IsZero(stones[i]))
            {
                stones[i] = 1;
            }
            else if (Rule_HasEvenNumberDigits(stones[i]))
            {
                var tNum = stones[i].ToString();
                var len = tNum.Length / 2;
                var s1 = tNum.Substring(0, len);
                var s2 = tNum.Substring(len);
                try
                {
                    stones[i] = s2.ToInt32();
                    stones.Insert(i, s1.ToInt32());
                }
                catch (Exception)
                {
                    throw;
                }
                
                i++;
            }
            else
            {
                stones[i] = stones[i] * 2024;
            }
        }
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
